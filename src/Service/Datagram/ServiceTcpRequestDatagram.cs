namespace Petecat.Service.Datagram
{
    public class ServiceTcpRequestDatagram : ServiceTcpDatagram
    {
        // Header: 0xFF, 0xFE
        // Length: 0x00, 0x01, 0x02, 0x3
        // BodyLength: 0x01, 0x02, 0x03, 0x04
        // BodyValue: 0x01, 0x02, 0x03
        // ServiceNameLength: 0x00
        // ServiceNameValue: 0x00, 0x01, 0x02, 0x03
        // MethodNameLength: 0x01
        // MethodNameValue: 0x01, 0x02, 0x03, 0x04
        // ContentTypeLength: 0x00
        // ContentTypeValue: 0x01, 0x02, 0x03, 0x04
        // Footer: 0xEF, 0xFF

        public ServiceTcpRequestDatagram(byte[] body, byte[] serviceName, byte[] methodName, byte[] contentType)
            : base()
        {
            Body = body ?? new byte[0];
            ServiceName = serviceName ?? new byte[0];
            MethodName = methodName ?? new byte[0];
            ContentType = contentType ?? new byte[0];

            _ContentSize += 4 + Body.Length;
            _ContentSize += 1 + ServiceName.Length;
            _ContentSize += 1 + MethodName.Length;
            _ContentSize += 1 + ContentType.Length;
        }

        public ServiceTcpRequestDatagram(byte[] bytes)
            : base(bytes, 128 * 1024)
        {
        }

        public byte[] Body { get; private set; }

        public byte[] ServiceName { get; private set; }

        public byte[] MethodName { get; private set; }

        public byte[] ContentType { get; private set; }

        protected override void Wrap(StackArray stackArray)
        {
            stackArray.Push(Body.Length);
            stackArray.Push(Body);
            stackArray.Push((byte)ServiceName.Length);
            stackArray.Push(ServiceName);
            stackArray.Push((byte)MethodName.Length);
            stackArray.Push(MethodName);
            stackArray.Push((byte)ContentType.Length);
            stackArray.Push(ContentType);
        }

        protected override void Unwrap(StackArray stackArray)
        {
            Body = stackArray.PopArray(stackArray.PopInt());
            ServiceName = stackArray.PopArray(stackArray.PopByte());
            MethodName = stackArray.PopArray(stackArray.PopByte());
            ContentType = stackArray.PopArray(stackArray.PopByte());
        }
    }
}
