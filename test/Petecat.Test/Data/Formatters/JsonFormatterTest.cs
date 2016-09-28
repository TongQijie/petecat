using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters;
using Petecat.Data.Formatters.Internal.Json;
using Petecat.Extension;
using Petecat.Logging;
using Petecat.Logging.Loggers;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class JsonFormatterTest
    {
        public JsonFormatterTest()
        {
            LoggerManager.SetLogger(new FileLogger(LoggerManager.AppDomainLoggerName, "./log".FullPath()));
        }

        [TestMethod]
        public void WriteString_Test()
        {
            var product = new Product() { Id = 1, Name = "dddddd\\\"\\\\\"\\", IsSelected = true };
            product.Prices = new System.Collections.Generic.List<Price>();
            product.Prices.Add(new Price() { Region = null, Value = 1.2M });
            product.Prices.Add(new Price() { Region = "CHN", Value = 11111.2M });

            var d = new JsonFormatter().WriteString(product, Encoding.UTF8);

            var p = new JsonFormatter().ReadObject<Product>(d, Encoding.UTF8);

            Assert.IsTrue(p != null && p.Id == product.Id && p.Name == product.Name && p.IsSelected == product.IsSelected
                && p.CheckInTime.Equals(product.CheckInTime)
                && p.Prices != null && p.Prices.Count == 2
                && p.Prices[0].Region == null && p.Prices[0].Value == 1.2M
                && p.Prices[1].Region == "CHN" && p.Prices[1].Value == 11111.2M);
        }

        [TestMethod]
        public void WriteObject_Test()
        {
            var product = new Product() { Id = 1, CheckInTime = DateTime.Now, Name = "dddddd\\\"" };
            product.AnotherPrices = new Price[]
            {
                new Price() { Region = "USA", Value = 1.2M },
                new Price() { Region = "CHN", Value = 11111.2M },
            };
            product.Prices = new System.Collections.Generic.List<Price>();
            product.Prices.Add(new Price() { Region = "USA", Value = 1.2M });
            product.Prices.Add(new Price() { Region = "CHN", Value = 11111.2M });

            new JsonFormatter().WriteObject(product, "json.txt");
        }

        [TestMethod]
        public void Parse_0_Test()
        {
            using (var inputStream = new FileStream("json.txt", FileMode.Open, FileAccess.Read))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);
            }
        }

        [TestMethod]
        public void Parse_1_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[1.2]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
            }
        }

        [TestMethod]
        public void Parse_2_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[  1.2  , 3.4   ]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "3.4");
            }
        }

        [TestMethod]
        public void Parse_3_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[1.2,3.4,5.6]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 3);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "3.4");
                Assert.IsTrue(col.Elements[2].Value is JsonPlainValueObject
                    && (col.Elements[2].Value as JsonPlainValueObject).ToString() == "5.6");
            }
        }

        [TestMethod]
        public void Parse_4_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[\"1.2\"]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
            }
        }

        [TestMethod]
        public void Parse_5_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[  \"1.2\"  , \"3.4\" ]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "3.4");
            }
        }

        [TestMethod]
        public void Parse_6_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[\"1.2\",\"3.4\",\"5.6\"]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 3);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "3.4");
                Assert.IsTrue(col.Elements[2].Value is JsonPlainValueObject
                    && (col.Elements[2].Value as JsonPlainValueObject).ToString() == "5.6");
            }
        }

        [TestMethod]
        public void Parse_7_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Name\":\"abc\"}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonDictionaryObject);

                var col = dict.Elements[0].Value as JsonDictionaryObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1);
                Assert.IsTrue(col.Elements[0].Key == "Name"
                    && col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "abc");
            }
        }

        [TestMethod]
        public void Parse_8_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Name\":\"abc\",\"Age\":12}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonDictionaryObject);

                var col = dict.Elements[0].Value as JsonDictionaryObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Key == "Name"
                    && col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "abc");
                Assert.IsTrue(col.Elements[1].Key == "Age"
                    && col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "12");
            }
        }

        [TestMethod]
        public void Parse_9_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Age\":12,\"Name\":\"abc\"}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonDictionaryObject);

                var col = dict.Elements[0].Value as JsonDictionaryObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Key == "Age"
                    && col.Elements[0].Value is JsonPlainValueObject
                    && (col.Elements[0].Value as JsonPlainValueObject).ToString() == "12");
                Assert.IsTrue(col.Elements[1].Key == "Name"
                    && col.Elements[1].Value is JsonPlainValueObject
                    && (col.Elements[1].Value as JsonPlainValueObject).ToString() == "abc");
            }
        }

        [TestMethod]
        public void Parse_10_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[{\"Age\":12,\"Name\":\"abc\"}]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1 && col.Elements[0].Value is JsonDictionaryObject);

                var d = col.Elements[0].Value as JsonDictionaryObject;

                Assert.IsTrue(d.Elements != null && d.Elements.Length == 2);
                Assert.IsTrue(d.Elements[0].Key == "Age"
                    && d.Elements[0].Value is JsonPlainValueObject
                    && (d.Elements[0].Value as JsonPlainValueObject).ToString() == "12");
                Assert.IsTrue(d.Elements[1].Key == "Name"
                    && d.Elements[1].Value is JsonPlainValueObject
                    && (d.Elements[1].Value as JsonPlainValueObject).ToString() == "abc");
            }
        }

        [TestMethod]
        public void Parse_11_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[{\"Age\":12,\"Name\":\"abc\"},{\"Age\":34,\"Name\":\"def\"}]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = new BufferStream(inputStream, 4 * 1024),
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2 
                    && col.Elements[0].Value is JsonDictionaryObject
                    && col.Elements[1].Value is JsonDictionaryObject);

                var d1 = col.Elements[0].Value as JsonDictionaryObject;

                Assert.IsTrue(d1.Elements != null && d1.Elements.Length == 2);
                Assert.IsTrue(d1.Elements[0].Key == "Age"
                    && d1.Elements[0].Value is JsonPlainValueObject
                    && (d1.Elements[0].Value as JsonPlainValueObject).ToString() == "12");
                Assert.IsTrue(d1.Elements[1].Key == "Name"
                    && d1.Elements[1].Value is JsonPlainValueObject
                    && (d1.Elements[1].Value as JsonPlainValueObject).ToString() == "abc");

                var d2 = col.Elements[1].Value as JsonDictionaryObject;

                Assert.IsTrue(d2.Elements != null && d2.Elements.Length == 2);
                Assert.IsTrue(d2.Elements[0].Key == "Age"
                    && d2.Elements[0].Value is JsonPlainValueObject
                    && (d2.Elements[0].Value as JsonPlainValueObject).ToString() == "34");
                Assert.IsTrue(d2.Elements[1].Key == "Name"
                    && d2.Elements[1].Value is JsonPlainValueObject
                    && (d2.Elements[1].Value as JsonPlainValueObject).ToString() == "def");
            }
        }

        [TestMethod]
        public void Parse_12_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":100,\"Name\":\"apple\",\"CheckInTime\":\"2016-09-23 12:00:00\"}")))
            {
                var product = new JsonFormatter().ReadObject<Product>(inputStream);

                Assert.IsTrue(product.Id == 100 && product.Name == "apple" 
                    && product.CheckInTime.Year == 2016 && product.CheckInTime.Month == 9 && product.CheckInTime.Day == 23
                    && product.CheckInTime.Hour == 12 && product.CheckInTime.Minute == 0 && product.CheckInTime.Second == 0);
            }
        }

        [TestMethod]
        public void Parse_13_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":100,\"Name\":\"apple\",\"CheckInTime\":\"2016-09-23 12:00:00\",\"Prices\":[{\"Value\":12.1,\"Region\":\"CHN\"},{\"Region\":\"USA\",\"Value\":2222.111}]}")))
            {
                var product = new JsonFormatter().ReadObject<Product>(inputStream);

                Assert.IsTrue(product.Id == 100 && product.Name == "apple"
                    && product.CheckInTime.Year == 2016 && product.CheckInTime.Month == 9 && product.CheckInTime.Day == 23
                    && product.CheckInTime.Hour == 12 && product.CheckInTime.Minute == 0 && product.CheckInTime.Second == 0);

                Assert.IsTrue(product.Prices != null && product.Prices.Count == 2
                    && product.Prices[0].Value == 12.1M && product.Prices[0].Region == "CHN"
                    && product.Prices[1].Value == 2222.111M && product.Prices[1].Region == "USA");
            }
        }

        [TestMethod]
        public void Parse_14_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Id\":100,\"Name\":\"apple\",\"CheckInTime\":\"2016-09-23 12:00:00\",\"AnotherPrices\":[{\"Value\":12.1,\"Region\":\"CHN\"},{\"Region\":\"USA\",\"Value\":2222.111}]}")))
            {
                var product = new JsonFormatter().ReadObject<Product>(inputStream);

                Assert.IsTrue(product.Id == 100 && product.Name == "apple"
                    && product.CheckInTime.Year == 2016 && product.CheckInTime.Month == 9 && product.CheckInTime.Day == 23
                    && product.CheckInTime.Hour == 12 && product.CheckInTime.Minute == 0 && product.CheckInTime.Second == 0);

                Assert.IsTrue(product.AnotherPrices != null && product.AnotherPrices.Length == 2
                    && product.AnotherPrices[0].Value == 12.1M && product.AnotherPrices[0].Region == "CHN"
                    && product.AnotherPrices[1].Value == 2222.111M && product.AnotherPrices[1].Region == "USA");
            }
        }

        [TestMethod]
        public void JsonFormatter_ReadObject_Test()
        {
            //var filename = "";

            //Assert.IsTrue(File.Exists(filename));

            //var a = 0;
            //while(a < 1000)
            //{
            //    using (var inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            //    {
            //        //var product = new JsonFormatter().ReadObject<ArticleService.RepositoryModel.ArticleInfoSource>(inputStream);
            //    }
            //    a++;
            //}

            var jsonString = "{\"Attributes\":[{\"Id\":1}],\"Name\":\"a\"}";

            var entity = new JsonFormatter().ReadObject<Entity>(jsonString, Encoding.UTF8);

            Assert.IsTrue(entity != null && entity.Attributes != null && entity.Attributes.Length == 1 && entity.Attributes[0].Id == 1 && entity.Name == "a");

            jsonString = "{\"Attributes\":[],\"Name\":\"a\"}";

            entity = new JsonFormatter().ReadObject<Entity>(jsonString, Encoding.UTF8);

            Assert.IsTrue(entity != null && entity.Attributes != null && entity.Attributes.Length == 0 && entity.Name == "a");

            jsonString = "{\"Attributes\":[{}],\"Name\":\"a\"}";

            entity = new JsonFormatter().ReadObject<Entity>(jsonString, Encoding.UTF8);

            Assert.IsTrue(entity != null && entity.Attributes != null && entity.Attributes.Length == 1 && entity.Name == "a");

            jsonString = "{\"Attributes\":[{}],\"Name\":\"a\",\"Alias\":\"abc\"}";

            entity = new JsonFormatter().ReadObject<Entity>(jsonString, Encoding.UTF8);

            Assert.IsTrue(entity != null && entity.Attributes != null && entity.Attributes.Length == 1 && entity.Name == "a");
        }

        public class Entity
        {
            public string Name { get; set; }

            public Attribute[] Attributes { get; set; }

            public string Alias
            {
                get { return Name; }
            }
        }

        public class Attribute
        {
            public int Id { get; set; }
        }

        [TestMethod]
        public void DataContractJsonFormatter_ReadObject_Test()
        {
            var filename = "";

            Assert.IsTrue(File.Exists(filename));

            var a = 0;
            while(a < 1000)
            {
                using (var inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                   //var product = new DataContractJsonFormatter().ReadObject<ArticleService.RepositoryModel.ArticleInfoSource>(inputStream);
                }
                a++;
            }
        }

        //private ArticleService.RepositoryModel.ArticleInfoSource Article { get; set; }

        [TestMethod]
        public void JsonFormatter_WriteString_Test()
        {
            var a = 0;
            while (a < 1)
            {
                var article = new JsonFormatter() { OmitDefaultValueProperty = true }.WriteString(new Entity() { Name = "abc" }, Encoding.UTF8);
                a++;
            }
        }

        [TestMethod]
        public void DataContractJsonFormatter_WriteString_Test()
        {
            var a = 0;
            while (a < 1000)
            {
                //var article = new DataContractJsonFormatter().WriteString(Article, Encoding.UTF8);
                a++;
            }
        }
    }
}
