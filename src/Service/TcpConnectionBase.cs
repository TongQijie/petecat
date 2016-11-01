using Petecat.Network.Sockets;
using Petecat.Service.Datagram;

namespace Petecat.Service
{
    public class TcpConnectionBase
    {
        public TcpConnectionBase()
        {
            Datagram = new ServiceTcpRequestDatagram(new byte[0]);
        }

        public TcpConnectionBase(ISocketObject socketObject) : base()
        {
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

        public void Reset(ISocketObject socketObject)
        {
            SocketObject = socketObject;
            Datagram.Clear();
            IsDisposed = false;
        }

        public bool IsDisposed { get; set; }
    }
}
