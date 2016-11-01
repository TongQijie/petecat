using Petecat.Data.Formatters;
using Petecat.Service.Datagram;

using System.Text;

namespace Petecat.Service
{
    public class ServiceTcpResponse
    {
        static ServiceTcpResponse()
        {
            _Formatter = new JsonFormatter();
        }

        static IObjectFormatter _Formatter = null;

        public ServiceTcpResponse(TcpConnectionBase connection)
        {
            _Connection = connection;
        }

        private TcpConnectionBase _Connection = null;

        public byte Status { get; set; }

        public string ContentType { get; set; }

        public void Flush(object instance)
        {
            var datagram = new ServiceTcpResponseDatagram(EncodeObject(instance), Status, EncodeString(ContentType));
            datagram.Wrap();
            _Connection.SocketObject.BeginSend(datagram.Bytes, 0, datagram.Bytes.Length);
        }

        public byte[] EncodeString(string stringValue)
        {
            return Encoding.UTF8.GetBytes(stringValue ?? string.Empty);
        }

        public byte[] EncodeObject(object objectValue)
        {
            if (objectValue is string)
            {
                return EncodeString(objectValue as string);
            }
            else
            {
                return _Formatter.WriteBytes(objectValue);
            }
        }
    }
}
