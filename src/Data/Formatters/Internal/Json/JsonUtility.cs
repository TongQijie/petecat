using System;
using System.IO;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal static class JsonUtility
    {
        public static bool Seek(IBufferStream stream, byte target)
        {
            return stream.Go(target);
        }

        public static int Find(IBufferStream stream, Predicate<int> predicate)
        {
            return stream.FirstOrDefault(predicate);
        }

        public static void Feed(IBufferStream stream, Func<int, bool> action)
        {
            int b;
            while ((b = stream.ReadByte()) != -1 && action(b)) { }
        }

        public static byte[] GetBytes(IBufferStream stream, byte terminator)
        {
            return stream.GetBytes(new byte[0], terminator);
        }

        public static byte[] GetBytes(IBufferStream stream, byte[] terminators)
        {
            return stream.GetBytes(new byte[0], terminators);
        }

        public static byte[] GetStringValue(IBufferStream stream)
        {
            byte[] buf = new byte[0];
            while ((buf = stream.GetBytes(buf, new byte[] { JsonEncoder.Double_Quotes, JsonEncoder.Backslash })) != null)
            {
                if (buf[buf.Length - 1] == JsonEncoder.Double_Quotes)
                {
                    return buf.SubArray(0, buf.Length - 1);
                }
                
                if (buf[buf.Length - 1] == JsonEncoder.Backslash)
                {
                    var b = stream.ReadByte();
                    if (b == -1)
                    {
                        return null;
                    }

                    buf = buf.Append((byte)b);
                }
            }

            return buf;
        }
    }
}
