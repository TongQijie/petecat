using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network
{
    internal class SocketObject : ITcpClientObject, ITcpListenerObject
    {
        public SocketObject(Socket socket)
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

        public event SocketDisconnectedHandlerDelegate SocketDisconnected;

        public void Connect(IPAddress address, int port)
        {
            Socket.Connect(address, port);

            Address = address;
            Port = port;

            BeginReceive();
        }

        public void Disconnect()
        {
            if (Socket.Connected)
            {
                Socket.Disconnect(false);
            }
        }

        public void Listen(IPEndPoint hostEndPoint)
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

            socketObject.BeginReceive(this);

            Socket.BeginAccept(AcceptCallback, this);
        }

        private byte[] _ReceiveBuffer = new byte[1024 * 4];

        public void BeginReceive(ISocketObject listener = null)
        {
            if (Socket.Connected)
            {
                Socket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, listener ?? this);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var owner = ar.AsyncState as SocketObject;

            var count = 0;
            try
            {
                count = Socket.EndReceive(ar);
            }
            catch (Exception)
            {
                if (owner.SocketDisconnected != null)
                {
                    owner.SocketDisconnected.Invoke(this);
                }

                Dispose();
                return;
            }

            if (count == 0)
            {
                if (owner.SocketDisconnected != null)
                {
                    owner.SocketDisconnected.Invoke(this);
                }

                Dispose();
                return;
            }

            if (owner.ReceivedData != null)
            {
                owner.ReceivedData.Invoke(this, _ReceiveBuffer, 0, count);
            }

            Socket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, ar.AsyncState);
        }

        public void Send(byte[] data, int offset, int count)
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
            Socket.Close();
        }
    }
}
