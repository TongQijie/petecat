using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters;
using System;
using System.IO;
using System.Text;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class DataContractJsonFormatterTest
    {
        [TestMethod]
        public void WriteObject_Test()
        {
            var product = new Product() { Id = 1, CheckInTime = DateTime.Now };

            using (var streamReader = new StreamReader("article.txt", Encoding.UTF8))
            {
                product.Name = streamReader.ReadToEnd();
            }

            using (var memoryStream = new MemoryStream())
            {
                ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson).WriteObject(product, memoryStream);
                var d = memoryStream.ToArray();
            }
        }

        [TestMethod]
        public void WriteString_Test()
        {
            var product = new Product() { Id = 1, CheckInTime = DateTime.Now, Name = "dddddd[{}]\'\\\"" };

            var d = ObjectFormatterFactory.GetFormatter(ObjectFormatterType.DataContractJson).WriteString(product, Encoding.UTF8);
        }
    }
}
