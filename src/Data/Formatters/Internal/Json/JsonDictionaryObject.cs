﻿using System;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonDictionaryObject : JsonObject
    {
        public JsonDictionaryElement[] Elements { get; set; }

        public override bool Fill(IBufferStream stream, byte[] seperators, byte[] terminators)
        {
            Elements = new JsonDictionaryElement[0];

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

            throw new Errors.JsonParseFailedException(stream.TotalIndex, "dictionary element has invalid terminal byte.");
        }

        private bool Parse(IBufferStream stream)
        {
            stream.SeekBytesUntilEqual(JsonEncoder.Double_Quotes);

            var buf = stream.ReadBytesUntil(JsonEncoder.Double_Quotes);
            if (buf == null)
            {
                buf = new byte[0];
            }

            var elementName = JsonEncoder.GetString(buf);
            if (!elementName.HasValue())
            {
                throw new Errors.JsonParseFailedException(stream.TotalIndex, "dictionary element name is empty.");
            }

            stream.SeekBytesUntilEqual(JsonEncoder.Colon);

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

            return args.Handled;
        }
    }
}
