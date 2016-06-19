using System.Text;

namespace Petecat.Utility
{
    public static class ValueConvertUtility
    {
        public static string HexStringFrom(byte[] bytes)
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
