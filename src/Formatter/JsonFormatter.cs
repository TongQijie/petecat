using System;
using System.IO;

using Petecat.Formatter.Json;
using Petecat.DependencyInjection.Attribute;

namespace Petecat.Formatter
{
    [DependencyInjectable(Inference = typeof(IJsonFormatter), Singleton = true)]
    public class JsonFormatter : FormatterBase, IJsonFormatter
    {
        public bool OmitDefaultValue { get; set; }

        public JsonFormatter()
        {
            OmitDefaultValue = true;
        }

        public override object ReadObject(Type targetType, Stream stream)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(stream);
        }

        public object ReadObject(Type targetType, JsonObject jsonObject)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(jsonObject);
        }

        public T ReadObject<T>(JsonObject jsonObject)
        {
            return (T)ReadObject(typeof(T), jsonObject);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream, OmitDefaultValue);
        }
    }
}