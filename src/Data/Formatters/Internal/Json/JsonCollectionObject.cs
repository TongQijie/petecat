﻿using System;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonCollectionObject : JsonObject
    {
        public JsonCollectionElement[] Elements { get; set; }

        public override bool Fill(IBufferStream stream, byte[] seperators, byte[] terminators)
        {
            Elements = new JsonCollectionElement[0];

            while (!Parse(stream))
            {
                ;
            }

            var b = stream.SeekBytesUntilNotEqual(JsonEncoder.Whitespace);
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

            throw new Errors.JsonParseFailedException(stream.TotalIndex, "collection element has invalid terminal byte.");
        }

        private bool Parse(IBufferStream stream)
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
