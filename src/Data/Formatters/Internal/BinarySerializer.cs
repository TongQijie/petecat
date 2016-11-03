using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Petecat.Utility;

namespace Petecat.Data.Formatters
{
    internal class BinarySerializer
    {
        #region 常数定义

        public const byte ObjectMarker = 0xFF;

        public const byte PropertyNameMarker = 0xF1;

        public const byte PropertyValueMarker = 0xF0;

        public const int PropertyNameMaxLength = 0xFF;

        public const int PropertyValueMaxLength = 0xFFFF;

        #endregion

        #region 序列化方法

        public static byte[] Serialize(object instance)
        {
            var byteArray = new List<byte>();

            byteArray.Add(ObjectMarker);

            var type = instance.GetType();

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                var index = 0;
                foreach (var element in (instance as ICollection))
                {
                    if (element == null) { continue; }

                    InsertProperty(byteArray, index.ToString(), element);
                }
            }
            else
            {
                foreach (var propertyInfo in type.GetProperties())
                {
                    Attributes.BinarySerializableAttribute attribute = null;
                    if (!Reflector.TryGetCustomAttribute<Attributes.BinarySerializableAttribute>(propertyInfo, null, out attribute) || attribute.NonSerialized)
                    {
                        continue;
                    }

                    var propertyName = string.IsNullOrEmpty(attribute.Name) ? propertyInfo.Name : attribute.Name;
                    var propertyValue = propertyInfo.GetValue(instance, null);

                    InsertProperty(byteArray, propertyName, propertyValue);
                }
            }

            byteArray.Add(ObjectMarker);

            return byteArray.ToArray();
        }

        public static void Serialize(object instance, Stream outputStream)
        {
            outputStream.WriteByte(ObjectMarker);

            var type = instance.GetType();

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                var index = 0;
                foreach (var element in (instance as ICollection))
                {
                    if (element == null) { continue; }
                    InsertProperty(outputStream, index.ToString(), element);
                    index++;
                }
            }
            else
            {
                foreach (var propertyInfo in type.GetProperties())
                {
                    Attributes.BinarySerializableAttribute attribute = null;
                    if (!Reflector.TryGetCustomAttribute<Attributes.BinarySerializableAttribute>(propertyInfo, null, out attribute) || attribute.NonSerialized)
                    {
                        continue;
                    }

                    var propertyName = string.IsNullOrEmpty(attribute.Name) ? propertyInfo.Name : attribute.Name;
                    var propertyValue = propertyInfo.GetValue(instance, null);

                    InsertProperty(outputStream, propertyName, propertyValue);
                }
            }

            outputStream.WriteByte(ObjectMarker);
        }

        private static void InsertProperty(List<byte> byteArray, string propertyName, object propertyValue)
        {
            byteArray.Add(PropertyNameMarker);
            byteArray.Add((byte)(propertyName.Length));
            byteArray.AddRange(Encoding.UTF8.GetBytes(propertyName.ToString()));

            var valueByteArray = BinaryEncoder.Encode(propertyValue);
            if (valueByteArray != null)
            {
                if (valueByteArray.Length < PropertyValueMarker)
                {
                    byteArray.Add((byte)valueByteArray.Length);
                    byteArray.AddRange(valueByteArray);
                }
                else if (valueByteArray.Length < PropertyValueMaxLength)
                {
                    byteArray.Add(PropertyValueMarker);
                    byteArray.AddRange(new byte[] { (byte)(valueByteArray.Length >> 8), (byte)valueByteArray.Length });
                    byteArray.AddRange(valueByteArray);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                byteArray.AddRange(Serialize(propertyValue));
            }
        }

        private static void InsertProperty(Stream outputStream, string propertyName, object propertyValue)
        {
            outputStream.WriteByte(PropertyNameMarker);
            outputStream.WriteByte((byte)(propertyName.Length));
            var propertyNameBytes = Encoding.UTF8.GetBytes(propertyName.ToString());
            outputStream.Write(propertyNameBytes, 0, propertyNameBytes.Length);

            var valueByteArray = BinaryEncoder.Encode(propertyValue);
            if (valueByteArray != null)
            {
                if (valueByteArray.Length < PropertyValueMarker)
                {
                    outputStream.WriteByte((byte)valueByteArray.Length);
                    outputStream.Write(valueByteArray, 0, valueByteArray.Length);
                }
                else if (valueByteArray.Length < PropertyValueMaxLength)
                {
                    outputStream.WriteByte(PropertyValueMarker);
                    outputStream.WriteByte((byte)(valueByteArray.Length >> 8));
                    outputStream.WriteByte((byte)valueByteArray.Length);
                    outputStream.Write(valueByteArray, 0, valueByteArray.Length);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                var propertyValueBytes = Serialize(propertyValue);
                outputStream.Write(propertyValueBytes, 0, propertyValueBytes.Length);
            }
        }

        #endregion

        #region 反序列化方法

        public static object Deserialize(byte[] byteArray, Type targetType)
        {
            var offset = 0;
            return Deserialize(byteArray, ref offset, targetType);
        }

        public static T Deserialize<T>(byte[] byteArray)
        {
            var offset = 0;
            return (T)Deserialize(byteArray, ref offset, typeof(T));
        }

        private static object Deserialize(byte[] byteArray, ref int offset, Type targetType)
        {
            var instance = Activator.CreateInstance(targetType);

            while (offset < byteArray.Length)
            {
                if (byteArray[offset] == ObjectMarker)
                {
                    break;
                }
            }

            if (byteArray[offset++] != ObjectMarker)
            {
                throw new Exception();
            }

            while (offset < byteArray.Length)
            {
                if (byteArray[offset] == PropertyNameMarker)
                {
                    offset++;
                    var propertyNameLength = byteArray[offset++];
                    var propertyName = Encoding.UTF8.GetString(byteArray, offset, propertyNameLength);
                    offset += propertyNameLength;

                    if (typeof(ICollection).IsAssignableFrom(targetType))
                    {
                        var propertyType = targetType.GetGenericArguments().FirstOrDefault() ?? targetType.GetElementType();

                        if (byteArray[offset] == ObjectMarker)
                        {
                            (instance as IList).Add(Deserialize(byteArray, ref offset, propertyType));
                        }
                        else if (byteArray[offset] == PropertyValueMarker)
                        {
                            offset++;
                            var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
                            offset += 2;

                            (instance as IList).Add(BinaryEncoder.Decode(propertyType, byteArray, offset, propertyValueLength));

                            offset += propertyValueLength;
                        }
                        else if (byteArray[offset] < PropertyValueMarker)
                        {
                            var propertyValueLength = byteArray[offset++];

                            (instance as IList).Add(BinaryEncoder.Decode(propertyType, byteArray, offset, propertyValueLength));

                            offset += propertyValueLength;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        var propertyInfo = targetType.GetProperties().SingleOrDefault(x => x.CanRead && x.CanWrite
                            && ((x.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false).Length > 0 && (x.GetCustomAttributes(typeof(Attributes.BinarySerializableAttribute), false)[0] as Attributes.BinarySerializableAttribute).Name.Equals(propertyName))
                            || (x.Name.Equals(propertyName))));

                        if (byteArray[offset] == ObjectMarker)
                        {
                            propertyInfo.SetValue(instance, Deserialize(byteArray, ref offset, propertyInfo.PropertyType), null);
                        }
                        else if (byteArray[offset] == PropertyValueMarker)
                        {
                            offset++;
                            var propertyValueLength = (byteArray[offset] << 8) + byteArray[offset + 1];
                            offset += 2;

                            propertyInfo.SetValue(instance, BinaryEncoder.Decode(propertyInfo.PropertyType, byteArray, offset, propertyValueLength), null);

                            offset += propertyValueLength;
                        }
                        else if (byteArray[offset] < PropertyValueMarker)
                        {
                            var propertyValueLength = byteArray[offset++];

                            propertyInfo.SetValue(instance, BinaryEncoder.Decode(propertyInfo.PropertyType, byteArray, offset, propertyValueLength), null);

                            offset += propertyValueLength;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                else if (byteArray[offset] == ObjectMarker)
                {
                    offset++;
                    return instance;
                }
                else
                {
                    throw new Exception();
                }
            }

            return instance;
        }

        public static object Deserialize(Type targetType, Stream stream)
        {
            var instance = Activator.CreateInstance(targetType);

            var readByte = -1;
            while ((readByte = stream.ReadByte()) != -1)
            {
                if (readByte == ObjectMarker)
                {
                    break;
                }
            }

            readByte = stream.ReadByte();
            while (readByte != -1)
            {
                if (readByte == PropertyNameMarker)
                {
                    readByte = stream.ReadByte();
                    var propertyNameLength = readByte;
                    var propertyName = Encoding.UTF8.GetString(ReadStream(stream, propertyNameLength));

                    if (typeof(ICollection).IsAssignableFrom(targetType))
                    {
                        var propertyType = targetType.GetGenericArguments().FirstOrDefault() ?? targetType.GetElementType();

                        var marker = stream.ReadByte();
                        if (marker == ObjectMarker)
                        {
                            stream.Seek(-1, SeekOrigin.Current);
                            (instance as IList).Add(Deserialize(propertyType, stream));
                        }
                        else if (marker == PropertyValueMarker)
                        {
                            var propertyValueLength = (stream.ReadByte() << 8);
                            propertyValueLength += stream.ReadByte();

                            var byteValues = ReadStream(stream, propertyValueLength);
                            (instance as IList).Add(BinaryEncoder.Decode(propertyType, byteValues, 0, byteValues.Length));
                        }
                        else if (marker < PropertyValueMarker)
                        {
                            var propertyValueLength = marker;

                            var byteValues = ReadStream(stream, propertyValueLength);
                            (instance as IList).Add(BinaryEncoder.Decode(propertyType, byteValues, 0, byteValues.Length));
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        foreach (var propertyInfo in targetType.GetProperties())
                        {
                            Attributes.BinarySerializableAttribute attribute = null;
                            if (!Reflector.TryGetCustomAttribute<Attributes.BinarySerializableAttribute>(propertyInfo, x => x.Name == propertyName, out attribute))
                            {
                                continue;
                            }

                            var marker = stream.ReadByte();
                            if (marker == ObjectMarker)
                            {
                                stream.Seek(-1, SeekOrigin.Current);
                                propertyInfo.SetValue(instance, Deserialize(propertyInfo.PropertyType, stream), null);
                            }
                            else if (marker == PropertyValueMarker)
                            {
                                var propertyValueLength = (stream.ReadByte() << 8);
                                propertyValueLength += stream.ReadByte();

                                var byteValues = ReadStream(stream, propertyValueLength);
                                propertyInfo.SetValue(instance, BinaryEncoder.Decode(propertyInfo.PropertyType, byteValues, 0, byteValues.Length), null);
                            }
                            else if (marker < PropertyValueMarker)
                            {
                                var propertyValueLength = marker;

                                var byteValues = ReadStream(stream, propertyValueLength);
                                propertyInfo.SetValue(instance, BinaryEncoder.Decode(propertyInfo.PropertyType, byteValues, 0, byteValues.Length), null);
                            }
                            else
                            {
                                throw new Exception();
                            }

                            break;
                        }
                    }
                }
                else if (readByte == ObjectMarker)
                {
                    return instance;
                }
                else
                {
                    throw new Exception();
                }

                readByte = stream.ReadByte();
            }

            return instance;
        }

        #endregion

        #region 公共方法

        private static byte[] ReadStream(Stream stream, int count)
        {
            var buffer = new byte[count];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        #endregion
    }
}
