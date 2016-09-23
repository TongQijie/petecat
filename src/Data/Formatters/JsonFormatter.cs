using System;
using System.IO;

using Petecat.Data.Formatters.Internal.Json;
using Petecat.Logging;

namespace Petecat.Data.Formatters
{
    public class JsonFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public bool OmitDefaultValueProperty { get; set; }

        public override object ReadObject(Type targetType, Stream stream)
        {
            var now = DateTime.Now;
            var obj = JsonSerializer.GetSerializer(targetType).Deserialize(stream);
            LoggerManager.GetLogger().LogEvent("JsonFormatter", LoggerLevel.Info, string.Format("deserialize [{0}] cost: {1}", targetType.FullName, (DateTime.Now - now).TotalMilliseconds));
            return obj;
        }

        public override void WriteObject(object instance, Stream stream)
        {
            var now = DateTime.Now;
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream, OmitDefaultValueProperty);
            LoggerManager.GetLogger().LogEvent("JsonFormatter", LoggerLevel.Info, string.Format("serialize [{0}] cost: {1}", instance.GetType().FullName, (DateTime.Now - now).TotalMilliseconds));
        }
    }
}
