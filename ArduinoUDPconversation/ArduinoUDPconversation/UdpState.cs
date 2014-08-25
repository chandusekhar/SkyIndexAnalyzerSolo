﻿using System.Net;
using System.Net.Sockets;
//using System.Linq;


namespace ArduinoUDPconversation
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
