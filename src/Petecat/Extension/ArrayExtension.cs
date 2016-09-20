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

        public static T FirstOrDefault<T>(this T[] data, Func<T, bool> predicate)
        {
            return data.ToList().FirstOrDefault(predicate);
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

        public static int IndexOf<T>(this T[] data, T flag, int startIndex, int count)
        {
            return data.ToList().IndexOf(flag, startIndex, count);
        }

        public static int IndexOfEx<T>(this T[] data, T[] findBytes, int startIndex, int count)
        {
            for (int i = startIndex; i < data.Length && i < (startIndex + count); i++)
            {
                var k = i;
                var foundBytes = true;
                for (int j = 0; j < findBytes.Length && k < data.Length && k < (startIndex + count); j++, k++)
                {
                    if (!data[k].Equals(findBytes[j]))
                    {
                        foundBytes = false;
                        break;
                    }
                }

                if (foundBytes)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int IndexOfEx<T>(this T[] data, T[][] findBytesSet, int startIndex, int count, out int index)
        {
            for (int i = startIndex; i < data.Length && i < (startIndex + count); i++)
            {
                for (int h = 0; h < findBytesSet.Length; h++)
                {
                    var foundBytes = true;
                    var k = i;
                    for (int j = 0; j < findBytesSet[h].Length && k < data.Length && k < (startIndex + count); j++, k++)
                    {
                        if (!data[k].Equals(findBytesSet[h][j]))
                        {
                            foundBytes = false;
                            break;
                        }
                    }

                    if (foundBytes)
                    {
                        index = h;
                        return i;
                    }
                }
            }

            index = -1;
            return -1;
        }

        public static T[] SubArray<T>(this T[] data, int startIndex)
        {
            return data.ToList().Skip(startIndex).ToArray();
        }

        public static T[] SubArray<T>(this T[] data, int startIndex, int count)
        {
            return data.ToList().Skip(startIndex).Take(count).ToArray();
        }
    }
}
