using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Petecat.Data.Formatters;
using System.Text;
using System.Linq;
using System.IO;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class DataFormatterTest
    {
        private readonly DataFormatterContent DataFormatContent = DataFormatterContent.DataContractJson;

        private readonly string Filename = "product.json";

        [TestMethod]
        public void ReadObject_StringValue()
        {
            var stringValue = DataFormatterUtility.Get(DataFormatContent).WriteObject(BuildEntities());

            var product = DataFormatterUtility.Get(DataFormatContent).ReadObject<Product>(stringValue);
        }

        [TestMethod]
        public void ReadObject_File()
        {
            var product = DataFormatterUtility.Get(DataFormatContent).ReadObject<Product>(Filename, Encoding.UTF8);
        }

        [TestMethod]
        public void ReadObject_Stream()
        {
            using (var memoryStream = new MemoryStream())
            {
                DataFormatterUtility.Get(DataFormatContent).WriteObject(BuildEntities(), memoryStream);

                using (var ms = new MemoryStream(memoryStream.ToArray()))
                {
                    var product = DataFormatterUtility.Get(DataFormatContent).ReadObject<Product>(ms);
                }
            }
        }

        [TestMethod]
        public void WriteObject_StringValue()
        {
            var stringValue = DataFormatterUtility.Get(DataFormatContent).WriteObject(BuildEntities());
        }

        [TestMethod]
        public void WriteObject_File()
        {
            DataFormatterUtility.Get(DataFormatContent).WriteObject(BuildEntities(), Filename, Encoding.UTF8);
        }

        [TestMethod]
        public void WriteObject_Stream()
        {
            using (var memoryStream = new MemoryStream())
            {
                DataFormatterUtility.Get(DataFormatContent).WriteObject(BuildEntities(), memoryStream);

                var stringValue = Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        private Product BuildEntities()
        {
            var product = new Product() { Id = 1, Name = "this is product name<>TTTT", CheckInTime = DateTime.Now };
            product.Prices = new Price[]
            {
                new Price() { Value = 100.0M, Region = "CHN" },
                new Price() { Value = 99.7M, Region = "USA" },
            }.ToList();

            return product;
        }
    }
}
