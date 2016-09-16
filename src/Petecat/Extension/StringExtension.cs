using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            var fields = stringValue.SplitByChar('/');
            if (fields.Length == 0)
            {
                return string.Empty;
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

            var fullPath = string.Join("/", pathStack.ToArray());

            if (Regex.IsMatch(fullPath, "^[A-Z]\x3A\x2F", RegexOptions.IgnoreCase))
            {
                return fullPath;
            }
            else
            {
                return "/" + fullPath;
            }
        }

        public static string[] SplitByChar(this string stringValue, char seperator)
        {
            if (stringValue == null)
            {
                return null;
            }

            return stringValue.Split(seperator).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();
        }

        public static bool HasValue(this string stringValue)
        {
            return !string.IsNullOrEmpty(stringValue) && !string.IsNullOrWhiteSpace(stringValue);
        }

        public static bool IsDateTime(this string stringValue)
        {
            DateTime datetime;
            return DateTime.TryParse(stringValue, out datetime);
        }
    }
}
