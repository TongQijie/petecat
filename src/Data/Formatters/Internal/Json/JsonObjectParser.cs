namespace Petecat.Data.Formatters.Internal.Json
{
    internal static class JsonObjectParser
    {
        public static void Parse(JsonObjectParseArgs args, bool header = false)
        {
            int b = args.Stream.SeekBytesUntilVisiableChar();
            //if (header)
            //{
            //    b = args.Stream.SeekBytesUntilMeets(x => x > 0x20 && x <= 0x7E);
            //}
            //else
            //{
            //    b = args.Stream.SeekBytesUntilNotEqual(JsonEncoder.Whitespace);
            //}
            if (b == -1)
            {
                return;
            }

            byte[] seperators = null, terminators = null;
            if (args.ExternalObject != null)
            {
                if (args.ExternalObject is JsonDictionaryObject)
                {
                    seperators = new byte[] { JsonEncoder.Comma };
                    terminators = new byte[] { JsonEncoder.Right_Brace };
                }
                else if (args.ExternalObject is JsonCollectionObject)
                {
                    seperators = new byte[] { JsonEncoder.Comma };
                    terminators = new byte[] { JsonEncoder.Right_Bracket };
                }
            }

            if (b == JsonEncoder.Left_Brace)
            {
                // dictionary
                // case1: {"name01":{"name01":value01,"name02":value02},"name01":{"name01":value01,"name02":value02}}
                //                  ------------------------------------^
                //                                                      Position
                // case2: [{"name01":value01,"name02":value02},{"name01":value01,"name02":value02}]
                //         ------------------------------------^
                //                                             Position
                args.InternalObject = new JsonDictionaryObject();
            }
            else if (b == JsonEncoder.Left_Bracket)
            {
                // collection
                // case1: {"name01":[value01,value02],"name02":[value03,value04]}
                //                  ------------------^
                //                                    Position
                // case2: [[value01,value02],[value03,value04]]
                //         ------------------^
                //                           Position
                args.InternalObject = new JsonCollectionObject();
            }
            else if (b == JsonEncoder.Right_Bracket)
            {
                args.Handled = true;
                return;
            }
            else
            {
                // plainvalue
                // case1: {"name01":value01,"name02":value02}
                //                                   --------^
                //                                           Position
                // case2: [value01, value02]
                //                  --------^
                //                          Position
                // case3: {"name01":value01,"name02":"value02"}
                //                                   ----------^
                //                                             Position
                // case4: [value01, value02]
                //                  --------^
                //                          Position
                args.InternalObject = new JsonValueObject()
                {
                    EncompassedByQuote = b == JsonEncoder.Double_Quotes,
                    Buffer = b == JsonEncoder.Double_Quotes ? new byte[0] : new byte[1] { (byte)b },
                };
            }

            args.Handled = args.InternalObject.Fill(args.Stream, seperators, terminators);
        }
    }
}
