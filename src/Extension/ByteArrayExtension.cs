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
    }
}
