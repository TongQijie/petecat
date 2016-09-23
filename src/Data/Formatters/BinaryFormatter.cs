using System;
using System.IO;
using System.Text;

namespace Petecat.Data.Formatters
{
    public class BinaryFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public override object ReadObject(Type targetType, Stream stream)
        {
            return BinarySerializer.Deserialize(targetType, stream);
        }

        public override object ReadObject(Type targetType, string stringValue, Encoding encoding)
        {
            var byteValues = Convert.FromBase64String(stringValue);
            using (var inputStream = new MemoryStream(byteValues))
            {
                return BinarySerializer.Deserialize(targetType, inputStream);
            }
        }

        public override void WriteObject(object instance, Stream stream)
        {
            BinarySerializer.Serialize(instance, stream);
        }

        public override string WriteString(object instance, Encoding encoding)
        {
            return Convert.ToBase64String(WriteBytes(instance));
        }
    }
}