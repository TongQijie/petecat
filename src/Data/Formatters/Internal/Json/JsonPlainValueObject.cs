using System;
using System.IO;
using System.Text;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonPlainValueObject : JsonObject
    {
        public bool EncompassedByQuote { get; set; }

        public JsonObject ExternalObject { get; set; }

        public byte[] Buffer { get; set; }

        public override string ToString()
        {
            if (Buffer != null)
            {
                return JsonEncoder.GetString(Buffer);
            }
            else
            {
                return string.Empty;
            }
        }

        public override bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            var terminated = true;

            if (EncompassedByQuote)
            {
                int before = -1, after = -1;
                JsonUtility.Feed(stream, (a) =>
                {
                    after = a;

                    if (after == JsonEncoder.Double_Quotes && before != JsonEncoder.Backslash)
                    {
                        var b = JsonUtility.Find(stream, x => JsonUtility.IsVisibleChar(x));
                        if (b == -1)
                        {
                            terminated = true;
                        }
                        else if (seperators != null && seperators.Exists(x => x == b))
                        {
                            terminated = false;
                        }
                        else if (terminators != null && terminators.Exists(x => x == b))
                        {
                            terminated = true;
                        }
                        else
                        {
                            throw new Exception("");
                        }

                        return false;
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
                    return true;
                });
            }
            else
            {
                JsonUtility.Feed(stream, (b) =>
                {
                    if (!JsonUtility.IsVisibleChar(b))
                    {
                        return true;
                    }
                    else if (seperators != null && seperators.Exists(x => x == b))
                    {
                        terminated = false;
                        return false;
                    }
                    else if (terminators != null && terminators.Exists(x => x == b))
                    {
                        terminated = true;
                        return false;
                    }
                    else
                    {
                        Buffer = Buffer.Append((byte)b);
                        return true;
                    }
                });
            }

            return terminated;
        }
    }
}
