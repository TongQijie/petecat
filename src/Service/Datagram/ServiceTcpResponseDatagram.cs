using System;
using System.Text;
namespace Petecat.Service.Datagram
{
    public class ServiceTcpResponseDatagram
    {
        // Header: 0xFF, 0xFE
        // Length: 0x00, 0x01, 0x02, 0x3
        // BodyLength: 0x01, 0x02, 0x03, 0x04
        // BodyValue: 0x01, 0x02, 0x03
        // Status: 0x00
        // ContentTypeLength: 0x00
        // ContentTypeValue: 0x01, 0x02, 0x03, 0x04
        // Footer: 0xEF, 0xFF

        public ServiceTcpResponseDatagram(object body, byte status, string contentType)
        {
            Body = body;
            Status = status;
            ContentType = contentType;
        }

        public ServiceTcpResponseDatagram(byte[] bytes)
        {
            _Bytes = bytes;
        }

        private byte[] _Bytes = null;

        public object Body { get; private set; }

        public byte Status { get; private set; }

        public string ContentType { get; private set; }

        public byte[] Wrap()
        {
            byte[] body = new byte[0];
            if (Body != null)
            {
                var objectFormatter = ServiceHttpFormatter.GetFormatter(ContentType);
                if (objectFormatter != null)
                {
                    body = objectFormatter.WriteBytes(Body);
                }
                else
                {
                    body = Encoding.UTF8.GetBytes(Body.ToString());
                }
            }

            var contentType = Encoding.UTF8.GetBytes(ContentType ?? string.Empty);

            var stackArray = new StackArray(2 + 4 + 4 + body.Length + 1 + 1 + contentType.Length + 2);
            stackArray.Push(0xFF);
            stackArray.Push(0xFE);
            stackArray.Push(stackArray.Bytes.Length - 8);
            stackArray.Push(body.Length);
            stackArray.Push(body);
            stackArray.Push(Status);
            stackArray.Push((byte)contentType.Length);
            stackArray.Push(contentType);
            stackArray.Push(0xEF);
            stackArray.Push(0xFF);

            return stackArray.Bytes;
        }

        public void Unwrap()
        {
            var stackArray = new StackArray(_Bytes);

            stackArray.Seek(6, StackArray.SeekOrigin.Current);

            stackArray.Seek(stackArray.PopInt(), StackArray.SeekOrigin.Current);

            Status = stackArray.PopByte();
            ContentType = Encoding.UTF8.GetString(stackArray.PopArray(stackArray.PopByte()));
        }

        public object ReadBody(Type bodyType)
        {
             var stackArray = new StackArray(_Bytes);

            stackArray.Seek(6, StackArray.SeekOrigin.Current);

            if (bodyType != null)
            {
                var objectFormatter = ServiceHttpFormatter.GetFormatter(ContentType);
                if (objectFormatter != null)
                {
                    Body = objectFormatter.ReadObject(bodyType, _Bytes, 10, stackArray.PopInt());
                }
            }

            return Encoding.UTF8.GetString(_Bytes, 10, stackArray.PopInt());
        }
    }
}
