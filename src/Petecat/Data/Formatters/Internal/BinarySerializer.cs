using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petecat.Data.Formatters.Internal
{
    internal class BinarySerializer
    {
        public const byte ObjectMarker = 0xFF;

        public const byte PropertyNameMarker = 0xF1;

        public const byte PropertyValueMarker = 0xF0;

        public const int PropertyNameMaxLength = 0xFF;

        public const int PropertyValueMaxLength = 0xFFFF; 

        //public static byte[] Encode(object instance)
        //{
        //    var byteArray = new List<byte>();

        //    byteArray.Add(ObjectMarker);

        //    var type = instance.GetType();

        //    if (typeof(ICollection).IsAssignableFrom(type))
        //    {
        //        var index = 0;
        //        foreach (var element in (instance as ICollection))
        //        {
        //            if (element == null) { continue; }

        //            InsertProperty(byteArray, index.ToString(), element);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var propertyInfo in type.GetProperties().Where(x => x.CanRead && x.CanWrite && x.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false).Length > 0))
        //        {
        //            var attr = propertyInfo.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false)[0] as Attributes.BinarySerializableAttribute;
        //            if (attr.NonSerialized)
        //            {
        //                continue;
        //            }

        //            var propertyName = string.IsNullOrEmpty(attr.Name) ? propertyInfo.Name : attr.Name;
        //            var propertyValue = propertyInfo.GetValue(instance, null);

        //            InsertProperty(byteArray, propertyName, propertyValue);
        //        }
        //    }

        //    byteArray.Add(ObjectMarker);

        //    return byteArray.ToArray();
        //}

        //private static void InsertProperty(List<byte> byteArray, string propertyName, object propertyValue)
        //{
        //    byteArray.Add(PropertyNameMarker);
        //    byteArray.Add((byte)(propertyName.Length));
        //    byteArray.AddRange(Encoding.UTF8.GetBytes(propertyName.ToString()));

        //    var valueByteArray = BinEncoderConverter.ConvertTo(propertyValue);
        //    if (valueByteArray != null)
        //    {
        //        if (valueByteArray.Length < PropertyValueMarker)
        //        {
        //            byteArray.Add((byte)valueByteArray.Length);
        //            byteArray.AddRange(valueByteArray);
        //        }
        //        else if (valueByteArray.Length < PropertyValueMaxLength)
        //        {
        //            byteArray.Add(PropertyValueMarker);
        //            byteArray.AddRange(new byte[] { (byte)(valueByteArray.Length >> 8), (byte)valueByteArray.Length });
        //            byteArray.AddRange(valueByteArray);
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    else
        //    {
        //        byteArray.AddRange(Encode(propertyValue));
        //    }
        //}

        //public static object Decode(byte[] byteArray, Type targetType)
        //{
        //    var offset = 0;
        //    return Decode(byteArray, ref offset, targetType);
        //}

        //public static T Decode<T>(byte[] byteArray)
        //{
        //    var offset = 0;
        //    return (T)Decode(byteArray, ref offset, typeof(T));
        //}

        //private static object Decode(byte[] byteArray, ref int offset, Type targetType)
        //{
        //    var instance = Activator.CreateInstance(targetType);

        //    while (offset < byteArray.Length)
        //    {
        //        if (byteArray[offset] == ObjectMarker)
        //        {
        //            break;
        //        }
        //    }

        //    if (byteArray[offset++] != ObjectMarker)
        //    {
        //        throw new Exception();
        //    }

        //    while (offset < byteArray.Length)
        //    {
        //        if (byteArray[offset] == PropertyNameMarker)
        //        {
        //            offset++;
        //            var propertyNameLength = byteArray[offset++];
        //            var propertyName = Encoding.UTF8.GetString(byteArray, offset, propertyNameLength);
        //            offset += propertyNameLength;

        //            if (typeof(ICollection).IsAssignableFrom(targetType))
        //            {
        //                var propertyType = targetType.GetGenericArguments().FirstOrDefault() ?? targetType.GetElementType();

        //                if (byteArray[offset] == ObjectMarker)
        //                {
        //                    (instance as IList).Add(Decode(byteArray, ref offset, propertyType));
        //                }
        //                else if (byteArray[offset] == PropertyValueMarker)
        //                {
        //                    offset++;
        //                    var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
        //                    offset += 2;

        //                    (instance as IList).Add(BinEncoderConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyType));

        //                    offset += propertyValueLength;
        //                }
        //                else if (byteArray[offset] < PropertyValueMarker)
        //                {
        //                    var propertyValueLength = byteArray[offset++];

        //                    (instance as IList).Add(BinEncoderConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyType));

        //                    offset += propertyValueLength;
        //                }
        //                else
        //                {
        //                    throw new Exception();
        //                }
        //            }
        //            else
        //            {
        //                var propertyInfo = targetType.GetProperties().SingleOrDefault(x => x.CanRead && x.CanWrite
        //                    && ((x.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false).Length > 0 && (x.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false)[0] as Attributes.BinarySerializableAttribute).Name.Equals(propertyName))
        //                    || (x.Name.Equals(propertyName))));

        //                if (byteArray[offset] == ObjectMarker)
        //                {
        //                    propertyInfo.SetValue(instance, Decode(byteArray, ref offset, propertyInfo.PropertyType), null);
        //                }
        //                else if (byteArray[offset] == PropertyValueMarker)
        //                {
        //                    offset++;
        //                    var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
        //                    offset += 2;

        //                    propertyInfo.SetValue(instance, BinEncoderConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyInfo.PropertyType), null);

        //                    offset += propertyValueLength;
        //                }
        //                else if (byteArray[offset] < PropertyValueMarker)
        //                {
        //                    var propertyValueLength = byteArray[offset++];

        //                    propertyInfo.SetValue(instance, BinEncoderConverter.ConvertFrom(byteArray, offset, propertyValueLength, propertyInfo.PropertyType), null);

        //                    offset += propertyValueLength;
        //                }
        //                else
        //                {
        //                    throw new Exception();
        //                }
        //            }
        //        }
        //        else if (byteArray[offset] == ObjectMarker)
        //        {
        //            offset++;
        //            return instance;
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }
        //    }

        //    return instance;
        //}
    }
}
