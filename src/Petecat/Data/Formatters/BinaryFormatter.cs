using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class BinaryFormatter : IDataFormatter
    {
        public T ReadObject<T>(string path, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }

        public T ReadObject<T>(string stringValue)
        {
            throw new System.NotImplementedException();
        }

        public T ReadObject<T>(Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public object ReadObject(Type targetType, string path, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }

        public object ReadObject(Type targetType, string stringValue)
        {
            throw new System.NotImplementedException();
        }

        public object ReadObject(Type targetType, Stream stream)
        {
            throw new System.NotImplementedException();
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