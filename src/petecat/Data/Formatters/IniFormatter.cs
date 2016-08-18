using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class IniFormatter : IObjectFormatter
    {
        public T ReadObject<T>(string path, Encoding encoding)
        {
            return (T)ReadObject(typeof(T), path, encoding);
        }

        public T ReadObject<T>(string stringValue)
        {
            throw new NotImplementedException();
        }

        public T ReadObject<T>(Stream stream)
        {
            throw new NotImplementedException();
        }

        public T ReadObject<T>(byte[] byteValues, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public object ReadObject(Type targetType, string path, Encoding encoding)
        {
            return IniSerializer.Deserialize(targetType, path, encoding);
        }

        public object ReadObject(Type targetType, string stringValue)
        {
            throw new NotImplementedException();
        }

        public object ReadObject(Type targetType, Stream stream)
        {
            throw new NotImplementedException();
        }

        public object ReadObject(Type targetType, byte[] byteValues, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public void WriteObject(object instance, Stream stream)
        {
            throw new NotImplementedException();
        }

        public string WriteString(object instance)
        {
            throw new NotImplementedException();
        }

        public byte[] WriteBytes(object instance)
        {
            throw new NotImplementedException();
        }
    }
}
