using Petecat.Extension;

using System;

namespace Petecat.Service.Datagram
{
    public class ServiceTcpDatagram
    {
        public ServiceTcpDatagram() { }

        public ServiceTcpDatagram(byte[] bytes, int maxinum = 4 * 1024)
        {
            _Bytes = bytes;
            _Maxinum = maxinum;
        }

        protected byte[] _Bytes = null;

        public byte[] Bytes { get { return _Bytes ?? new byte[0]; } }

        protected int _Maxinum = 4 * 1024;

        protected int _ContentSize = 0;

        public void Append(byte[] bytes, int offset, int count)
        {
            if (_Bytes.Length + count > _Maxinum)
            {
                throw new Exception("");
            }
            else
            {
                _Bytes = _Bytes.Append(bytes, offset, count);
            }
        }

        public void Clear()
        {
            _Bytes = new byte[0];
        }

        public bool Validate()
        {
            if (_Bytes.Length < 2)
            {
                return false;
            }

            if (_Bytes[0] != 0xFF || _Bytes[1] != 0xFE)
            {
                _Bytes = new byte[0];
                return false;
            }

            if (_Bytes.Length < 8)
            {
                return false;
            }

            var size = _Bytes[2] + (_Bytes[3] << 8) + (_Bytes[4] << 16) + (_Bytes[5] << 24);
            if (size + 8 > _Maxinum)
            {
                _Bytes = new byte[0];
                return false;
            }

            if (_Bytes.Length < size + 8)
            {
                return false;
            }

            if (_Bytes[size + 6] != 0xEF || _Bytes[size + 7] != 0xFF)
            {
                _Bytes = new byte[0];
                return false;
            }

            return true;
        }

        public void Wrap()
        {
            var stackArray = new StackArray(_ContentSize + 8);
            stackArray.Push((byte)0xFF);
            stackArray.Push((byte)0xFE);
            stackArray.Push(_ContentSize);
            Wrap(stackArray);
            stackArray.Push((byte)0xEF);
            stackArray.Push((byte)0xFF);
            _Bytes = stackArray.Bytes;
        }

        protected virtual void Wrap(StackArray stackArray) { }

        public void Unwrap()
        {
            var stackArray = new StackArray(_Bytes);
            stackArray.Seek(6, StackArray.SeekOrigin.Start);
            Unwrap(stackArray);
        }

        protected virtual void Unwrap(StackArray stackArray) { }
    }
}
