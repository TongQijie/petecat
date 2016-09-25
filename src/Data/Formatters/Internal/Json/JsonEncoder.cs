using System;
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
        /// '/'
        /// </summary>
        public const byte Slash = 0x2F;
        /// <summary>
        /// ' '
        /// </summary>
        public const byte Whitespace = 0x20;
        /// <summary>
        /// '\b'
        /// </summary>
        public const byte Backspace = 0x08;
        /// <summary>
        /// '\f'
        /// </summary>
        public const byte Formfeed = 0x0C;
        /// <summary>
        /// '\n'
        /// </summary>
        public const byte Newline = 0x0A;
        /// <summary>
        /// '\r'
        /// </summary>
        public const byte CarriageReturn = 0x0D;
        /// <summary>
        /// '\t'
        /// </summary>
        public const byte HorizontalTab = 0x09;
        /// <summary>
        /// 'b'
        /// </summary>
        public const byte B = 0x62;
        /// <summary>
        /// 'f'
        /// </summary>
        public const byte F = 0x66;
        /// <summary>
        /// 'n'
        /// </summary>
        public const byte N = 0x6E;
        /// <summary>
        /// 'r'
        /// </summary>
        public const byte R = 0x72;
        /// <summary>
        /// 't'
        /// </summary>
        public const byte T = 0x74;
        /// <summary>
        /// 'u'
        /// </summary>
        public const byte U = 0x75;
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
            if (elementValue == null)
            {
                return GetNullValue();
            }

            var stringBuilder = new StringBuilder();
            foreach (var c in elementValue.ToString())
            {
                stringBuilder.Append(Doescape(c));
            }

            var byteValues = GetBytes(stringBuilder.ToString());

            if (elementValue is string || elementValue is DateTime)
            {
                return new byte[0].Append(Double_Quotes).Append(byteValues).Append(Double_Quotes);
            }
            else
            {
                return byteValues;
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

        public static byte DoUnescape(int source)
        {
            switch (source)
            {
                case Double_Quotes:
                    {
                        return Double_Quotes ;
                    }
                case Backslash:
                    {
                        return Backslash;
                    }
                case Slash:
                    {
                        return Slash;
                    }
                case B:
                    {
                        return Backspace;
                    }
                case F:
                    {
                        return Formfeed;
                    }
                case N:
                    {
                        return Newline;
                    }
                case R:
                    {
                        return CarriageReturn;
                    }
                case T:
                    {
                        return HorizontalTab;
                    }
                default:
                    {
                        return 0;
                    }
            }
        }

        public static byte[] DoUnescape(byte[] source)
        {
            if (source == null || source.Length != 4)
            {
                return new byte[0];
            }

            var firstByte = ConvertHexByte(source[0]) * 0x0F + ConvertHexByte(source[1]);
            var secondByte = ConvertHexByte(source[2]) * 0x0F + ConvertHexByte(source[3]);

            if (firstByte == 0)
            {
                return new byte[] { (byte)secondByte };
            }
            else
            {
                return new byte[] { (byte)firstByte, (byte)secondByte };
            }
        }

        private static int ConvertHexByte(byte byteValue)
        {
            if (byteValue >= 0x30 && byteValue <= 0x39)
            {
                return byteValue - 0x30;
            }
            else if (byteValue >= 0x41 && byteValue <= 0x46)
            {
                return (byteValue - 0x41) + 0x0A;
            }
            else if (byteValue >= 0x61 && byteValue <= 0x66)
            {
                return (byteValue - 0x61) + 0x0A;
            }
            else
            {
                return 0;
            }
        }

        public static string Doescape(char source)
        {
            switch (source)
            {
                case '"':
                    {
                        return "\\\"";
                    }
                case '\\':
                    {
                        return "\\\\";
                    }
                case '/':
                    {
                        return "\\/";
                    }
                case '\b':
                    {
                        return "\\b";
                    }
                case '\f':
                    {
                        return "\\f";
                    }
                case '\n':
                    {
                        return "\\n";
                    }
                case '\r':
                    {
                        return "\\r";
                    }
                case '\t':
                    {
                        return "\\t";
                    }
                default:
                    {
                        return Convert.ToString(source);
                    }
            }
        }
    }
}
