using Petecat.Console;
using Petecat.Extending;
using Petecat.Data.Formatters;

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace Formatter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleBridging.Write("input path: ");
            var inputPath = ConsoleBridging.ReadLine().FullPath();
            if (!File.Exists(inputPath))
            {
                ConsoleBridging.WriteLine("file does not exist. path: " + inputPath);
                return;
            }

            ConsoleBridging.Write("output path: ");
            var outputPath = ConsoleBridging.ReadLine().FullPath();

            var formatterConfig = new XmlFormatter().ReadObject<Configuration.FormatterConfig>("./format.config".FullPath());
            if (formatterConfig == null)
            {
                ConsoleBridging.WriteLine("format.config does not exist.");
                return;
            }

            if (formatterConfig.TextConfig == null)
            {
                ConsoleBridging.WriteLine("text config does not exist.");
                return;
            }

            using (var inputStream = new FileStream(inputPath, FileMode.Open, FileAccess.Read))
            {
                using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ConvertBOM(inputStream, outputStream, formatterConfig.TextConfig);

                    var newBytesSet = new List<byte[]>();
                    var oldBytesSet = new List<byte[]>();

                    if (formatterConfig.TextConfig.ReplaceTabWithSpaces)
                    {
                        var spaces = new byte[formatterConfig.TextConfig.SpacesForTab];
                        for (int i = 0; i < spaces.Length; i++)
                        {
                            spaces[i] = Convert.ToByte(' ');
                        }

                        oldBytesSet.Add(new byte[] { Convert.ToByte('\t') });
                        newBytesSet.Add(spaces);
                    }

                    if (formatterConfig.TextConfig.NewLineStyle.Equals("CRLF", StringComparison.OrdinalIgnoreCase))
                    {
                        oldBytesSet.Add(new byte[] { 0x0d, 0x0a });
                        oldBytesSet.Add(new byte[] { 0x0a });
                        newBytesSet.Add(new byte[] { 0x0d, 0x0a });
                        newBytesSet.Add(new byte[] { 0x0d, 0x0a });
                    }
                    else if (formatterConfig.TextConfig.NewLineStyle.Equals("LF"))
                    {
                        oldBytesSet.Add(new byte[] { 0x0d, 0x0a });
                        oldBytesSet.Add(new byte[] { 0x0a });
                        newBytesSet.Add(new byte[] { 0x0a });
                        newBytesSet.Add(new byte[] { 0x0a });
                    }

                    ReplaceBytes(inputStream, outputStream, oldBytesSet.ToArray(), newBytesSet.ToArray());
                }
            }

            ConsoleBridging.Write("done.");
        }

        static void ConvertBOM(Stream inputStream, Stream outputStream, Configuration.TextConfig config)
        {
            var buffer = new byte[3];
            var count = inputStream.Read(buffer, 0, 3);
            if (count < 3)
            {
                inputStream.Seek(-count, SeekOrigin.Current);
                return;
            }

            if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
            {
                if (config.ContainsBOM)
                {
                    outputStream.Write(buffer, 0, 3);
                }
            }
            else
            {
                if (config.ContainsBOM)
                {
                    var bom = new byte[3] { 0xef, 0xbb, 0xbf };
                    outputStream.Write(bom, 0, 3);
                }
                inputStream.Seek(-3, SeekOrigin.Current);
            }
        }

        static void ReplaceBytes(Stream inputStream, Stream outputStream, byte[] oldBytes, byte[] newBytes)
        {
            var buffer = new byte[1024];
            var count = 0;
            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var subArray = new byte[count];
                Array.Copy(buffer, subArray, count);

                var startIndex = 0;
                while ((startIndex = IndexOfEx<byte>(subArray, oldBytes, 0, count)) >= 0)
                {
                    outputStream.Write(subArray, 0, startIndex);
                    outputStream.Write(newBytes, 0, newBytes.Length);

                    count = count - startIndex - oldBytes.Length;
                    startIndex += oldBytes.Length;
                    subArray = subArray.Subset(startIndex);
                }

                if (count > 0)
                {
                    outputStream.Write(subArray, 0, count);
                }
            }
        }

        static void ReplaceBytes(Stream inputStream, Stream outputStream, byte[][] oldBytesSet, byte[][] newBytesSet)
        {
            var buffer = new byte[1024];
            var count = 0;
            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                var subArray = new byte[count];
                Array.Copy(buffer, subArray, count);

                var startIndex = 0;
                int index;
                while ((startIndex = IndexOfEx<byte>(subArray, oldBytesSet, 0, count, out index)) >= 0)
                {
                    outputStream.Write(subArray, 0, startIndex);
                    outputStream.Write(newBytesSet[index], 0, newBytesSet[index].Length);

                    count = count - startIndex - oldBytesSet[index].Length;
                    startIndex += oldBytesSet[index].Length;
                    subArray = subArray.Subset(startIndex);
                }

                if (count > 0)
                {
                    outputStream.Write(subArray, 0, count);
                }
            }
        }

        static int IndexOfEx<T>(T[] data, T[] findBytes, int startIndex, int count)
        {
            for (int i = startIndex; i < data.Length && i < (startIndex + count); i++)
            {
                var k = i;
                var foundBytes = true;
                for (int j = 0; j < findBytes.Length && k < data.Length && k < (startIndex + count); j++, k++)
                {
                    if (!data[k].Equals(findBytes[j]))
                    {
                        foundBytes = false;
                        break;
                    }
                }

                if (foundBytes)
                {
                    return i;
                }
            }

            return -1;
        }

        static int IndexOfEx<T>(T[] data, T[][] findBytesSet, int startIndex, int count, out int index)
        {
            for (int i = startIndex; i < data.Length && i < (startIndex + count); i++)
            {
                for (int h = 0; h < findBytesSet.Length; h++)
                {
                    var foundBytes = true;
                    var k = i;
                    for (int j = 0; j < findBytesSet[h].Length && k < data.Length && k < (startIndex + count); j++, k++)
                    {
                        if (!data[k].Equals(findBytesSet[h][j]))
                        {
                            foundBytes = false;
                            break;
                        }
                    }

                    if (foundBytes)
                    {
                        index = h;
                        return i;
                    }
                }
            }

            index = -1;
            return -1;
        }
    }
}
