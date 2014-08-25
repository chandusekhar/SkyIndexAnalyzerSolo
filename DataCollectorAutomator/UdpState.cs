using System.Net;
using System.Net.Sockets;
//using System.Linq;


namespace DataCollectorAutomator
{
    class UdpState
    {
        public IPEndPoint ipEndPoint;
        public UdpClient UDPclient;
        //public bool isBroadcasted;
        public SocketAddress sktAddress;
        public string udpMessage;
    }
}
