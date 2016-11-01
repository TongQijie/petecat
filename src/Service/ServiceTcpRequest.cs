using System;
using System.Text;

using Petecat.Data.Formatters;
using Petecat.Service.Datagram;

namespace Petecat.Service
{
    public class ServiceTcpRequest
    {
        static ServiceTcpRequest()
        {
            _Formatter = new JsonFormatter();
        }

        static IObjectFormatter _Formatter = null;

        public ServiceTcpRequest(ServiceTcpRequestDatagram datagram, TcpConnectionBase connection)
        {
            _Datagram = datagram;
            Connection = connection;
        }

        private ServiceTcpRequestDatagram _Datagram = null;

        public TcpConnectionBase Connection { get; private set; }

        public string ServiceName
        {
            get { return DecodeString(_Datagram.ServiceName); }
        }

        public string MethodName
        {
            get { return DecodeString(_Datagram.MethodName); }
        }

        public string ContentType
        {
            get { return DecodeString(_Datagram.ContentType); }
        }

        public object ReadObject(Type targetType)
        {
            return DecodeObject(_Datagram.Body, targetType);
        }

        public string DecodeString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public object DecodeObject(byte[] bytes, Type targetType)
        {
            return _Formatter.ReadObject(targetType, bytes, 0, bytes.Length);
        }
    }
}
