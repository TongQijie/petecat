using System;
using System.IO;
using System.Text;

using Petecat.Extension;

namespace Petecat.Data.Formatters.Internal.Json
{
    internal class JsonPlainValueObject : JsonObject
    {
        public bool EncompassedByQuote { get; set; }

        public JsonObject ExternalObject { get; set; }

        public byte[] Buffer { get; set; }

        public override string ToString()
        {
            if (Buffer != null)
            {
                return JsonEncoder.GetPlainValue(Buffer);
            }
            else
            {
                return string.Empty;
            }
        }

        public override bool Fill(IBufferStream stream, byte[] seperators, byte[] terminators)
        {
            var terminated = true;
            if (Buffer == null)
            {
                Buffer = new byte[0];
            }

            if (EncompassedByQuote)
            {
                Buffer = JsonUtility.GetStringValue(stream);
                if (Buffer == null)
                {
                    throw new Exception("");
                }

                var b = stream.Except(JsonEncoder.Space);
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
                    throw new Exception("");
                }

                //int before = -1, after = -1;
                //JsonUtility.Feed(stream, (a) =>
                //{
                //    after = a;

                //    if (after == JsonEncoder.Double_Quotes && before != JsonEncoder.Backslash)
                //    {
                //        var b = JsonUtility.Find(stream, x => JsonUtility.IsVisibleChar(x));
                //        if (b == -1)
                //        {
                //            terminated = true;
                //        }
                //        else if (seperators != null && seperators.Exists(x => x == b))
                //        {
                //            terminated = false;
                //        }
                //        else if (terminators != null && terminators.Exists(x => x == b))
                //        {
                //            terminated = true;
                //        }
                //        else
                //        {
                //            throw new Exception("");
                //        }

                //        return false;
                //    }
                //    else
                //    {
                //        Buffer = Buffer.Append((byte)a);
                //    }

                //    if (before == JsonEncoder.Backslash)
                //    {
                //        before = -1;
                //    }
                //    else
                //    {
                //        before = after;
                //    }
                    
                //    return true;
                //});
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

                var buf = JsonUtility.GetBytes(stream, ends.Append(JsonEncoder.Space));
                if (buf == null || buf.Length == 0)
                {
                    throw new Exception("");
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
                if (terminator == JsonEncoder.Space)
                {
                    terminator = (byte)stream.Except(JsonEncoder.Space);
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
                    throw new Exception("");
                }
                //var hasTrailingSpace = false;
                //JsonUtility.Feed(stream, (b) =>
                //{
                //    if (!JsonUtility.IsVisibleChar(b))
                //    {
                //        if (Buffer.Length == 0 || hasTrailingSpace)
                //        {
                //            return true;
                //        }
                //        else if(!hasTrailingSpace)
                //        {
                //            hasTrailingSpace = true;
                //            return true;
                //        }
                //        else
                //        {
                //            throw new Exception("");
                //        }
                //    }
                //    else if (seperators != null && seperators.Exists(x => x == b))
                //    {
                //        terminated = false;
                //        return false;
                //    }
                //    else if (terminators != null && terminators.Exists(x => x == b))
                //    {
                //        terminated = true;
                //        return false;
                //    }
                //    else
                //    {
                //        if (hasTrailingSpace)
                //        {
                //            throw new Exception("");
                //        }

                //        Buffer = Buffer.Append((byte)b);
                //        return true;
                //    }
                //});
            }

            return terminated;
        }
    }
}
