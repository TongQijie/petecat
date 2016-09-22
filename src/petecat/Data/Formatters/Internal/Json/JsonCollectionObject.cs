using System;
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

            while (!Parse(stream))
            {
                ;
            }

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
