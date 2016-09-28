using System;
using System.Linq;
using System.Collections.Generic;

namespace Petecat.Extension
{
    public static class ArrayExtension
    {
        public static bool HasValue<T>(this T[] data)
        {
            return data != null && data.Length > 0;
        }

        public static T[] Where<T>(this T[] data, Func<T, bool> predicate)
        {
            return data.ToList().Where(predicate).ToArray();
        }

        public static T FirstOrDefault<T>(this T[] data)
        {
            return data.ToList().FirstOrDefault();
        }

        public static T FirstOrDefault<T>(this T[] data, Func<T, bool> predicate)
        {
            return data.ToList().FirstOrDefault(predicate);
        }

        public static T[] Append<T>(this T[] data, T item)
        {
            var buf = new T[data.Length + 1];
            buf[buf.Length - 1] = item;
            Array.Copy(data, 0, buf, 0, data.Length);
            return buf;
        }

        public static T[] Append<T>(this T[] data, IEnumerable<T> items)
        {
            return data.Concat(items).ToArray();
        }

        public static T[] Append<T>(this T[] data, T[] items)
        {
            var buf = new T[data.Length + items.Length];
            Array.Copy(data, 0, buf, 0, data.Length);
            Array.Copy(items, 0, buf, data.Length, items.Length);
            return buf;
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

        public static int IndexOf<T>(this T[] data, T flag, int startIndex)
        {
            return data.ToList().IndexOf(flag, startIndex);
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
            return SubArray(data, startIndex, data.Length - startIndex);
        }

        public static T[] SubArray<T>(this T[] data, int startIndex, int count)
        {
            var buf = new T[count];
            Array.Copy(data, startIndex, buf, 0, buf.Length);
            return buf;
        }
    }
}
