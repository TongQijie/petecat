using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Petecat.Data.Ini
{
    public class StringFormatter
    {
        private static List<string> _CommentIndicators = new string[] { ";", "#" }.ToList();

        public static T ReadObject<T>(string path, Encoding encoding, string elementKey)
        {
            return ConvertFromFile(path, encoding, elementKey).ReadObject<T>();
        }

        public static IElement ConvertFromFile(string path, Encoding encoding, string elementKey)
        {
            return ConvertFromFile(path, encoding).FirstOrDefault(x => x.Key.Equals(elementKey, StringComparison.OrdinalIgnoreCase));
        }

        public static IElement[] ConvertFromFile(string path, Encoding encoding)
        {
            using (var sr = new StreamReader(path, encoding))
            {
                return ConvertFromString(sr.ReadToEnd());
            }
        }

        public static IElement[] ConvertFromString(string iniString)
        {
            var elements = new List<IElement>();

            iniString = iniString.Trim();

            SectionElement sectionElement = null;

            var index = -1;
            while ((index = iniString.IndexOf(Environment.NewLine)) > 0 || !string.IsNullOrWhiteSpace(iniString))
            {
                var line = "";
                if (index > 0)
                {
                    line = iniString.Substring(0, index).Trim();
                    iniString = iniString.Remove(0, index).Trim();
                }
                else
                {
                    line = iniString.Trim();
                    iniString = "";
                }

                if (string.IsNullOrWhiteSpace(line) || _CommentIndicators.Exists(x => line.StartsWith(x)))
                {
                    continue;
                }

                if (Regex.IsMatch(line, @"^\x5B\w+\x5D$")) // [section]
                {
                    if (sectionElement != null)
                    {
                        elements.Add(sectionElement);
                    }

                    sectionElement = new SectionElement(line.Trim('[', ']'));
                }
                else if (Regex.IsMatch(line, @"^\w+=[^=]+$")) // key=value
                {
                    var kv = line.Split('=');
                    if (kv.Length != 2)
                    {
                        throw new FormatException();
                    }

                    var keyElement = new KeyElement(kv[0].Trim()) { Value = kv[1].Trim() };
                    if (sectionElement == null)
                    {
                        elements.Add(keyElement);
                    }
                    else
                    {
                        sectionElement.KeyElements.Add(new KeyElement(kv[0].Trim()) { Value = kv[1].Trim() });
                    }
                }
                else
                {
                    throw new FormatException();
                }
            }

            if (sectionElement != null)
            {
                elements.Add(sectionElement);
            }

            return elements.ToArray();
        }
    }
}
