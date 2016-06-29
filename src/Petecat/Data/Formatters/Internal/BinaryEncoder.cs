using System;
using System.Text;

namespace Petecat.Data.Formatters
{
    internal static class BinaryEncoder
    {
        public static byte[] Encode(object valueObject)
        {
            var valueType = valueObject.GetType();
            if (valueType == typeof(bool))
            {
                return BitConverter.GetBytes((bool)valueObject);
            }
            else if (valueType == typeof(char))
            {
                return BitConverter.GetBytes((char)valueObject);
            }
            else if (valueType == typeof(double))
            {
                return BitConverter.GetBytes((double)valueObject);
            }
            else if (valueType == typeof(float))
            {
                return BitConverter.GetBytes((float)valueObject);
            }
            else if (valueType == typeof(int))
            {
                return BitConverter.GetBytes((int)valueObject);
            }
            else if (valueType == typeof(long))
            {
                return BitConverter.GetBytes((long)valueObject);
            }
            else if (valueType == typeof(short))
            {
                return BitConverter.GetBytes((short)valueObject);
            }
            else if (valueType == typeof(uint))
            {
                return BitConverter.GetBytes((uint)valueObject);
            }
            else if (valueType == typeof(ulong))
            {
                return BitConverter.GetBytes((ulong)valueObject);
            }
            else if (valueType == typeof(ushort))
            {
                return BitConverter.GetBytes((ushort)valueObject);
            }
            else if (valueType == typeof(byte[]))
            {
                return valueObject as byte[];
            }
            else if (valueType == typeof(string))
            {
                return Encoding.UTF8.GetBytes(valueObject as string);
            }
            else if (valueType == typeof(decimal))
            {
                return Encoding.UTF8.GetBytes(valueObject.ToString());
            }
            else if (valueType == typeof(DateTime))
            {
                return BitConverter.GetBytes(((DateTime)valueObject).ToBinary());
            }
            else
            {
                return null;
            }
        }

        public static object Decode(Type targetType, byte[] byteValues, int startIndex, int length)
        {
            if (targetType == typeof(bool))
            {
                return BitConverter.ToBoolean(byteValues, startIndex);
            }
            else if (targetType == typeof(char))
            {
                return BitConverter.ToChar(byteValues, startIndex);
            }
            else if (targetType == typeof(double))
            {
                return BitConverter.ToDouble(byteValues, startIndex);
            }
            else if (targetType == typeof(float))
            {
                return BitConverter.ToSingle(byteValues, startIndex);
            }
            else if (targetType == typeof(int))
            {
                return BitConverter.ToInt32(byteValues, startIndex);
            }
            else if (targetType == typeof(long))
            {
                return BitConverter.ToInt64(byteValues, startIndex);
            }
            else if (targetType == typeof(short))
            {
                return BitConverter.ToInt16(byteValues, startIndex);
            }
            else if (targetType == typeof(uint))
            {
                return BitConverter.ToUInt32(byteValues, startIndex);
            }
            else if (targetType == typeof(ulong))
            {
                return BitConverter.ToUInt64(byteValues, startIndex);
            }
            else if (targetType == typeof(ushort))
            {
                return BitConverter.ToUInt16(byteValues, startIndex);
            }
            else if (targetType == typeof(byte[]))
            {
                var data = new byte[length];
                Buffer.BlockCopy(byteValues, startIndex, data, 0, length);
                return data;
            }
            else if (targetType == typeof(string))
            {
                var data = new byte[length];
                Buffer.BlockCopy(byteValues, startIndex, data, 0, length);
                return Encoding.UTF8.GetString(data);
            }
            else if (targetType == typeof(decimal))
            {
                return decimal.Parse(Encoding.UTF8.GetString(byteValues, startIndex, length));
            }
            else if (targetType == typeof(DateTime))
            {
                return DateTime.FromBinary(BitConverter.ToInt64(byteValues, startIndex));
            }
            else
            {
                return null;
            }
        }
    }
}
