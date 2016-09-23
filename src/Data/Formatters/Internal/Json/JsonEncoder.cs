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
        /// <summary>
        /// "null"
        /// </summary>
        public const string NullValueMark = "null";
        /// <summary>
        /// current encoding: UTF8 is default.
        /// </summary>
        public static Encoding CurrentEncoding = Encoding.UTF8;

        public static string GetString(byte[] byteValues)
        {
            return CurrentEncoding.GetString(byteValues);
        }

        public static byte[] GetBytes(string stringValue)
        {
            return CurrentEncoding.GetBytes(stringValue);
        }

        public static byte[] GetElementName(string elementName)
        {
            var name = GetBytes(elementName);

            var dest = new byte[name.Length + 3];
            dest[0] = Double_Quotes;
            Array.Copy(name, 0, dest, 1, name.Length);
            dest[dest.Length - 2] = Double_Quotes;
            dest[dest.Length - 1] = Colon;
            return dest;
        }

        public static byte[] GetPlainValue(object elementValue)
        {
            if (elementValue is string || elementValue is DateTime)
            {
                // " -> \"
                // \ -> \\
                var value = GetBytes(elementValue.ToString().Replace("\\", "\\\\").Replace("\"", "\\\""));

                var dest = new byte[value.Length + 2];
                dest[0] = Double_Quotes;
                Array.Copy(value, 0, dest, 1, value.Length);
                dest[dest.Length - 1] = Double_Quotes;
                return dest;
            }
            else
            {
                return GetBytes(elementValue.ToString().Replace("\\", "\\\\").Replace("\"", "\\\""));
            }
        }

        public static byte[] GetNullValue()
        {
            return GetBytes(NullValueMark);
        }

        public static bool IsNullValue(object value)
        {
            return string.Equals(value.ToString(), NullValueMark, StringComparison.OrdinalIgnoreCase);
        }
    }
}
