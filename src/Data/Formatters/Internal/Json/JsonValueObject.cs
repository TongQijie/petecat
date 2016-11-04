﻿using System;

using Petecat.IO;
using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonValueObject : JsonObject
    {
        public bool EncompassedByQuote { get; set; }

        public JsonObject ExternalObject { get; set; }

        public byte[] Buffer { get; set; }

        public override string ToString()
        {
            if (Buffer != null)
            {
                return JsonEncoder.GetString(Buffer);
            }
            else
            {
                return string.Empty;
            }
        }

        public override bool Fill(IStream stream, byte[] seperators, byte[] terminators)
        {
            var terminated = true;
            if (Buffer == null)
            {
                Buffer = new byte[0];
            }

            if (EncompassedByQuote)
            {
                Buffer = GetUnescapeByteValues(stream);

                if (Buffer == null)
                {
                    throw new Errors.JsonParseFailedException(stream.Position, "plain value cannot be empty.");
                }

                //var b = stream.SeekBytesUntilNotEqual(JsonEncoder.Whitespace);
                var b = stream.SeekBytesUntilVisiableChar();
                if (b == -1)
                {
                    terminated = true;
                }
                else if (seperators != null && seperators.Exists(x => x == b))
                {
                    terminated = false;
                }
                else if (terminators != null && terminators.Exists(x => x == b))
                {
                    terminated = true;
                }
                else
                {
                    throw new Errors.JsonParseFailedException(stream.Position, "plain value object has invalid terminal byte.");
                }
            }
            else
            {
                byte[] ends = new byte[0];
                if (seperators != null && seperators.Length > 0)
	            {
                    ends = ends.Append(seperators);
	            }
                if (terminators != null && terminators.Length > 0)
                {
                    ends = ends.Append(terminators);
                }

                var buf = stream.ReadBytesUntil(ends.Append(JsonEncoder.Whitespace));
                if (buf == null || buf.Length == 0)
                {
                    throw new Errors.JsonParseFailedException(stream.Position, "plain value cannot be empty.");
                }

                if (Buffer != null && Buffer.Length > 0)
                {
                    Buffer = Buffer.Append(buf.SubArray(0, buf.Length - 1));
                }
                else
                {
                    Buffer = buf.SubArray(0, buf.Length - 1);
                }

                var terminator = buf[buf.Length - 1];
                if (terminator == JsonEncoder.Whitespace)
                {
                    terminator = (byte)stream.SeekBytesUntilNotEqual(JsonEncoder.Whitespace);
                }
                if (seperators != null && seperators.Exists(x => x == terminator))
                {
                    terminated = false;
                }
                else if (terminators != null && terminators.Exists(x => x == terminator))
                {
                    terminated = true;
                }
                else
                {
                    throw new Errors.JsonParseFailedException(stream.Position, "plain value object has invalid terminal byte.");
                }
            }

            return terminated;
        }

        private byte[] GetUnescapeByteValues(IStream stream)
        {
            var buffer = new byte[0];

            var buf = new byte[0];
            while ((buf = stream.ReadBytesUntil(new byte[] { JsonEncoder.Double_Quotes, JsonEncoder.Backslash })) != null)
            {
                if (buf[buf.Length - 1] == JsonEncoder.Double_Quotes)
                {
                    return Concat(buffer, 0, buffer.Length, buf, 0, buf.Length - 1);
                }

                if (buf[buf.Length - 1] == JsonEncoder.Backslash)
                {
                    var b = stream.ReadByte();
                    if (b == -1)
                    {
                        return null;
                    }

                    if (b == JsonEncoder.U)
                    {
                        var escape = JsonEncoder.DoUnescape(stream.ReadBytes(4));
                        buf = Concat(buf, 0, buf.Length - 1, escape, 0, escape.Length);
                    }
                    else
                    {
                        buf[buf.Length - 1] = JsonEncoder.DoUnescape(b);
                    }

                    buffer = Concat(buffer, 0, buffer.Length, buf, 0, buf.Length);
                }
            }

            return buffer;
        }

        private byte[] Concat(byte[] firstArray, int firstStart, int firstCount, byte[] secondArray, int secondStart, int secondCount)
        {
            var buf = new byte[firstCount + secondCount];

            Array.Copy(firstArray, firstStart, buf, 0, firstCount);

            Array.Copy(secondArray, secondStart, buf, firstCount, secondCount);

            return buf;
        }
    }
}
