using System;
using System.IO;

using Petecat.Formatter.Json;

namespace Petecat.Formatter
{
    public class JsonFormatter : FormatterBase, IFormatter
    {
        public bool OmitDefaultValueProperty { get; set; }

        public override object ReadObject(Type targetType, Stream stream)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(stream);
        }

        public object ReadObject(Type targetType, JsonObject jsonObject)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(jsonObject);
        }

        public object ReadObject<T>(JsonObject jsonObject)
        {
            return ReadObject(typeof(T), jsonObject);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream, OmitDefaultValueProperty);
        }
    }
}