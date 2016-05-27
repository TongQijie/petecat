using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Petecat.Data.Ini
{
    public class StringFormatter
    {
        private static List<string> _CommnetIndicators = new string[] { ";", "#" }.ToList();

        public static IElement[] ConvertFromString(string iniString)
        {
            var elements = new List<IElement>();

            iniString = iniString.Trim();

            SectionElement sectionElement = null;

            var index = -1;
            while ((index = iniString.IndexOf(Environment.NewLine)) > 0)
            {
                var line = iniString.Substring(0, index).Trim();
                iniString = iniString.Remove(0, index);

                if (string.IsNullOrWhiteSpace(line) || _CommnetIndicators.Exists(x => line.StartsWith(x)))
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
