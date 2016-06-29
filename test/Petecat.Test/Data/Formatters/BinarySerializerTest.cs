using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Petecat.Data.Formatters;

using System.Linq;


namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void Serialize()
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

            var byteValues = new BinaryFormatter().WriteBytes(product);

            var p = new BinaryFormatter().ReadObject(typeof(Product), byteValues, 0, byteValues.Length);
        }
    }
}
