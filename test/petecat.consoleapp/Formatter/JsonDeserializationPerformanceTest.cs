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
            var example02 = "./formatter/examples/example02.json".FullPath();
            var example03 = "./formatter/examples/example03.json".FullPath();

            var count = 10000;

            var stopWatch = new Stopwatch();

            // from Petecat

            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>(example02);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>(example03);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            // ---------------------------------------------------------------------------------------- //

            // from Newtonsoft

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream(example02, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        new JsonSerializer().Deserialize(sr, typeof(BananaClass));
                    }
                }
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream(example03, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        new JsonSerializer().Deserialize(sr, typeof(BananaClass));
                    }
                }
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            // ------------------------------------------------------------------------------------------- //

            // from .Net framework

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream(example02, FileMode.Open, FileAccess.Read))
                {
                    new DataContractJsonSerializer(typeof(CherryClass)).ReadObject(stream);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("DataContractJsonSerializer 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream(example03, FileMode.Open, FileAccess.Read))
                {
                    new DataContractJsonSerializer(typeof(CherryClass)).ReadObject(stream);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("DataContractJsonSerializer 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);
        }
    }
}