using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class BinaryFormatter : IDataFormatter
    {
        public T ReadObject<T>(string path, Encoding encoding)
        {
            using (var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return (T)ReadObject(typeof(T), inputStream);
            }
        }

        public T ReadObject<T>(string stringValue)
        {
            var byteValues = Convert.FromBase64String(stringValue);
            return (T)ReadObject(typeof(T), byteValues, 0, byteValues.Length);
        }

        public T ReadObject<T>(Stream stream)
        {
            return (T)BinarySerializer.Decode(typeof(T), stream);
        }

        public T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            var buffer = new byte[count];
            Buffer.BlockCopy(byteValues, offset, buffer, 0, count);
            return (T)BinarySerializer.Decode(buffer, typeof(T));
        }

        public object ReadObject(Type targetType, string path, Encoding encoding)
        {
            using (var inputStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return ReadObject(targetType, inputStream);
            }
        }

        public object ReadObject(Type targetType, string stringValue)
        {
            var byteValues = Convert.FromBase64String(stringValue);
            return ReadObject(targetType, byteValues, 0, byteValues.Length);
        }

        public object ReadObject(Type targetType, Stream stream)
        {
            return BinarySerializer.Decode(targetType, stream);
        }

        public object ReadObject(Type targetType, byte[] byteValues, int offset, int count)
        {
            var buffer = new byte[count];
            Buffer.BlockCopy(byteValues, offset, buffer, 0, count);
            return BinarySerializer.Decode(buffer, targetType);
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var outputStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                WriteObject(instance, outputStream);
            }
        }

        /// <summary>
        /// [Obsolete] replaced by WriteString(object instance);
        /// </summary>
        public string WriteObject(object instance)
        {
            return Convert.ToBase64String(BinarySerializer.Encode(instance));
        }

        public void WriteObject(object instance, Stream stream)
        {
            BinarySerializer.Encode(instance, stream);
        }

        public string WriteString(object instance)
        {
            return Convert.ToBase64String(BinarySerializer.Encode(instance));
        }

        public byte[] WriteBytes(object instance)
        {
            return BinarySerializer.Encode(instance);
        }
    }
}