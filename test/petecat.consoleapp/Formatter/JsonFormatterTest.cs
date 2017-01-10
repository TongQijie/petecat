using Petecat.Formatter;
using Petecat.Extending;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace Petecat.ConsoleApp.Formatter
{
    public class JsonFormatterTest
    {
        public void Run()
        {
            //while (true)
            //{
            //    var input = Console.ReadLine();
            //    var pattern = "([\\\\\"/\\b\\f\\n\\r\\t]{1})";
            //    var matched = Regex.IsMatch(input, pattern);
            //    var output = Regex.Replace(input, pattern, "\\$1");

            //    Console.WriteLine(output);
            //}
            

            var obj1 = new JsonFormatter().ReadObject<DurianClass>("./formatter/examples/example01.json".FullPath());

            var obj2 = new JsonFormatter().ReadObject<GrapeClass[]>(obj1.Grapes);

            var str1 = new JsonFormatter().WriteString(obj1, Encoding.UTF8);

            var str2 = new JsonFormatter().WriteString(obj2, Encoding.UTF8);
        }
    }
}