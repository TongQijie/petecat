using System.Text;

namespace Petecat.Extension
{
    public static class ByteArrayExtension
    {
        public static string ToHexString(this byte[] bytes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        public static bool EqualsWith(this byte[] thisBytes, byte[] anotherBytes)
        {
            if (thisBytes == null || anotherBytes == null || thisBytes.Length != anotherBytes.Length)
            {
                return false;
            }

            for (int i = 0; i < thisBytes.Length; i++)
            {
                if (thisBytes[i] != anotherBytes[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
