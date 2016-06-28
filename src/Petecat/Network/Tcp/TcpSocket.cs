using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Petecat.Network.Tcp
{
    public class TcpSocket
    {
        private Socket _Socket = null;

        private List<byte[]> _SendQueue = null;

        private int _Offset = 0;

        public bool Connected
        {
            get { return _Socket != null && _Socket.Connected; }
        }

        public TcpSocket()
        {
            _SendQueue = new List<byte[]>();
        }

        public void CreateSocket()
        {
            if (_Socket != null)
            {
                _Socket.Close();
            }
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string server, int port)
        {
            CreateSocket();

            _Socket.Blocking = true;
            _Socket.Connect(server, port);
            _Socket.Blocking = false;
        }

        public void Connect(IPAddress[] ips, int port)
        {
            this.CreateSocket();

            _Socket.Blocking = true;
            _Socket.Connect(ips, port);
            _Socket.Blocking = false;
        }

        public void Disconnect(bool reuse)
        {
            if (_Socket != null)
            {
                _Socket.Shutdown(SocketShutdown.Both);
                _Socket.Disconnect(reuse);
                _Socket.Close();
                _Socket = null;
            }
        }

        public void Send(byte[] data)
        {
            if (_SendQueue.Count > 0)
            {
                _SendQueue.Add(data);
            }
            else
            {
                _SendQueue.Add(data);
                _Offset = 0;
            }

            TrySendout();
        }

        public void TrySendout()
        {
            while (_SendQueue.Count > 0)
            {
                byte[] array = _SendQueue[0];
                int num = this.Sendout(array, _Offset);
                if (num <= 0)
                {
                    break;
                }
                _Offset += num;
                if (_Offset != array.Length)
                {
                    break;
                }
                _Offset = 0;
                _SendQueue.RemoveAt(0);
            }
        }

        public int Receive(byte[] buf, int offset, int size)
        {
            SocketError error;
            return _Socket.Receive(buf, offset, size, SocketFlags.None, out error);
        }

        private int Sendout(byte[] data, int offset)
        {
            return this._Socket.Send(data, offset, data.Length - offset, SocketFlags.None);
        }
    }
}
