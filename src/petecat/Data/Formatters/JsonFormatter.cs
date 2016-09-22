using Petecat.Collection;
using Petecat.Data.Formatters.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class JsonFormatter : IObjectFormatter
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
            throw new System.NotImplementedException();
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }

        public void WriteObject(object instance, Stream stream)
        {
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream);
        }

        public string WriteString(object instance)
        {
            return Encoding.UTF8.GetString(WriteBytes(instance));
        }

        public byte[] WriteBytes(object instance)
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteObject(instance, memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
