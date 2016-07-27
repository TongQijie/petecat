using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network
{
    public class SocketObject : ITcpClientObject, ITcpListenerObject
    {
        public static ITcpClientObject CreateTcpClientObject()
        {
            return new SocketObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) as ITcpClientObject;
        }

        public static ITcpListenerObject CreateTcpListenerObject()
        {
            return new SocketObject(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) as ITcpListenerObject; 
        }

        private SocketObject(Socket socket)
        {
            Socket = socket;

            var remoteEndPoint = socket.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                Address = remoteEndPoint.Address;
                Port = remoteEndPoint.Port;
            }
        }

        public Socket Socket { get; private set; }

        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        public event SocketReceivedDataHandlerDelegate ReceivedData;

        public event SocketConnectedHandlerDelegate SocketConnected;

        public void Connect(IPAddress address, int port)
        {
            Socket.Connect(address, port);

            Address = address;
            Port = port;
        }

        public void Disconnect()
        {
            if (Socket.Connected)
            {
                Socket.Disconnect(true);
            }
        }

        public void BeginListen(IPEndPoint hostEndPoint)
        {
            Socket.Bind(hostEndPoint);

            Socket.Listen(100);

            Socket.BeginAccept(AcceptCallback, this);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var socketObject = new SocketObject(Socket.EndAccept(ar));

            if (SocketConnected != null)
            {
                SocketConnected(socketObject);
            }

            socketObject.BeginReceive();

            Socket.BeginAccept(AcceptCallback, this);
        }

        private byte[] _ReceiveBuffer = new byte[1024 * 4];

        public void BeginReceive()
        {
            if (Socket.Connected)
            {
                Socket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, this);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var count = Socket.EndReceive(ar);
            if (count == 0)
            {
                return;
            }

            if (ReceivedData != null)
            {
                ReceivedData(ar.AsyncState as ISocketObject, _ReceiveBuffer, 0, count);
            }

            Socket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, this);
        }

        public void BeginSend(byte[] data, int offset, int count)
        {
            if (Socket.Connected)
            {
                Socket.BeginSend(data, offset, count, SocketFlags.None, SendCallback, this);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket.EndSend(ar);
        }

        public void Dispose()
        {
            Socket.Shutdown(SocketShutdown.Both);
            Socket.Close();
        }
    }
}
