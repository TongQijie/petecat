using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters;
using Petecat.Data.Formatters.Internal.Json;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class JsonFormatterTest
    {
        [TestMethod]
        public void WriteObject_Test()
        {
            
        }

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
        public void Parse_Test()
        {
            using (var fileStream = new FileStream("json.txt", FileMode.Open, FileAccess.Read))
            {
                var args = new JsonObjectParseArgs()
                {
                    Stream = fileStream,
                };
                JsonObjectParser.Parse(args);
            }
        }
    }
}
