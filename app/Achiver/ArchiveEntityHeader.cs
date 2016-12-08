using System;
using System.IO;
using System.Text;

using Petecat.Extending;

namespace Achiver
{
    internal class ArchiveEntityHeader
    {
        public string RelativePath { get; set; }

        public long Length { get; set; }

        public string HashValue { get; set; }

        public bool IsCompressed { get; set; }

        public void ReadStream(Stream inputStream)
        {
            RelativePath = ReadBlock(inputStream);
            Length = Convert.ToInt64(ReadBlock(inputStream));
            HashValue = ReadBlock(inputStream);
            IsCompressed = Convert.ToBoolean(ReadBlock(inputStream));
        }

        public void WriteStream(Stream outputStream)
        {
            WriteBlock(outputStream, RelativePath);
            WriteBlock(outputStream, Length.ToString());
            WriteBlock(outputStream, HashValue);
            WriteBlock(outputStream, IsCompressed.ToString());
        }

        private string ReadBlock(Stream inputStream)
        {
            var buffer = new byte[0];

            var b = -1;
            while ((b = inputStream.ReadByte()) != -1)
            {
                if (b != 0xFF)
                {
                    buffer = buffer.Append((byte)b);
                }
                else
                {
                    break;
                }
            }

            return Encoding.UTF8.GetString(buffer);
        }

        private void WriteBlock(Stream outputStream, string stringValue)
        {
            var data = Encoding.UTF8.GetBytes(stringValue);
            outputStream.Write(data, 0, data.Length);
            outputStream.WriteByte(0xFF);
        }
    }
}
