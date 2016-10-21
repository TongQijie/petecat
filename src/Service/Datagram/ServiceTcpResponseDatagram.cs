namespace Petecat.Service.Datagram
{
    public class ServiceTcpResponseDatagram : ServiceTcpDatagram
    {
        // Header: 0xFF, 0xFE
        // Length: 0x00, 0x01, 0x02, 0x3
        // BodyLength: 0x01, 0x02, 0x03, 0x04
        // BodyValue: 0x01, 0x02, 0x03
        // Status: 0x00
        // ContentTypeLength: 0x00
        // ContentTypeValue: 0x01, 0x02, 0x03, 0x04
        // Footer: 0xEF, 0xFF

        public ServiceTcpResponseDatagram(byte[] body, byte status, byte[] contentType)
        {
            Body = body;
            Status = status;
            ContentType = contentType;

            _ContentSize += 4 + Body.Length;
            _ContentSize += 1;
            _ContentSize += 1 + ContentType.Length;
        }

        public ServiceTcpResponseDatagram(byte[] bytes)
            : base(bytes, 128 * 1024)
        {
        }

        public byte[] Body { get; private set; }

        public byte Status { get; private set; }

        public byte[] ContentType { get; private set; }

        protected override void Wrap(StackArray stackArray)
        {
            stackArray.Push(Body.Length);
            stackArray.Push(Body);
            stackArray.Push(Status);
            stackArray.Push((byte)ContentType.Length);
            stackArray.Push(ContentType);
        }

        protected override void Unwrap(StackArray stackArray)
        {
            Body = stackArray.PopArray(stackArray.PopInt());
            Status = stackArray.PopByte();
            ContentType = stackArray.PopArray(stackArray.PopByte());
        }
    }
}
