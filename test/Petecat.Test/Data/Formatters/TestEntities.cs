using System;

namespace Petecat.Test.Data.Formatters
{
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
