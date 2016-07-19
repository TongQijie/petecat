using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Petecat.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// . -> current directory
        /// .. --> last directory
        /// </summary>
        public static string FullPath(this string stringValue)
        {
            var fields = stringValue.Split('/');
            if (fields.Length == 0)
            {
                throw new Exception("empty path string.");
            }

            var pathStack = new List<string>();

            if (fields[0].Equals(".", StringComparison.OrdinalIgnoreCase) || fields[0].Equals("..", StringComparison.OrdinalIgnoreCase))
            {
                pathStack.AddRange(AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/').SplitByChar('/'));
            }

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Equals(".", StringComparison.OrdinalIgnoreCase))
                {
                }
                else if (fields[i].Equals("..", StringComparison.OrdinalIgnoreCase))
                {
                    if (pathStack.Count == 0)
                    {
                        throw new Exception("path is invalid.");
                    }

                    pathStack.RemoveAt(pathStack.Count - 1);
                }
                else
                {
                    pathStack.Add(fields[i]);
                }
            }

            return string.Join("/", pathStack.ToArray());
        }

        public static string[] SplitByChar(this string stringValue, char seperator)
        {
            return stringValue.Split(seperator).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();
        }
    }
}
