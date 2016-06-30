using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Petecat.Data.Formatters;

using System.Linq;
using System.IO;


namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void Serialize_ToBytes()
        {
            var product = GetEntity();

            var byteValues = new BinaryFormatter().WriteBytes(product);

            var anotherProduct = new BinaryFormatter().ReadObject(typeof(Product), byteValues, 0, byteValues.Length) as Product;

            Assert.IsTrue(product.Equals(anotherProduct));
        }

        [TestMethod]
        public void Serialize_ToStream()
        {
            var product = GetEntity();

            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().WriteObject(product, memoryStream);

                memoryStream.Seek(0, SeekOrigin.Begin);

                var anotherProduct = new BinaryFormatter().ReadObject<Product>(memoryStream);

                Assert.IsTrue(product.Equals(anotherProduct));
            }
        }

        [TestMethod]
        public void Serialize_ToFile()
        {
            var product = GetEntity();

            new BinaryFormatter().WriteObject(product, "product.dat", null);

            var anotherProduct = new BinaryFormatter().ReadObject<Product>("product.dat", null);

            Assert.IsTrue(product.Equals(anotherProduct));
        }

        private Product GetEntity()
        {
            var product = new Product()
            {
                CheckInTime = DateTime.Now,
                Id = 1,
                Name = "helllllllo",
                Prices = new Price[] 
                { 
                    new Price() { Region = "china", Value = 122.222M },
                    new Price() { Region = "usa", Value = 0.2M }
                }.ToList(),
            };

            return product;
        }
    }
}
