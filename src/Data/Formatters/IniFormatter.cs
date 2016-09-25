using System;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class IniFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public override object ReadObject(Type targetType, string path, Encoding encoding)
        {
            return IniSerializer.Deserialize(targetType, path, encoding);
        }

        public override T ReadObject<T>(string path, Encoding encoding)
        {
            return (T)ReadObject(typeof(T), path, encoding);
        }
    }
}
