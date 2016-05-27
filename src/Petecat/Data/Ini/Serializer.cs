using System;
using System.Collections.Generic;

namespace Petecat.Data.Ini
{
    public class Serializer
    {
        public static T ReadObject<T>(string iniString)
        {
            return default(T);
        }

        public static string WriteObject(object instance)
        {
            return null;
        }
    }
}