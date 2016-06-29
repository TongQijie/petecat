using System;

using Petecat.Data.Attributes;
using System.Collections.Generic;

namespace Petecat.Test.Data.Formatters
{
    public class Product
    {
        [BinarySerializable("id")]
        public int Id { get; set; }

        [BinarySerializable("name")]
        public string Name { get; set; }

        [BinarySerializable("time")]
        public DateTime CheckInTime { get; set; }

        [BinarySerializable("prices")]
        public List<Price> Prices { get; set; }
    }

    public class Price
    {
        [BinarySerializable("value")]
        public decimal Value { get; set; }

        [BinarySerializable("region")]
        public string Region { get; set; }
    }
}
