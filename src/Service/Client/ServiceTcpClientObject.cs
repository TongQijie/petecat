using Petecat.Extension;
using Petecat.Network.Sockets;
using Petecat.Service.Datagram;
using System;
using System.Net;

namespace Petecat.Service.Client
{
    public class ServiceTcpClientObject : IDisposable
    {
        public ServiceTcpClientObject(string address, int port)
        {
            Datagram = new ServiceTcpResponseDatagram(new byte[0]);
            _Address = IPAddress.Parse(address);
            _Port = port;
        }

        private IPAddress _Address = null;

        private int _Port = 0;

        public ITcpClientObject TcpClientObject { get; private set; }

        private ServiceTcpResponseDatagram Datagram { get; set; }

        private bool _IsGotDatagram = false;

        public byte[] GetResponse(byte[] request, int timeout = 30000)
        {
            TcpClientObject = SocketFactory.CreateTcpClientObject();
            TcpClientObject.Connect(_Address, _Port);
            TcpClientObject.Send(request, 0, request.Length);
            TcpClientObject.ReceivedData += TcpClientObject_ReceivedData;

            int costTime = 0;
            while (!_IsGotDatagram && costTime < timeout)
            {
                Threading.ThreadBridging.Sleep(50);
                costTime += 50;
            }

            if (costTime >= timeout)
            {
                throw new TimeoutException();
            }

            return Datagram.Bytes;
        }

        private void TcpClientObject_ReceivedData(ISocketObject socketObject, byte[] data, int offset, int count)
        {
            Datagram.Append(data, offset, count);
            _IsGotDatagram = Datagram.Validate();
        }

        public void Dispose()
        {
            if (TcpClientObject != null)
            {
                TcpClientObject.Disconnect();
                TcpClientObject.Dispose();
            }
        }
    }
}
