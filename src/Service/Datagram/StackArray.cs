using System;

namespace Petecat.Service.Datagram
{
    public class StackArray
    {
        public StackArray(int size)
        {
            Bytes = new byte[size];
        }

        public StackArray(byte[] bytes)
        {
            Bytes = bytes;
        }

        public byte[] Bytes { get; private set; }

        private int _Top = 0;

        public void Push(byte b)
        {
            Bytes[_Top++] = b;
        }

        public void Push(int i)
        {
            Push((byte)i);
            Push((byte)(i >> 8));
            Push((byte)(i >> 16));
            Push((byte)(i >> 24));
        }

        public void Push(byte[] a)
        {
            Array.Copy(a, 0, Bytes, _Top, a.Length);
            _Top += a.Length;
        }

        public byte PopByte()
        {
            return Bytes[_Top++];
        }

        public int PopInt()
        {
            var i = Bytes[_Top] + (Bytes[_Top + 1] << 8) + (Bytes[_Top + 2] << 16) + (Bytes[_Top + 3] << 24);
            _Top += 4;
            return i;
        }

        public byte[] PopArray(int count)
        {
            var a = new byte[count];
            Array.Copy(Bytes, _Top, a, 0, a.Length);
            _Top += count;
            return a;
        }

        public void Seek(int count, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Current: _Top += count; return;
                case SeekOrigin.Start: _Top = count; return;
                case SeekOrigin.End: _Top = Bytes.Length + count; return;
            }
        }

        public enum SeekOrigin
        {
            Start,

            Current,

            End,
        }
    }
}
