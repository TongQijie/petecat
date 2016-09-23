using System;
using System.IO;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal static class JsonUtility
    {
        public static bool IsVisibleChar(int b)
        {
            return b > 0x20 && b <= 0x7E;
        }

        public static bool Seek(Stream stream, byte target)
        {
            int b;
            while ((b = stream.ReadByte()) != -1 && b != target)
            {
            }

            if (b == -1)
            {
                return false;
            }

            return true;
        }

        public static int Find(Stream stream, Predicate<int> predicate)
        {
            int b;
            while ((b = stream.ReadByte()) != -1 && !predicate(b))
            {
            }

            if (b == -1)
            {
                return -1;
            }

            return b;
        }
    }
}
