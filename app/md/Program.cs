using Petecat.Extending;
using System;
using System.IO;
using System.Text;

namespace Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 2)
            {
                Console.WriteLine("arguments error. md [input] [output]");
                return;
            }

            if (!args[0].IsFile())
            {
                Console.WriteLine("input file not found.");
                return;
            }

            var md = "";
            using (var inputStream = new StreamReader(args[0], Encoding.UTF8))
            {
                md = inputStream.ReadToEnd();
            }

            var translator = new MarkdownTranslator();
            var html = translator.Transform(md);

            using (var outputStream = new StreamWriter(args[1], false, Encoding.UTF8))
            {
                outputStream.WriteLine(html);
            }
        }
    }
}
