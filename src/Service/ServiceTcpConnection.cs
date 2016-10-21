using System;

using Petecat.Network.Sockets;
using Petecat.Extension;
using Petecat.Service.Datagram;

namespace Petecat.Service
{
    public class ServiceTcpConnection
    {
        public ServiceTcpConnection(ISocketObject socketObject)
        {
            Datagram = new ServiceTcpRequestDatagram(new byte[0]);
            SocketObject = socketObject;
        }

        public ServiceTcpRequestDatagram Datagram { get; private set; }

        public ISocketObject SocketObject { get; private set; }

        public event ServiceRequestArrivalHandlerDelegate ServiceRequestArrival = null;

        public void ReceiveData(byte[] data, int offset, int count)
        {
            Datagram.Append(data, offset, count);

            if (Datagram.Validate())
            {
                if (ServiceRequestArrival != null)
                {
                    Datagram.Unwrap();
                    ServiceRequestArrival.Invoke(new ServiceTcpRequest(Datagram, this));
                }

                Datagram.Clear();
            }
        }
    }
}
