using System;
using System.Text;

namespace Petecat.Network.Shared
{
    public class ByteArray
    {
        private const int STEP = 100;

        private byte[] m_p = new byte[100];

        private int m_nDataSize;

        private void Extend(int size)
        {
            byte[] array = new byte[this.m_p.Length + size];
            Buffer.BlockCopy(this.m_p, 0, array, 0, this.m_p.Length);
            this.m_p = array;
        }

        public void Add(byte[] srcArr)
        {
            if (this.m_nDataSize + srcArr.Length > this.m_p.Length)
            {
                int num = this.m_nDataSize + srcArr.Length - this.m_p.Length;
                this.Extend((num < 100) ? 100 : num);
            }
            Buffer.BlockCopy(srcArr, 0, this.m_p, this.m_nDataSize, srcArr.Length);
            this.m_nDataSize += srcArr.Length;
        }

        public void Add(byte p)
        {
            if (this.m_nDataSize >= this.m_p.Length)
            {
                this.Extend(100);
            }
            this.m_p[this.m_nDataSize++] = p;
        }

        public void Add(short p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(int p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(ushort p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(uint p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(float p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(double p)
        {
            this.Add(BitConverter.GetBytes(p));
        }

        public void Add(string p)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(p);
            this.Add((ushort)bytes.Length);
            this.Add(bytes);
        }

        public byte[] Pack()
        {
            byte[] array = new byte[this.m_nDataSize];
            Buffer.BlockCopy(this.m_p, 0, array, 0, this.m_nDataSize);
            return array;
        }
    }
}
