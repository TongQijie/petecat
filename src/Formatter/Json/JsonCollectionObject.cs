﻿using Petecat.Extending;
using Petecat.Formatter.Internal;

namespace Petecat.Formatter.Json
{
    public class JsonCollectionObject : JsonObject
    {
        public JsonCollectionElement[] Elements { get; set; }

        public override bool Fill(IStream stream, byte[] seperators, byte[] terminators)
        {
            Elements = new JsonCollectionElement[0];

            while (!Parse(stream))
            {
                ;
            }

            var b = stream.SeekBytesUntilVisiableChar();
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

            throw new Errors.JsonParseFailedException(stream.Position, "collection element has invalid terminal byte.");
        }

        private bool Parse(IStream stream)
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