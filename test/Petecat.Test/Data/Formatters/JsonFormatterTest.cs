﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters;
using Petecat.Data.Formatters.Internal.Json;
using System;
using System.IO;
using System.Text;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class JsonFormatterTest
    {
        [TestMethod]
        public void WriteString_Test()
        {
            var product = new Product() { Id = 1, CheckInTime = DateTime.Now, Name = "dddddd[{}]\'\"" };
            product.AnotherPrices = new Price[]
            {
                new Price() { Region = "USA", Value = 1.2M },
                new Price() { Region = "CHN", Value = 11111.2M },
            };
            product.Prices = new System.Collections.Generic.List<Price>();
            product.Prices.Add(new Price() { Region = "USA", Value = 1.2M });
            product.Prices.Add(new Price() { Region = "CHN", Value = 11111.2M });

            var d = new JsonFormatter().WriteString(product);
        }

        [TestMethod]
        public void WriteObject_Test()
        {
            var product = new Product() { Id = 1, CheckInTime = DateTime.Now, Name = "dddddd[{}]\'\"" };
            product.AnotherPrices = new Price[]
            {
                new Price() { Region = "USA", Value = 1.2M },
                new Price() { Region = "CHN", Value = 11111.2M },
            };
            product.Prices = new System.Collections.Generic.List<Price>();
            product.Prices.Add(new Price() { Region = "USA", Value = 1.2M });
            product.Prices.Add(new Price() { Region = "CHN", Value = 11111.2M });

            new JsonFormatter().WriteObject(product, "json.txt", null);
        }

        [TestMethod]
        public void Parse_0_Test()
        {
            using (var inputStream = new FileStream("json.txt", FileMode.Open, FileAccess.Read))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
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
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
            }
        }

        [TestMethod]
        public void Parse_2_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[  1.2  , 3.4   ]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "3.4");
            }
        }

        [TestMethod]
        public void Parse_3_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[1.2,3.4,5.6]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 3);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "3.4");
                Assert.IsTrue(col.Elements[2].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[2].Value as JsonPlainValueObject).Buffer) == "5.6");
            }
        }

        [TestMethod]
        public void Parse_4_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[\"1.2\"]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 1);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
            }
        }

        [TestMethod]
        public void Parse_5_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[  \"1.2\"  , \"3.4\" ]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 2);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "3.4");
            }
        }

        [TestMethod]
        public void Parse_6_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[\"1.2\",\"3.4\",\"5.6\"]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
                };
                JsonObjectParser.Parse(args);

                Assert.IsTrue(args.InternalObject != null && args.InternalObject is JsonDictionaryObject);

                var dict = args.InternalObject as JsonDictionaryObject;

                Assert.IsTrue(dict.Elements != null && dict.Elements.Length == 1
                    && dict.Elements[0].Key == "Prices" && dict.Elements[0].Value is JsonCollectionObject);

                var col = dict.Elements[0].Value as JsonCollectionObject;

                Assert.IsTrue(col.Elements != null && col.Elements.Length == 3);
                Assert.IsTrue(col.Elements[0].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "1.2");
                Assert.IsTrue(col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "3.4");
                Assert.IsTrue(col.Elements[2].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[2].Value as JsonPlainValueObject).Buffer) == "5.6");
            }
        }

        [TestMethod]
        public void Parse_7_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Name\":\"abc\"}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
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
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "abc");
            }
        }

        [TestMethod]
        public void Parse_8_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Name\":\"abc\",\"Age\":12}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
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
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "abc");
                Assert.IsTrue(col.Elements[1].Key == "Age"
                    && col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "12");
            }
        }

        [TestMethod]
        public void Parse_9_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":{\"Age\":12,\"Name\":\"abc\"}}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
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
                    && Encoding.UTF8.GetString((col.Elements[0].Value as JsonPlainValueObject).Buffer) == "12");
                Assert.IsTrue(col.Elements[1].Key == "Name"
                    && col.Elements[1].Value is JsonPlainValueObject
                    && Encoding.UTF8.GetString((col.Elements[1].Value as JsonPlainValueObject).Buffer) == "abc");
            }
        }

        [TestMethod]
        public void Parse_10_Test()
        {
            using (var inputStream = new MemoryStream(Encoding.UTF8.GetBytes("{\"Prices\":[{\"Age\":12,\"Name\":\"abc\"}]}")))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = inputStream,
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
                    Stream = inputStream,
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
    }
}