using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public interface IDataFormatter
    {
        T ReadObject<T>(string path, Encoding encoding);

        T ReadObject<T>(string stringValue);

        T ReadObject<T>(Stream stream);

        object ReadObject(Type targetType, string path, Encoding encoding);

        object ReadObject(Type targetType, string stringValue);

        object ReadObject(Type targetType, Stream stream);

        void WriteObject(object instance, string path, Encoding encoding);

        string WriteObject(object instance);

        void WriteObject(object instance, Stream stream);
    }
}
