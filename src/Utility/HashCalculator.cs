using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Petecat.Utility
{
    public static class HashCalculator
    {
        public enum Algorithm
        {
            Sha1,

            Md5,
        }

        private static int HashBlockSize = 4 * 1024 * 1024;

        public static byte[] Compute(Algorithm algorithmName, string filename)
        {
            using (var inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return Compute(algorithmName, inputStream);
            }
        }

        public static byte[] Compute(Algorithm algorithmName, Stream stream, long offset, long size)
        {
            HashAlgorithm algorithm = null;
            if (algorithmName == Algorithm.Sha1)
            {
                algorithm = SHA1.Create();
            }
            else if (algorithmName == Algorithm.Md5)
            {
                algorithm = MD5.Create();
            }
            else
            {
                throw new NotSupportedException();
            }

            var buffer = new byte[HashBlockSize];

            stream.Seek(offset, SeekOrigin.Begin);

            long count = 0;

            while (count < size)
            {
                if ((size - count) > HashBlockSize)
                {
                    stream.Read(buffer, 0, buffer.Length);
                    algorithm.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
                    count += HashBlockSize;
                }
                else
                {
                    stream.Read(buffer, 0, (int)(size - count));
                    algorithm.TransformFinalBlock(buffer, 0, (int)(size - count));
                    count = size;
                }
            }

            return algorithm.Hash;
        }

        public static byte[] Compute(Algorithm algorithmName, Stream stream)
        {
            HashAlgorithm algorithm = null;
            if (algorithmName == Algorithm.Sha1)
            {
                algorithm = SHA1.Create();
            }
            else if (algorithmName == Algorithm.Md5)
            {
                algorithm = MD5.Create();
            }
            else
            {
                throw new NotSupportedException();
            }

            return algorithm.ComputeHash(stream);
        }

        public static byte[] Compute(Algorithm algorithmName, byte[] buffer, int offset, int size)
        {
            HashAlgorithm algorithm = null;
            if (algorithmName == Algorithm.Sha1)
            {
                algorithm = SHA1.Create();
            }
            else if (algorithmName == Algorithm.Md5)
            {
                algorithm = MD5.Create();
            }
            else
            {
                throw new NotSupportedException();
            }

            return algorithm.ComputeHash(buffer, offset, size);
        }
    }
}
