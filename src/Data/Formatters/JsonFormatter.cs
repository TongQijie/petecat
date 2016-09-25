using System;
using System.IO;

using Petecat.Data.Formatters.Internal.Json;

namespace Petecat.Data.Formatters
{
    public class JsonFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public bool OmitDefaultValueProperty { get; set; }

        public override object ReadObject(Type targetType, Stream stream)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(stream);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream, OmitDefaultValueProperty);
        }
    }
}
