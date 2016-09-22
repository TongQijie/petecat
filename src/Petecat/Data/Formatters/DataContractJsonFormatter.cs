using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Petecat.Data.Formatters
{
    public class DataContractJsonFormatter : IObjectFormatter
    {
        public object ReadObject(Type targetType, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(targetType);
            return serializer.ReadObject(stream);
        }

        public object ReadObject(Type targetType, string stringValue)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(stringValue)))
            {
                return ReadObject(targetType, memoryStream);
            }
        }

        public object ReadObject(Type targetType, string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                return ReadObject(targetType, streamReader.ReadToEnd());
            }
        }

        public object ReadObject(Type targetType, byte[] byteValues, int offset, int count)
        {
            return ReadObject(targetType, Encoding.UTF8.GetString(byteValues, offset, count));
        }

        public T ReadObject<T>(Stream stream)
        {
            return (T)ReadObject(typeof(T), stream);
        }

        public T ReadObject<T>(string stringValue)
        {
            return (T)ReadObject(typeof(T), stringValue);
        }

        public T ReadObject<T>(string path, Encoding encoding)
        {
            return (T)ReadObject(typeof(T), path, encoding);
        }

        public T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            return (T)ReadObject<T>(Encoding.UTF8.GetString(byteValues, offset, count));
        }

        public void WriteObject(object instance, Stream stream)
        {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(writer, instance);
            }
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            using (var streamWriter = new StreamWriter(path, false, encoding))
            {
                streamWriter.Write(WriteString(instance));
            }
        }

        public byte[] WriteBytes(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public string WriteString(object instance)
        {
            return Encoding.UTF8.GetString(WriteBytes(instance));
        }
    }
}
