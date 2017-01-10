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
    public class JsonFormatterPerformanceTest
    {
        public void Run()
        {
            var example02 = "./formatter/examples/example02.json".FullPath();
            var example03 = "./formatter/examples/example03.json".FullPath();

            var obj2 = new JsonFormatter().ReadObject<AppleClass>(example02);
            var obj3 = new JsonFormatter().ReadObject<AppleClass>(example03);

            var count = 10000;

            var stopWatch = new Stopwatch();

            // from Petecat

            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>(example02);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter read 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                new JsonFormatter().ReadObject<AppleClass>(example03);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter read 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Start();

            var formatter = new JsonFormatter();

            for (int i = 0; i < count; i++)
            {
                formatter.WriteString(obj2, Encoding.UTF8);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter write 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                formatter.WriteString(obj3, Encoding.UTF8);
            }

            stopWatch.Stop();

            Console.WriteLine("JsonFormatter write 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

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

            Console.WriteLine("Newtonsoft read 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

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

            Console.WriteLine("Newtonsoft read 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                JsonConvert.SerializeObject(obj2);
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft write 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Start();

            for (int i = 0; i < count; i++)
            {
                JsonConvert.SerializeObject(obj3);
            }

            stopWatch.Stop();

            Console.WriteLine("Newtonsoft write 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

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

            Console.WriteLine("DataContractJsonSerializer read 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            stopWatch.Restart();

            for (int i = 0; i < count; i++)
            {
                using (var stream = new FileStream(example03, FileMode.Open, FileAccess.Read))
                {
                    new DataContractJsonSerializer(typeof(CherryClass)).ReadObject(stream);
                }
            }

            stopWatch.Stop();

            Console.WriteLine("DataContractJsonSerializer read 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            //stopWatch.Start();

            //for (int i = 0; i < count; i++)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        new DataContractJsonSerializer(.WriteObject(memoryStream, obj2);
            //    }
            //}

            //stopWatch.Stop();

            //Console.WriteLine("DataContractJsonSerializer write 'example02': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);

            //stopWatch.Start();

            //for (int i = 0; i < count; i++)
            //{
            //    using (var memoryStream = new MemoryStream())
            //    {
            //        new DataContractJsonSerializer(typeof(CherryClass)).WriteObject(memoryStream, obj3);
            //    }
            //}

            //stopWatch.Stop();

            //Console.WriteLine("DataContractJsonSerializer write 'example03': cost {0} ms", stopWatch.Elapsed.TotalMilliseconds);
        }
    }
}