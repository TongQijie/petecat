using System;

using Petecat.Data.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Petecat.Test.Data.Formatters
{
    [DataContract]
    public class Product
    {
        [DataMember(Name = "id")]
        [BinarySerializable("id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        [BinarySerializable("name")]
        public string Name { get; set; }

        [DataMember(Name = "time")]
        [BinarySerializable("time")]
        public DateTime CheckInTime { get; set; }

        [DataMember(Name = "prices")]
        [BinarySerializable("prices")]
        public List<Price> Prices { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Product)
            {
                var anotherProduct = obj as Product;
                if (Id != anotherProduct.Id || Name != anotherProduct.Name || CheckInTime != anotherProduct.CheckInTime)
                {
                    return false;
                }

                if ((Prices == null && anotherProduct.Prices != null) || (Prices != null && anotherProduct.Prices == null))
                {
                    return false;
                }

                if (Prices != null && anotherProduct.Prices != null)
                {
                    if (Prices.Count != anotherProduct.Prices.Count)
                    {
                        return false;
                    }

                    for (int i = 0; i < Prices.Count; i++)
                    {
                        if (Prices[i].Value != anotherProduct.Prices[i].Value || Prices[i].Region != anotherProduct.Prices[i].Region)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract]
    public class Price
    {
        [DataMember(Name = "value")]
        [BinarySerializable("value")]
        public decimal Value { get; set; }

        [DataMember(Name = "region")]
        [BinarySerializable("region")]
        public string Region { get; set; }
    }
}
