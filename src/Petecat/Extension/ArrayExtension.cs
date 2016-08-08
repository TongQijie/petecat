using System;
using System.Collections.Generic;
using System.Linq;

namespace Petecat.Extension
{
    public static class ArrayExtension
    {
        public static T[] Where<T>(this T[] data, Func<T, bool> predicate)
        {
            return data.ToList().Where(predicate).ToArray();
        }

        public static T[] Append<T>(this T[] data, T item)
        {
            return data.Concat(new T[] { item }).ToArray();
        }

        public static T[] Append<T>(this T[] data, IEnumerable<T> items)
        {
            return data.Concat(items).ToArray();
        }

        public static bool Exists<T>(this T[] data, Func<T, bool> predicate)
        {
            return data.ToList().Exists(x => predicate(x));
        }

        public static T[] Remove<T>(this T[] data, Func<T, bool> predicate)
        {
            var d = new T[0];
            foreach (var item in data)
            {
                if (!predicate(item))
                {
                    d = d.Append(item);
                }
            }
            return d;
        }

        public static bool EqualsWith<T>(this T[] data, T[] anotherArray)
        {
            if (data == null || anotherArray == null || data.Length != anotherArray.Length)
            {
                return false;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (!data[i].Equals(anotherArray[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
