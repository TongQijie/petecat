using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Petecat.Data.DataContractJson;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Petecat.Data.Formatters;

namespace Petecat.Test.Data.DataContractJson
{
    [TestClass]
    public class SerializerTest
    {
        [TestMethod]
        public void Read()
        {
            Write();

            var product = Serializer.ReadObject<Product>("product.json", Encoding.UTF8);
        }

        [TestMethod]
        public void ReadString()
        {
            var product = Serializer.ReadObject<Product>("{\"name\":\"\\u000d\\u000a<p>this is a test article<\\/p>\\u000d\\u000a<p><img><\\/p>\"}");
        }
    
        [TestMethod]
        public void ReadFile()
        {
            using (var inputStream = new FileStream("test.txt", FileMode.Open, FileAccess.Read))
            {
                var d = new byte[1024];
                var c = inputStream.Read(d, 0, d.Length);

                var p = Serializer.ReadObject<BlogArticleResponse>(inputStream);
            }
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

            Serializer.WriteObject(product, "product.json", Encoding.UTF8);
        }

        [TestMethod]
        public void WriteFile()
        {
            var product = new Product() { Id = 1, Name = "this is product name<>TTTT", CheckInTime = DateTime.Now };
            product.Prices = new Price[] 
            {
                new Price() { Value = 100.0M, Region = "CHN" },
                new Price() { Value = 99.7M, Region = "USA" },
            };

            using (var inputStream = new FileStream("json-utf-8.json", FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[1024];
                inputStream.Read(buffer, 0, buffer.Length);
            }

            using (var outputStream = new FileStream("json-utf-8.json", FileMode.Create, FileAccess.Write))
            {
                new DataContractJsonFormatter().WriteObject(product, outputStream);
            }

            using (var inputStream = new FileStream("json-utf-8.json", FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[1024];
                inputStream.Read(buffer, 0, buffer.Length);
            }
        }
    }

    [DataContract]
    public class Product
    {
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public DateTime CheckInTime { get; set; }

        public Price[] Prices { get; set; }
    }

    public class Price
    {
        public decimal Value { get; set; }

        public string Region { get; set; }
    }

    [DataContract]
    public class BlogArticleResponse
    {
        public string Unit { get; set; }

        [DataMember(Name = "articles")]
        public BlogArticle[] Articles { get; set; }

        [DataMember(Name = "error")]
        public bool HasError { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }

    [DataContract]
    public class BlogArticle
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "catagory")]
        public string Catagory { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "subtitle")]
        public string Subtitle { get; set; }

        [DataMember(Name = "creationDate")]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "lastEditDate")]
        public DateTime LastEditDate { get; set; }

        [DataMember(Name = "writer")]
        public string Writer { get; set; }

        [DataMember(Name = "htmlContent")]
        public string HtmlContent { get; set; }

        [DataMember(Name = "private")]
        public bool IsPrivate { get; set; }

        [DataMember(Name = "blocked")]
        public bool IsBlocked { get; set; }
    }
}
