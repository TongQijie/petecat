using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Petecat.Utility
{
    public static class CompressUtility
    {
        public static void GzipCompress(string fileToCompress, string compressedFile)
        {
            using (var outputStream = new FileStream(compressedFile, FileMode.Create, FileAccess.Write))
            {
                GzipCompress(fileToCompress, outputStream);
            }
        }

        public static void GzipDecompress(string fileToDecompress, string decompressedFile)
        {
            using (var outputStream = new FileStream(decompressedFile, FileMode.Create, FileAccess.Write))
            {
                GzipDecompress(decompressedFile, outputStream);
            }
        }

        public static void GzipCompress(string fileToCompress, Stream outputStream)
        {
            using (var inputStream = new FileStream(fileToCompress, FileMode.Open, FileAccess.Read))
            {
                GzipCompress(inputStream, outputStream);
            }
        }

        public static void GzipDecompress(string fileToDecompress, Stream outputStream)
        {
            using (var inputStream = new FileStream(fileToDecompress, FileMode.Open, FileAccess.Read))
            {
                GzipDecompress(inputStream, outputStream);
            }
        }

        public static void GzipCompress(Stream inputStream, Stream outputStream)
        {
            using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                inputStream.CopyTo(compressionStream);
            }
        }

        public static void GzipDecompress(Stream inputStream, Stream outputStream)
        {
            using (var decompressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(outputStream);
            }
        }
    }
}