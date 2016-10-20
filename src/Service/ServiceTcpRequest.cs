using System;
using System.Text;

using Petecat.Extension;
using Petecat.Service.Datagram;

namespace Petecat.Service
{
    public class ServiceTcpRequest
    {
        public ServiceTcpRequest(ServiceTcpRequestDatagram datagram, ServiceTcpConnection connection)
        {
            _Datagram = datagram;
            Connection = connection;
        }

        private ServiceTcpRequestDatagram _Datagram = null;

        public ServiceTcpConnection Connection { get; private set; }

        public string ServiceName { get { return _Datagram.ServiceName; } }

        public string MethodName { get { return _Datagram.MethodName; } }

        public string ContentType { get { return _Datagram.ContentType; } }

        public object ReadObject(Type targetType)
        {
            return _Datagram.ReadBody(targetType);
        }
    }
}
