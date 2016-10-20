using Petecat.Service.Datagram;
using System;
using System.Text;

namespace Petecat.Service
{
    public class ServiceTcpResponse
    {
        public ServiceTcpResponse(ServiceTcpConnection connection)
        {
            _Connection = connection;
        }

        private ServiceTcpConnection _Connection = null;

        public byte Status { get; set; }

        public string ContentType { get; set; }

        public void WriteObject(object instance)
        {
            var data = new ServiceTcpResponseDatagram(instance, Status, ContentType).Wrap();
            _Connection.TcpClientObject.BeginSend(data, 0, data.Length);
        }
    }
}
