using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public interface IObjectFormatter
    {
        object ReadObject(Type targetType, Stream stream);

        object ReadObject(Type targetType, string path);

        object ReadObject(Type targetType, string stringValue, Encoding encoding);

        object ReadObject(Type targetType, byte[] byteValues, int offset, int count);

        T ReadObject<T>(Stream stream);

        T ReadObject<T>(string path);

        T ReadObject<T>(string stringValue, Encoding encoding);

        T ReadObject<T>(byte[] byteValues, int offset, int count);

        void WriteObject(object instance, Stream stream);

        void WriteObject(object instance, string path);

        string WriteString(object instance, Encoding encoding);

        byte[] WriteBytes(object instance);
    }
}
