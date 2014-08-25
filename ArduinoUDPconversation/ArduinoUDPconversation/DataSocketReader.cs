using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace ArduinoUDPconversation
{
    class DataSocketReader
    {
        private Socket UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private IPAddress Target_IP, SourceIP;
        private int Target_Port;
        //public static int bPause;
        public static ManualResetEvent allDone;
        public StateObject state;

        public DataSocketReader()
        {
            Target_IP = IPAddress.Parse("192.168.1.2");
            SourceIP = IPAddress.Parse("192.168.1.7");
            Target_Port = 5555;

            try
            {
                IPEndPoint LocalHostIPEnd = new IPEndPoint(Target_IP, Target_Port);
                UDPSocket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, 1);
                UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                UDPSocket.Bind(LocalHostIPEnd);
                //Recieve();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message + " " + e.StackTrace);
            }
        }

        public void Recieve()
        {
            allDone = new ManualResetEvent(false);
            try
            {
                IPEndPoint LocalIPEndPoint = new IPEndPoint(Target_IP, Target_Port);
                EndPoint LocalEndPoint = (EndPoint)LocalIPEndPoint;
                IPEndPoint RemoteIPEndPoint = new IPEndPoint(SourceIP, 0);
                EndPoint RemoteEndPoint = (EndPoint)RemoteIPEndPoint;
                state = new StateObject();
                state.workSocket = UDPSocket;
                //Console.WriteLine("Begin Recieve");
                allDone.Reset();
                //UDPSocket.BeginReceiveFrom(state.buffer, 0, state.BufferSize, 0, ref LocalEndPoint, new AsyncCallback(ReceiveCallback), state);
                UDPSocket.BeginReceiveFrom(state.buffer, 0, state.BufferSize, 0, ref RemoteEndPoint, new AsyncCallback(ReceiveCallback), state);
                allDone.WaitOne();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            //allDone.Set();
            IPEndPoint LocalIPEndPoint = new IPEndPoint(Target_IP, Target_Port);
            IPEndPoint RemoteIPEndPoint = new IPEndPoint(SourceIP, 0);
            EndPoint LocalEndPoint = (EndPoint)LocalIPEndPoint;
            EndPoint RemoteEndpoint = (EndPoint)RemoteIPEndPoint;
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            int bytesRead = client.EndReceiveFrom(ar, ref LocalEndPoint);

            client.BeginReceiveFrom(state.buffer, 0, state.BufferSize, 0, ref RemoteEndpoint, new AsyncCallback(ReceiveCallback), state);
        }


        public class StateObject
        {
            public int BufferSize = 9120;
            public Socket workSocket;
            public byte[] buffer;

            public StateObject()
            {
                buffer = new byte[BufferSize];
            }
        }
    }
}
