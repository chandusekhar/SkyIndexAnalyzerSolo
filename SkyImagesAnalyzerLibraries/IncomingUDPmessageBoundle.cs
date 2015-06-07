using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyImagesAnalyzerLibraries
{
    public class IncomingUDPmessageBoundle
    {
        public string srcUDPmessage = "";
        public string udpMessage = "";
        public int devID = 1;
        public bool isReplyMessage = false;
        //public bool isErrorMessage = false;
        public string ipAddrString = "";
        private SocketAddress remoteSocketAddress = new SocketAddress(AddressFamily.InterNetwork);
        public bool isPartOfMessage = false;

        private string plusSign = "<+>";
        private string plusplusSign = "<++>";
        private string msgIDsign = "<mid:";
        private string msgSubIDsign = "<sub:";
        public long mID = 0;
        public int mSubID = 0;
        public bool isStartOfTuple = false;
        public bool isEndOfTuple = false;


        public IncomingUDPmessageBoundle()
        { }

        public IncomingUDPmessageBoundle(SocketAddress remSktAddr, string message)
        {
            srcUDPmessage = message;
            remoteSocketAddress = remSktAddr;
            string addrStr = remoteSocketAddress.ToString(); //InterNetwork:16:{21,179,192,168,192,221,0,0,0,0,0,0,0,0}
            addrStr = addrStr.Substring(addrStr.IndexOf("{") + 1);
            addrStr = addrStr.Substring(0, addrStr.Length - 1);
            char[] splitChar = { ',' };
            string[] sktAddrStrArray = addrStr.Split(splitChar);
            ipAddrString = sktAddrStrArray[2] + "." + sktAddrStrArray[3] + "." + sktAddrStrArray[4] + "." + sktAddrStrArray[5];


            if ((message.IndexOf(plusplusSign) > -1) || (message.IndexOf(plusSign) > -1))
            {
                isPartOfMessage = true;

                if ((message.Substring(0, 3) == plusSign) && (message.Substring(message.Length-3) == plusSign))
                {
                    // середина кортежа
                    udpMessage = message.Replace(plusSign, "");

                }
                else if ((message.Substring(0, 4) == plusplusSign) && (message.Substring(message.Length - 3) == plusSign))
                {
                    // начало кортежа
                    udpMessage = message.Replace(plusSign, "");
                    udpMessage = udpMessage.Replace(plusplusSign, "");
                    isStartOfTuple = true;
                }
                else if ((message.Substring(0, 3) == plusSign) && (message.Substring(message.Length - 4) == plusplusSign))
                {
                    // конец кортежа
                    udpMessage = message.Replace(plusSign, "");
                    udpMessage = udpMessage.Replace(plusplusSign, "");
                    isEndOfTuple = true;
                }


                if ((udpMessage.Length >= msgIDsign.Length) && (udpMessage.IndexOf(msgIDsign) >= 0))
                {
                    int idxStartMsgIDtag = udpMessage.IndexOf(msgIDsign);
                    int idxEndMsgIDtag = udpMessage.IndexOf('>', idxStartMsgIDtag);
                    string strMsgIDTag = udpMessage.Substring(idxStartMsgIDtag, idxEndMsgIDtag - idxStartMsgIDtag + 1); // "<mid:615253478172>"

                    try
                    {
                        strMsgIDTag = strMsgIDTag.Replace(msgIDsign, "");
                        strMsgIDTag = strMsgIDTag.Replace(">", "");
                        mID = Convert.ToInt32(strMsgIDTag);
                    }
                    catch (Exception)
                    {
                        mID = 0;
                    }

                    udpMessage = udpMessage.Replace(msgIDsign + mID + ">", "");
                }
                else
                {
                    mID = 0;
                }



                if ((udpMessage.Length >= msgSubIDsign.Length) && (udpMessage.IndexOf(msgSubIDsign) >= 0))
                {
                    int idxStartMsgSubIDtag = udpMessage.IndexOf(msgSubIDsign);
                    int idxEndMsgSubIDtag = udpMessage.IndexOf('>', idxStartMsgSubIDtag);
                    string strMsgSubIDTag = udpMessage.Substring(idxStartMsgSubIDtag, idxEndMsgSubIDtag - idxStartMsgSubIDtag + 1); // "<mid:615253478172>"

                    try
                    {
                        strMsgSubIDTag = strMsgSubIDTag.Replace(msgSubIDsign, "");
                        strMsgSubIDTag = strMsgSubIDTag.Replace(">", "");
                        mSubID = Convert.ToInt32(strMsgSubIDTag);
                    }
                    catch (Exception)
                    {
                        mSubID = 0;
                    }

                    udpMessage = udpMessage.Replace(msgSubIDsign + mSubID + ">", "");
                }
                else
                {
                    mSubID = 0;
                }
            }
            else
            {
                udpMessage = message;
                srcUDPmessage = message;
            }




            if ((udpMessage.Length >= 5) && (udpMessage.IndexOf("<id") >= 0))
            {
                int idxStartDevIDtag = udpMessage.IndexOf("<id");
                string strDevIDTag = udpMessage.Substring(idxStartDevIDtag, 5); // "<id2>"

                try
                {
                    strDevIDTag = strDevIDTag.Replace("<id", "");
                    strDevIDTag = strDevIDTag.Replace(">", "");
                    devID = Convert.ToInt32(strDevIDTag);
                }
                catch (Exception)
                {
                    devID = 0;
                }

                udpMessage = udpMessage.Replace("<id" + devID + ">", "");
            }
            else
            {
                devID = 0;
            }


            if (isPartOfMessage)
            {
                if ((udpMessage.Length >= 6) && (udpMessage.IndexOf("<repl>") >= 0))
                {
                    udpMessage = udpMessage.Replace("<repl>", "");
                    isReplyMessage = true;
                }

                //if ((udpMessage.Length >= 5) && (udpMessage.IndexOf("<err>") >= 0))
                //{
                //    udpMessage = udpMessage.Replace("<err>", "");
                //    isErrorMessage = true;
                //}
            }
        }




        public static IncomingUDPmessagesBoundlesTuple operator +(IncomingUDPmessageBoundle msg1, IncomingUDPmessageBoundle msg2)
        {
            if ((msg1.devID == msg2.devID) && (msg1.mID == msg2.mID))
            {
                return new IncomingUDPmessagesBoundlesTuple(msg1) + msg2;
            }
            else
            {
                return null;
            }
        }

    }




    public class IncomingUDPmessagesBoundlesTuple
    {
        public List<IncomingUDPmessageBoundle> messageBoundles = new List<IncomingUDPmessageBoundle>();
        public long mID = 0;
        public int devID = 0;

        public bool IsComplete
        {
            get
            {
                bool retVal = true;
                retVal = retVal && (messageBoundles.FindIndex(msg => msg.isStartOfTuple) > -1);
                retVal = retVal && (messageBoundles.FindIndex(msg => msg.isEndOfTuple) > -1);

                if (!retVal)
                {
                    return retVal;
                }

                int tupleMaxMsgIdx = messageBoundles.Find(msg => msg.isEndOfTuple).mSubID;
                List<int> subIdxList = new List<int>();
                for (int i = 0; i <= tupleMaxMsgIdx; i++) subIdxList.Add(i);
                if (retVal)
                {
                    retVal = retVal && (!subIdxList.Except(messageBoundles.ConvertAll<int>(msg => msg.mSubID)).Any());
                }

                return retVal;
            }
        }



        public IncomingUDPmessagesBoundlesTuple()
        {
        }



        public IncomingUDPmessagesBoundlesTuple(IncomingUDPmessageBoundle fstMsg)
        {
            messageBoundles.Add(fstMsg);
            mID = fstMsg.mID;
            devID = fstMsg.devID;
        }



        public bool AddSubMessage(IncomingUDPmessageBoundle inMsg)
        {
            if ((inMsg.mID != mID) || (inMsg.devID != devID))
            {
                return false;
            }
            messageBoundles.Add(inMsg);
            messageBoundles.Sort((msg1, msg2) => msg1.mSubID.CompareTo(msg2.mSubID));
            return true;
        }


        public static IncomingUDPmessagesBoundlesTuple operator +(
            IncomingUDPmessagesBoundlesTuple tpl, IncomingUDPmessageBoundle msg)
        {
            tpl.AddSubMessage(msg);
            return tpl;
        }




        public IncomingUDPmessageBoundle CompleteMessage
        {
            get
            {
                if (IsComplete)
                {
                    return new IncomingUDPmessageBoundle()
                    {
                        srcUDPmessage = "",
                        udpMessage = messageBoundles.Aggregate<IncomingUDPmessageBoundle, string>("", (outstr, bndl) => outstr + bndl.udpMessage),
                        devID = devID,
                        isReplyMessage = messageBoundles[0].isReplyMessage,
                        //isErrorMessage = messageBoundles[0].isErrorMessage,
                        ipAddrString = messageBoundles[0].ipAddrString,
                        isPartOfMessage = false,
                        mID = messageBoundles[0].mID,
                    };
                }
                else return null;
            }
        }
    }
}
