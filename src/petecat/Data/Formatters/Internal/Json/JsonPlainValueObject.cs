using System.IO;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    public class JsonPlainValueObject : JsonObject
    {
        public object Value { get; set; }

        public bool EncompassedByQuote { get; set; }

        public JsonObject ExternalObject { get; set; }

        public byte[] Buffer { get; set; }

        public override bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            if (EncompassedByQuote)
            {
                int before = -1, after = -1;
                while ((after = stream.ReadByte()) != -1)
                {
                    if (after == JsonEncoder.Double_Quotes && before != JsonEncoder.Backslash)
                    {
                        break;
                    }
                    else if (after == JsonEncoder.Double_Quotes && before == JsonEncoder.Backslash)
                    {
                        Buffer[Buffer.Length - 1] = JsonEncoder.Double_Quotes;
                    }
                    else
                    {
                        Buffer = Buffer.Append((byte)after);
                    }

                    before = after;
                }

                int b;
                while ((b = stream.ReadByte()) != -1)
                {
                    if (seperators != null && seperators.Exists(x => x == b))
                    {
                        return false;
                    }
                    
                    if (terminators != null && terminators.Exists(x => x == b))
                    {
                        return true;
                    }
                }
            }
            else
            {
                int b;
                while ((b = stream.ReadByte()) != -1)
                {
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
