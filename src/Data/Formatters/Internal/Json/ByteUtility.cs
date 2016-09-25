using System;

namespace Petecat.Data.Formatters.Internal.Json
{
    public static class ByteUtility
    {
        public static byte[] Concat(byte[] firstArray, int firstStart, int firstCount, byte[] secondArray, int secondStart, int secondCount)
        {
            var buf = new byte[firstCount + secondCount];

            Array.Copy(firstArray, firstStart, buf, 0, firstCount);

            Array.Copy(secondArray, secondStart, buf, firstCount, secondCount);

            return buf;
        }
    }
}
