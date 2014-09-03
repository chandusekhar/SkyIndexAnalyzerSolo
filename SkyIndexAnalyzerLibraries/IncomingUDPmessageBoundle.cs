using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SkyIndexAnalyzerLibraries
{
    public class IncomingUDPmessageBoundle
    {
        public string udpMessage = "";
        public int devID = 1;
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
                //int idx = message.IndexOf('>');
                string strDevIDTag = message.Substring(idxStartDevIDtag, 5); // "<id23>"

                try
                {
                    strDevIDTag = strDevIDTag.Substring(3); // "23>"
                    int idx2 = strDevIDTag.IndexOf('>'); // 2
                    strDevIDTag = strDevIDTag.Substring(0, idx2); // "23"
                    devID = Convert.ToInt32(strDevIDTag);
                }
                catch (Exception)
                {
                    devID = 0;
                }

                udpMessage = message.Substring(0, idxStartDevIDtag) + message.Substring(idx + 1);
            }
            else
            {
                devID = 0;
            }
        }
    }
}
