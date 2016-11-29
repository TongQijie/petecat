using System;

using Petecat.Formatter.Json;

namespace Petecat.Formatter
{
    public interface IJsonFormatter : IFormatter
    {
        bool OmitDefaultValue { get; set; }

        object ReadObject(Type targetType, JsonObject jsonObject);

        T ReadObject<T>(JsonObject jsonObject);
    }
}