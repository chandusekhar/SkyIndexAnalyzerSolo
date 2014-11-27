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
    }
}
