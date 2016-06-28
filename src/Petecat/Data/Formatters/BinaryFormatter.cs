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
            throw new System.NotImplementedException();
        }

        public void WriteObject(object instance, string path, Encoding encoding)
        {
            throw new System.NotImplementedException();
        }

        public string WriteObject(object instance)
        {
            throw new System.NotImplementedException();
        }

        public void WriteObject(object instance, Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public string WriteString(object instance)
        {
            throw new System.NotImplementedException();
        }

        public byte[] WriteBytes(object instance)
        {
            throw new System.NotImplementedException();
        }
    }
}