using Petecat.Utility;
using Petecat.Extension;
using Petecat.Collection;
using Petecat.Data.Formatters.Internal;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using Petecat.Data.Formatters.Internal.Json;

namespace Petecat.Data.Formatters
{
    public class JsonFormatter : AbstractObjectFormatter, IObjectFormatter
    {
        public override object ReadObject(Type targetType, Stream stream)
        {
            return JsonSerializer.GetSerializer(targetType).Deserialize(stream);
        }

        public override void WriteObject(object instance, Stream stream)
        {
            JsonSerializer.GetSerializer(instance.GetType()).Serialize(instance, stream);
        }
    }
}
