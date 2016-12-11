using Newtonsoft.Json;
using Petecat.Extending;
using Petecat.Formatter;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System;

namespace Petecat.ConsoleApp.Formatter
{
    public class JsonDeserializationPerformanceTest
    {
        public void Run()
        {
            var count = 1000;

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>("./formatter/3ixpc1re3x0.json".FullPath());
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter '3ixpc1re3x0.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>("./formatter/fbktgdhqboo.json".FullPath());
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter 'fbktgdhqboo.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream("./formatter/3ixpc1re3x0.json".FullPath(), FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        new JsonSerializer().Deserialize(sr, typeof(BananaClass));
                    }
                }
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft '3ixpc1re3x0.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream("./formatter/fbktgdhqboo.json".FullPath(), FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        new JsonSerializer().Deserialize(sr, typeof(BananaClass));
                    }
                }
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft 'fbktgdhqboo.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream("./formatter/3ixpc1re3x0.json".FullPath(), FileMode.Open, FileAccess.Read))
                {
                    new DataContractJsonSerializer(typeof(CherryClass)).ReadObject(stream);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("DataContractJsonSerializer '3ixpc1re3x0.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream("./formatter/fbktgdhqboo.json".FullPath(), FileMode.Open, FileAccess.Read))
                {
                    new DataContractJsonSerializer(typeof(CherryClass)).ReadObject(stream);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("DataContractJsonSerializer 'fbktgdhqboo.json': cost {0}", stopWatch.Elapsed.TotalMilliseconds);
        }
    }
}