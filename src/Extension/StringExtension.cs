using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Petecat.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Gets the full path relative to current base directory
        /// </summary>
        /// <param name="stringValue">relative path value</param>
        /// <returns>return full path with slash '/' as path seperator</returns>
        public static string FullPath(this string stringValue)
        {
            return stringValue.FullPath(null);
        }

        /// <summary>
        /// Gets the full path relative to the specified path
        /// </summary>
        /// <param name="stringValue">relative path value</param>
        /// <param name="relativePath">specified path value</param>
        /// <returns>return full path with slash '/' as path seperator</returns>
        public static string FullPath(this string stringValue, string specifiedPath)
        {
            var fields = stringValue.Replace('\\', '/').SplitByChar('/');
            if (fields.Length == 0)
            {
                return string.Empty;
            }

            var pathParts = new List<string>();

            if (fields[0].Equals(".", StringComparison.OrdinalIgnoreCase) || fields[0].Equals("..", StringComparison.OrdinalIgnoreCase))
            {
                if (specifiedPath.HasValue())
                {
                    pathParts.AddRange(specifiedPath.Replace('\\', '/').SplitByChar('/'));
                }
                else
                {
                    pathParts.AddRange(AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/').SplitByChar('/'));
                }
            }

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Equals(".", StringComparison.OrdinalIgnoreCase))
                {
                }
                else if (fields[i].Equals("..", StringComparison.OrdinalIgnoreCase))
                {
                    if (pathParts.Count == 0)
                    {
                        throw new Exception(string.Format("path '{0}' and '{1}' is not valid.", stringValue, specifiedPath ?? string.Empty));
                    }

                    pathParts.RemoveAt(pathParts.Count - 1);
                }
                else
                {
                    pathParts.Add(fields[i]);
                }
            }

            var fullPath = string.Join("/", pathParts.ToArray());
            if (Regex.IsMatch(fullPath, "^[A-Z]\x3A\x2F", RegexOptions.IgnoreCase))
            {
                // for Windows
                return fullPath;
            }
            else
            {
                // for linux
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

        public static bool IsFile(this string stringValue)
        {
            return File.Exists(stringValue);
        }

        public static bool IsFolder(this string stringValue)
        {
            return Directory.Exists(stringValue);
        }
    }
}
