using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Petecat.Data.Formatters
{
    public class DataContractJsonFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public override object ReadObject(Type targetType, Stream stream)
        {
            return new DataContractJsonSerializer(targetType).ReadObject(stream);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(writer, instance);
            }
        }
    }
}
