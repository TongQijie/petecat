using System;
using System.IO;
using System.Text;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal static class JsonEncoder
    {
        /// <summary>
        /// '{'
        /// </summary>
        public const byte Left_Brace = 0x7B;
        /// <summary>
        /// '}'
        /// </summary>
        public const byte Right_Brace = 0x7D;
        /// <summary>
        /// '['
        /// </summary>
        public const byte Left_Bracket = 0x5B;
        /// <summary>
        /// ']'
        /// </summary>
        public const byte Right_Bracket = 0x5D;
        /// <summary>
        /// ','
        /// </summary>
        public const byte Comma = 0x2C;
        /// <summary>
        /// ':'
        /// </summary>
        public const byte Colon = 0x3A;
        /// <summary>
        /// '"'
        /// </summary>
        public const byte Double_Quotes = 0x22;
        /// <summary>
        /// '\'
        /// </summary>
        public const byte Backslash = 0x5C;

        public static byte[] GetElementName(string elementName)
        {
            var name = Encoding.UTF8.GetBytes(elementName);
            var dest = new byte[name.Length + 3];
            dest[0] = Double_Quotes;
            Array.Copy(name, 0, dest, 1, name.Length);
            dest[dest.Length - 2] = Double_Quotes;
            dest[dest.Length - 1] = Colon;
            return dest;
        }

        public static string GetElementName(Stream stream)
        {
            Json.JsonUtility.Seek(stream, Double_Quotes);

            var name = new byte[0];
            int b;
            while ((b = stream.ReadByte()) != -1 && b != Double_Quotes)
            {
                name = name.Append((byte)b);
            }

            if (b == -1)
            {
                return null;
            }

            return Encoding.UTF8.GetString(name);
        }

        public static byte[] GetSimpleValue(object elementValue)
        {
            if (elementValue is string || elementValue is DateTime)
            {
                // " -> \"
                // \ -> \\
                var value = Encoding.UTF8.GetBytes(elementValue.ToString().Replace("\\", "\\\\").Replace("\"", "\\\""));
                var dest = new byte[value.Length + 2];
                dest[0] = Double_Quotes;
                Array.Copy(value, 0, dest, 1, value.Length);
                dest[dest.Length - 1] = Double_Quotes;
                return dest;
            }
            else
            {
                return Encoding.UTF8.GetBytes(elementValue.ToString().Replace("\"", "\\\""));
            }
        }

        public static byte[] GetNullValue()
        {
            return Encoding.UTF8.GetBytes("null");
        }
    }
}
