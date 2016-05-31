using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Data.DataContractXml;
using System;
using System.Text;

namespace Petecat.Test.Data.DataContractXml
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void Read()
        {
            Write();

            var product = Serializer.ReadObject<Product>("product.xml", Encoding.UTF8);
        }

        [TestMethod]
        public void Write()
        {
            var product = new Product() { Id = 1, Name = "this is product name<>TTTT", CheckInTime = DateTime.Now };
            product.Prices = new Price[] 
            {
                new Price() { Value = 100.0M, Region = "CHN" },
                new Price() { Value = 99.7M, Region = "USA" },
            };

            Serializer.WriteObject(product, "product.xml", Encoding.UTF8);
        }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CheckInTime { get; set; }

        public Price[] Prices { get; set; }
    }

    public class Price
    {
        public decimal Value { get; set; }

        public string Region { get; set; }
    }
}
