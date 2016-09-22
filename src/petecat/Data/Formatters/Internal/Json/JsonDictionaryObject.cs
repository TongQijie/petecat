using System.IO;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    public class JsonDictionaryObject : JsonObject
    {
        public JsonDictionaryElement[] Elements { get; set; }

        public override bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            Elements = new JsonDictionaryElement[0];

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

                if (b == JsonEncoder.Double_Quotes)
                {
                    // start to read element's name
                    var elementName = JsonEncoder.GetElementName(stream);

                    // start to read element's value
                    Seek(stream, JsonEncoder.Colon);

                    var args = new JsonObjectParseArgs()
                    {
                        ExternalObject = this,
                        Stream = stream,
                    };
                    JsonObjectParser.Parse(args);

                    if (args.InternalObject != null)
                    {
                        Elements = Elements.Append(new JsonDictionaryElement() 
                        { 
                            Key = elementName,
                            Value = args.InternalObject,
                        });
                    }

                    if (args.Handled)
                    {
                        break;
                    }
                }
            }

            return true;
        }

        private bool Seek(Stream stream, byte target)
        {
            int b;
            while ((b = stream.ReadByte()) != -1 && b != target)
            {
            }

            if (b == -1)
            {
                return false;
            }

            return true;
        }
    }
}
