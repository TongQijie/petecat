using System;
using System.IO;
using System.Text;

using Petecat.Extension;
using System.Globalization;

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
        public const byte Space = 0x20;
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

        /// <summary>
        /// convert memory object value to byte array with escape characters.
        /// </summary>
        /// <param name="elementValue">object value, it's type could be string, number, boolean, datetime.</param>
        /// <returns>byte array with escape characters</returns>
        public static byte[] GetPlainValue(object elementValue)
        {
            if (elementValue == null)
            {
                return GetNullValue();
            }

            var stringBuilder = new StringBuilder();
            foreach (var c in elementValue.ToString())
            {
                stringBuilder.Append(ConvertToEscapeCharacter(c));
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

        /// <summary>
        /// convert byte array with escape characters to byte array without escape characters
        /// </summary>
        /// <param name="byteValues">byte array with escape characters</param>
        /// <returns>byte array without escape characters</returns>
        public static string GetPlainValue(byte[] byteValues)
        {
            if (byteValues == null || byteValues.Length == 0)
            {
                return string.Empty;
            }

            var value = new byte[0];

            for (int i = 0; i < byteValues.Length;)
            {
                if (byteValues[i] == Backslash)
                {
                    if (i < byteValues.Length - 1)
                    {
                        if (Convert.ToChar(byteValues[i + 1]) == 'u')
                        {
                            if (i < byteValues.Length - 5)
                            {
                                value = value.Append(ConvertFromEscapeCharacter(byteValues.SubArray(i + 1, 5)));
                                i += 6;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                        else
                        {
                            value = value.Append(ConvertFromEscapeCharacter(byteValues.SubArray(i + 1, 1)));
                            i += 2;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    value = value.Append(byteValues[i]);
                    i++;
                }
            }

            return GetString(value);
        }

        public static byte[] GetNullValue()
        {
            return GetBytes(NullValueMark);
        }

        public static bool IsNullValue(object value)
        {
            return string.Equals(value.ToString(), NullValueMark, StringComparison.OrdinalIgnoreCase);
        }

        public static byte[] ConvertFromEscapeCharacter(byte[] source)
        {
            if (source == null || source.Length == 0)
            {
                return new byte[0];
            }

            switch (Convert.ToChar(source[0]))
            {
                case '"':
                    {
                        return new byte[] { Double_Quotes };
                    }
                case '\\':
                    {
                        return new byte[] { Backslash };
                    }
                case '/':
                    {
                        return new byte[] { Slash };
                    }
                case 'b':
                    {
                        return new byte[] { Backspace };
                    }
                case 'f':
                    {
                        return new byte[] { Formfeed };
                    }
                case 'n':
                    {
                        return new byte[] { Newline };
                    }
                case 'r':
                    {
                        return new byte[] { CarriageReturn };
                    }
                case 't':
                    {
                        return new byte[] { HorizontalTab };
                    }
                case 'u':
                    {
                        if (source.Length == 5)
                        {
                            var number = int.Parse(CurrentEncoding.GetString(source.SubArray(1, 4)), NumberStyles.HexNumber);
                            return CurrentEncoding.GetBytes(new char[] { Convert.ToChar(number) });
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return new byte[0];
        }

        public static string ConvertToEscapeCharacter(char source)
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
