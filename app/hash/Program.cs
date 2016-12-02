using Petecat.Extending;
using Petecat.Utility;
using System;
using System.IO;
using System.Text;

namespace Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("hash [filename]");
                return;
            }

            var filename = args[0];
            if (!filename.IsFile())
            {
                Console.WriteLine("file is not valid.");
                return;
            }

            using (var inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var data = HashCalculator.Compute(HashCalculator.Algorithm.Sha1, inputStream);
                Console.WriteLine("sha1 value: " + BinaryToString(data));
            }
        }

        static string BinaryToString(byte[] data)
        {
            var stringBuilder = new StringBuilder();

            foreach (var b in data)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}
