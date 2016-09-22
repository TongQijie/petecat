using System.IO;

namespace Petecat.Data.Formatters.Internal.Json
{
    public static class JsonObjectParser
    {
        public static void Parse(JsonObjectParseArgs args)
        {
            int b;
            while ((b = args.Stream.ReadByte()) != -1)
            {
                if (b == JsonEncoder.Left_Brace)
                {
                    // object
                    args.InternalObject = new JsonDictionaryObject();
                    args.Handled = args.InternalObject.Fill(args.Stream, new byte[] { JsonEncoder.Right_Brace }, null);
                }
                else if (b == JsonEncoder.Left_Bracket)
                {
                    // collection
                    args.InternalObject = new JsonCollectionObject();
                    args.Handled = args.InternalObject.Fill(args.Stream, new byte[] { JsonEncoder.Right_Bracket }, null);
                }
                else if (IsNonEmptyChar(b))
                {
                    // value
                    args.InternalObject = new JsonPlainValueObject()
                    {
                        EncompassedByQuote = b == JsonEncoder.Double_Quotes,
                        Buffer = b == JsonEncoder.Double_Quotes ? new byte[0] : new byte[1] { (byte)b },
                    };

                    byte[] terminators = null;
                    if (args.ExternalObject != null && args.ExternalObject is JsonDictionaryObject)
                    {
                        terminators = new byte[] { JsonEncoder.Right_Brace };
                    }
                    else if (args.ExternalObject != null && args.ExternalObject is JsonCollectionObject)
                    {
                        terminators = new byte[] { JsonEncoder.Right_Bracket };
                    }

                    args.Handled = args.InternalObject.Fill(args.Stream, new byte[] { JsonEncoder.Comma }, terminators);
                }

                if (args.InternalObject != null)
                {
                    break;
                }
            }
        }

        private static bool IsNonEmptyChar(int b)
        {
            return b > 0x20 && b <= 0x7E;
        }
    }
}
