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
        public string udpMessage = "";
        public int devID = 1;
        public bool isReplyMessage = false;
        public bool isErrorMessage = false;
        public string ipAddrString = "";
        private SocketAddress remoteSocketAddress = new SocketAddress(AddressFamily.InterNetwork);
        public bool isPartOfMessage = false;


        public IncomingUDPmessageBoundle()
        { }

        public IncomingUDPmessageBoundle(SocketAddress remSktAddr, string message)
        {
            remoteSocketAddress = remSktAddr;
            string addrStr = remoteSocketAddress.ToString(); //InterNetwork:16:{21,179,192,168,192,221,0,0,0,0,0,0,0,0}
            addrStr = addrStr.Substring(addrStr.IndexOf("{") + 1);
            addrStr = addrStr.Substring(0, addrStr.Length - 1);
            char[] splitChar = { ',' };
            string[] sktAddrStrArray = addrStr.Split(splitChar);
            ipAddrString = sktAddrStrArray[2] + "." + sktAddrStrArray[3] + "." + sktAddrStrArray[4] + "." + sktAddrStrArray[5];


            if ((message.Substring(0, 1) == "+") && (message.Last() == '+'))
            {
                message = message.Substring(1, message.Length - 2);
                isPartOfMessage = true;
            }


            if ((message.Length >= 5) && (message.IndexOf("<id") >= 0))
            {
                int idxStartDevIDtag = message.IndexOf("<id");
                string strDevIDTag = message.Substring(idxStartDevIDtag, 5); // "<id2>"

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

                udpMessage = message.Replace("<id" + devID + ">", "");
            }
            else
            {
                devID = 0;
            }

            if ((udpMessage.Length >= 6) && (udpMessage.IndexOf("<repl>") >= 0))
            {
                udpMessage = udpMessage.Replace("<repl>", "");
                isReplyMessage = true;
            }

            if ((udpMessage.Length >= 5) && (udpMessage.IndexOf("<err>") >= 0))
            {
                udpMessage = udpMessage.Replace("<err>", "");
                isErrorMessage = true;
            }
        }




        public static IncomingUDPmessageBoundle operator +(IncomingUDPmessageBoundle msg1, IncomingUDPmessageBoundle msg2)
        {
            if ((msg1.devID == msg2.devID) &&
                (msg1.isReplyMessage == msg2.isReplyMessage) &&
                (msg1.isErrorMessage == msg2.isErrorMessage) &&
                (msg1.ipAddrString == msg2.ipAddrString) &&
                (msg1.isPartOfMessage == msg2.isPartOfMessage))
            {
                IncomingUDPmessageBoundle retMsg = new IncomingUDPmessageBoundle()
                {
                    udpMessage = msg1.udpMessage + msg2.udpMessage,
                    devID = msg1.devID,
                    isReplyMessage = msg1.isReplyMessage,
                    isErrorMessage = msg1.isErrorMessage,
                    ipAddrString = msg1.ipAddrString,
                    isPartOfMessage = msg1.isPartOfMessage,
                };
                return retMsg;
            }
            else
            {
                return null;
            }
        }

    }
}
