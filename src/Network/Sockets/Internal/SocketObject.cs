using System;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network.Sockets
{
    internal class SocketObject : ITcpClientObject, ITcpListenerObject
    {
        public SocketObject(Socket socket)
        {
            InternalSocket = socket;

            var remoteEndPoint = socket.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                Address = remoteEndPoint.Address;
                Port = remoteEndPoint.Port;
            }
        }

        public Socket InternalSocket { get; private set; }

        public IPAddress Address { get; private set; }

        public int Port { get; private set; }

        public event SocketReceivedDataHandlerDelegate ReceivedData;

        public event SocketConnectedHandlerDelegate SocketConnected;

        public event SocketDisconnectedHandlerDelegate SocketDisconnected;

        public void Connect(IPAddress address, int port)
        {
            InternalSocket.Connect(address, port);

            Address = address;
            Port = port;

            BeginReceive();
        }

        public void Disconnect()
        {
            if (InternalSocket.Connected)
            {
                InternalSocket.Disconnect(false);
            }
        }

        public void Listen(IPEndPoint hostEndPoint)
        {
            InternalSocket.Bind(hostEndPoint);

            InternalSocket.Listen(100);

            InternalSocket.BeginAccept(AcceptCallback, this);
        }

        public void Listen(int port)
        {
            InternalSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            InternalSocket.Listen(100);

            InternalSocket.BeginAccept(AcceptCallback, this);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            var socketObject = new SocketObject(InternalSocket.EndAccept(ar));

            if (SocketConnected != null)
            {
                SocketConnected(socketObject);
            }

            socketObject.BeginReceive(this);

            InternalSocket.BeginAccept(AcceptCallback, this);
        }

        private byte[] _ReceiveBuffer = new byte[1024 * 4];

        public void BeginReceive(ISocketObject listener = null)
        {
            if (InternalSocket.Connected)
            {
                InternalSocket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, listener ?? this);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            var owner = ar.AsyncState as SocketObject;

            var count = 0;
            try
            {
                count = InternalSocket.EndReceive(ar);
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

            if (InternalSocket.Connected)
            {
                InternalSocket.BeginReceive(_ReceiveBuffer, 0, _ReceiveBuffer.Length, SocketFlags.None, ReceiveCallback, ar.AsyncState);
            }
        }

        public void Send(byte[] data, int offset, int count)
        {
            if (InternalSocket.Connected)
            {
                InternalSocket.Send(data, offset, count, SocketFlags.None);
            }
        }

        public void BeginSend(byte[] data, int offset, int count)
        {
            if (InternalSocket.Connected)
            {
                InternalSocket.BeginSend(data, offset, count, SocketFlags.None, SendCallback, this);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            InternalSocket.EndSend(ar);
        }

        public void Dispose()
        {
            InternalSocket.Close();
        }
    }
}
