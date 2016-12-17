using System;
using System.Collections;

namespace Petecat.Formatter.Json
{
    internal static class JsonUtility
    {
        public static JsonObjectType GetJsonObjectType(Type type)
        {
            if (type == typeof(object))
            {
                return JsonObjectType.Runtime;
            }
            else if (typeof(ICollection).IsAssignableFrom(type))
            {
                return JsonObjectType.Collection;
            }
            else if (type.IsClass && type != typeof(string))
            {
                return JsonObjectType.Dictionary;
            }
            else
            {
                return JsonObjectType.Value;
            }
        }
    }
}