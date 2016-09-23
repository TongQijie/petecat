using System;
using System.IO;
using System.Text;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonPlainValueObject : JsonObject
    {
        public object Value { get; set; }

        public bool EncompassedByQuote { get; set; }

        public JsonObject ExternalObject { get; set; }

        public byte[] Buffer { get; set; }

        public override string ToString()
        {
            if (Buffer != null)
            {
                return Encoding.UTF8.GetString(Buffer);
            }
            else
            {
                return string.Empty;
            }
        }

        public override bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            if (EncompassedByQuote)
            {
                int before = -1, after = -1;
                while ((after = stream.ReadByte()) != -1)
                {
                    if (after == JsonEncoder.Double_Quotes && before != JsonEncoder.Backslash)
                    {
                        var b = JsonUtility.Find(stream, x => JsonUtility.IsVisibleChar(x));
                        if (b == -1)
                        {
                            return true;
                        }

                        if (seperators != null && seperators.Exists(x => x == b))
                        {
                            return false;
                        }

                        if (terminators != null && terminators.Exists(x => x == b))
                        {
                            return true;
                        }

                        throw new Exception("");
                    }
                    else if (after == JsonEncoder.Double_Quotes && before == JsonEncoder.Backslash)
                    {
                        Buffer[Buffer.Length - 1] = JsonEncoder.Double_Quotes;
                    }
                    else if (after == JsonEncoder.Backslash && before == JsonEncoder.Backslash)
                    {
                        after = -1;
                    }
                    else
                    {
                        Buffer = Buffer.Append((byte)after);
                    }

                    before = after;
                }
            }
            else
            {
                int b;
                while ((b = stream.ReadByte()) != -1)
                {
                    if (!JsonUtility.IsVisibleChar(b))
                    {
                        continue;
                    }

                    if (seperators != null && seperators.Exists(x => x == b))
                    {
                        return false;
                    }
                    
                    if (terminators != null && terminators.Exists(x => x == b))
                    {
                        return true;
                    }

                    Buffer = Buffer.Append((byte)b);
                }
            }

            return true;
        }
    }
}
