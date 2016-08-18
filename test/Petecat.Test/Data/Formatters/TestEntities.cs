using System;

using Petecat.Data.Attributes;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

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

    public class ItemInfo
    {
        [IniSerializable(Name = "detail")]
        public ItemDetail Detail { get; set; }

        [IniSerializable(Name = "price")]
        public ItemPrice Price { get; set; }
    }

    public class ItemDetail
    {
        [IniSerializable(Name = "name")]
        public string Name { get; set; }

        [IniSerializable(Name = "qty")]
        public int Qty { get; set; }
    }

    public class ItemPrice
    {
        [IniSerializable(Name = "price")]
        public decimal Price { get; set; }

        [IniSerializable(Name = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot("settings")]
    public class GlobalSettings
    {
        [XmlElement("network")]
        public NetworkSetting[] NetworkSettings { get; set; }
    }

    [XmlRoot("network")]
    public class NetworkSetting
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("desc")]
        public Description Description { get; set; }
    }

    public class Description
    {
        [XmlAnyElement]
        public XmlElement Node { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
