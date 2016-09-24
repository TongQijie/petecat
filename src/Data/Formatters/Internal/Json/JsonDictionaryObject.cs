using System;
using System.IO;

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

            //var b = JsonUtility.Find(stream, x => JsonUtility.IsVisibleChar(x));
            var b = stream.Except(JsonEncoder.Space);
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

        private bool Parse(IBufferStream stream)
        {
            Json.JsonUtility.Seek(stream, JsonEncoder.Double_Quotes);

            var buf = JsonUtility.GetBytes(stream, JsonEncoder.Double_Quotes);
            if (buf == null)
            {
                buf = new byte[0];
            }

            //var buf = new byte[0];
            //JsonUtility.Feed(stream, (b) =>
            //{
            //    if (b != JsonEncoder.Double_Quotes)
            //    {
            //        buf = buf.Append((byte)b);
            //        return true;
            //    }

            //    return false;
            //});

            var elementName = JsonEncoder.GetString(buf);
            if (!elementName.HasValue())
            {
                throw new Exception("");
            }

            JsonUtility.Seek(stream, JsonEncoder.Colon);

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
