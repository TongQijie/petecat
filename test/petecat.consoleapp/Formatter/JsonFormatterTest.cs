using Petecat.Formatter;
using Petecat.Extension;

namespace Petecat.ConsoleApp.Formatter
{
    public class JsonFormatterTest
    {
        public void Run()
        {
            var obj1 = new JsonFormatter().ReadObject<DurianClass>("./formatter/example.json".FullPath());

            var obj2 = new JsonFormatter().ReadObject<GrapeClass[]>(obj1.Grapes);
        }
    }
}