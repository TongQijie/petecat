using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Petecat.Utility
{
    public static class CompressUtility
    {
        public static void GzipCompress(FileInfo fileToCompress)
        {
            using (var originalFileStream = fileToCompress.OpenRead())
            {
                using (var compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public static void GzipDecompress(FileInfo fileToDecompress)
        {
            using (var originalFileStream = fileToDecompress.OpenRead())
            {
                using (var decompressedFileStream = File.Create(fileToDecompress.FullName.Remove(fileToDecompress.FullName.Length - fileToDecompress.Extension.Length)))
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        public static void Archive(FileInfo[] filesToArchive, string archiveFileName)
        {
            if (filesToArchive == null || filesToArchive.Length == 0)
            {
                return;
            }

            var archiveFileHeader = new ArchiveFileHeader()
            {
                Entities = new ArchiveEntityHeader[filesToArchive.Length]
            };

            for (int i = 0; i < archiveFileHeader.Entities.Length; i++)
            {
                archiveFileHeader.Entities[i] = new ArchiveEntityHeader();
                archiveFileHeader.Entities[i].Fullname = filesToArchive[i].FullName;
                archiveFileHeader.Entities[i].Length = filesToArchive[i].Length;
                archiveFileHeader.Entities[i].Hash = "HHHHHHHHHHash"; // generate hash
            }

            using (var archivefileStream = new FileStream(archiveFileName, FileMode.Create, FileAccess.Write))
            {
                archiveFileHeader.WriteStream(archivefileStream);

                foreach (var fileToArchive in filesToArchive)
                {
                    using (var fileStream = new FileStream(fileToArchive.FullName, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.CopyTo(archivefileStream);
                    }
                }
            }
        }

        public static void Unarchive(string fileToUnarchive, string unarchiveDirectory)
        {
            if (!Directory.Exists(unarchiveDirectory))
            {
                Directory.CreateDirectory(unarchiveDirectory);
            }

            using (var unarchiveFileStream = new FileStream(fileToUnarchive, FileMode.Open, FileAccess.Read))
            {
                var archiveFileHeader = new ArchiveFileHeader(unarchiveFileStream);

                foreach (var entity in archiveFileHeader.Entities)
                {
                    var unarchiveFilename = Path.Combine(unarchiveDirectory, new FileInfo(entity.Fullname).Name);

                    using (var outputStream = new FileStream(unarchiveFilename, FileMode.Create, FileAccess.Write))
                    {
                        var totalBytes = 0;
                        var buffer = new byte[1024 * 4];

                        var count = 0;
                        while ((count = unarchiveFileStream.Read(buffer, 0, (int)Math.Min(entity.Length - totalBytes, buffer.Length))) > 0)
                        {
                            totalBytes += count;
                            outputStream.Write(buffer, 0, count);

                            if (totalBytes >= entity.Length)
                            {
                                break;
                            }
                        }
                    }

                    // check hash
                }
            }
        }

        public class ArchiveFileHeader
        {
            public ArchiveFileHeader()
            {
            }

            public ArchiveFileHeader(Stream inputStream)
            {
                var fileCount = Convert.ToInt32(Encoding.UTF8.GetString(ReadBlock(inputStream)));
                Entities = new ArchiveEntityHeader[fileCount];
                for (int i = 0; i < fileCount; i++)
                {
                    var entity = new ArchiveEntityHeader()
                    {
                        Fullname = Encoding.UTF8.GetString(ReadBlock(inputStream)),
                        Length = Convert.ToInt32(Encoding.UTF8.GetString(ReadBlock(inputStream))),
                        Hash = Encoding.UTF8.GetString(ReadBlock(inputStream))
                    };

                    Entities[i] = entity;
                }
            }

            public void WriteStream(Stream outputStream)
            {
                if (Entities == null || Entities.Length == 0)
                {
                    return;
                }

                WriteBlock(outputStream, Entities.Length);

                foreach (var entity in Entities)
                {
                    WriteBlock(outputStream, entity.Fullname);
                    WriteBlock(outputStream, entity.Length);
                    WriteBlock(outputStream, entity.Hash);
                }
            }

            private byte[] ReadBlock(Stream inputStream)
            {
                var block = new byte[0];

                var b = -1;
                while ((b = inputStream.ReadByte()) != -1)
                {
                    if (b != 0xFF)
                    {
                        block = block.Concat(new byte[] { (byte)b }).ToArray();
                    }
                    else
                    {
                        break;
                    }
                }

                return block;
            }

            private void WriteBlock(Stream outputStream, object value)
            {
                var data = Encoding.UTF8.GetBytes(value.ToString());
                outputStream.Write(data, 0, data.Length);
                outputStream.WriteByte(0xFF);
            }

            public ArchiveEntityHeader[] Entities { get; set; }
        }

        public class ArchiveEntityHeader
        {
            public string Fullname { get; set; }

            public long Length { get; set; }

            public string Hash { get; set; }
        }
    }
}