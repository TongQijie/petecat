using System.IO;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    public class JsonCollectionObject : JsonObject
    {
        public JsonCollectionElement[] Elements { get; set; }

        public override bool Fill(Stream stream, byte[] seperators, byte[] terminators)
        {
            Elements = new JsonCollectionElement[0];

            if (Parse(stream))
            {
                return true;
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

                if (b == JsonEncoder.Comma)
                {
                    if (Parse(stream))
                    {
                        break;
                    }
                }
            }

            return true;
        }

        private bool Parse(Stream stream)
        {
            var args = new JsonObjectParseArgs()
            {
                ExternalObject = this,
                Stream = stream,
            };
            JsonObjectParser.Parse(args);

            if (args.InternalObject != null)
            {
                Elements = Elements.Append(new JsonCollectionElement() { Value = args.InternalObject });
            }

            return args.Handled;
        }
    }
}
