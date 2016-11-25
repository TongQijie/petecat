using System.IO;
using System;

using Petecat.Utility;
using Petecat.Extending;
using System.Text;

namespace Petecat.Archiving
{
    public class ArchiveFile : AbstractArchiveItem
    {
        public long Length { get; set; }

        public string HashValue { get; set; }

        public override int GetFiles()
        {
            return 1;
        }

        public override void WriteHeader(Stream outputStream)
        {
            var header = new ArchiveEntityHeader()
            {
                RelativePath = RelativePath,
                Length = Length,
                HashValue = ToHexString(HashCalculator.Compute(HashCalculator.Algorithm.Sha1, AbsolutePath)),
            };

            header.WriteStream(outputStream);
        }

        public override void WriteContent(Stream outputStream)
        {
            using (var inputStream = new FileStream(AbsolutePath, FileMode.Open, FileAccess.Read))
            {
                inputStream.CopyTo(outputStream);
            }
        }

        public override void ReadHeader(Stream inputStream)
        {
            var header = new ArchiveEntityHeader();
            header.ReadStream(inputStream);

            RelativePath = header.RelativePath;
            Length = header.Length;
            HashValue = header.HashValue;
        }

        public override void ReadContent(Stream inputStream)
        {
            var folder = GetFolder();
            if (folder == null)
            {
                return;
            }
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var outputStream = new FileStream(AbsolutePath, FileMode.Create, FileAccess.Write))
            {
                var totalBytes = 0;
                var buffer = new byte[1024 * 4];

                var count = 0;
                while ((count = inputStream.Read(buffer, 0, (int)Math.Min(Length - totalBytes, buffer.Length))) > 0)
                {
                    totalBytes += count;
                    outputStream.Write(buffer, 0, count);

                    if (totalBytes >= Length)
                    {
                        break;
                    }
                }
            }

            var hashValue = ToHexString(HashCalculator.Compute(HashCalculator.Algorithm.Sha1, AbsolutePath));
            if (!hashValue.Equals(HashValue, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException();
            }
        }

        public string ToHexString(byte[] bytes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}
