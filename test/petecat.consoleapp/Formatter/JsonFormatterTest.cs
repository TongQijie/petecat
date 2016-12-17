using Petecat.Formatter;
using Petecat.Extending;
using System.Text;

namespace Petecat.ConsoleApp.Formatter
{
    public class JsonFormatterTest
    {
        public void Run()
        {
            var obj1 = new JsonFormatter().ReadObject<DurianClass>("./formatter/examples/example01.json".FullPath());

            var obj2 = new JsonFormatter().ReadObject<GrapeClass[]>(obj1.Grapes);

            var str1 = new JsonFormatter().WriteString(obj1, Encoding.UTF8);

            var str2 = new JsonFormatter().WriteString(obj2, Encoding.UTF8);
        }
    }
}