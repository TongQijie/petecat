using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Petecat.Data.Formatters
{
    internal class IniSerializer
    {
        public static object Deserialize(Type targetType, string path, Encoding encoding)
        {
            using (var streamReader = new StreamReader(path, encoding))
            {
                string line = null;
                var instance = InternalDeserialize(targetType, streamReader, out line);

                while (line != null)
                {
                    var section = line.TrimStart('[').TrimEnd(']').Trim();

                    var foundProperty = false;
                    foreach (var propertyInfo in targetType.GetProperties())
                    {
                        Attributes.IniSerializableAttribute attribute;
                        if (!Utility.ReflectionUtility.TryGetCustomAttribute<Attributes.IniSerializableAttribute>(propertyInfo,
                            x => !x.NonSerialized && x.Name.Equals(section, StringComparison.OrdinalIgnoreCase), out attribute))
                        {
                            continue;
                        }

                        foundProperty = true;
                        propertyInfo.SetValue(instance, InternalDeserialize(propertyInfo.PropertyType, streamReader, out line));
                        break;
                    }

                    if (!foundProperty)
                    {
                        throw new FormatException(string.Format("section '{0}' does not exist.", section));
                    }
                }

                return instance;
            }
        }

        private static object InternalDeserialize(Type targetType, StreamReader streamReader, out string lastLine)
        {
            var instance = Activator.CreateInstance(targetType);

            string line = null;
            while ((line = ReadLine(streamReader)) != null)
            {
                if (Regex.IsMatch(line, @"^\x5B\w+\x5D$")) // [section]
                {
                    break;
                }
                else if (Regex.IsMatch(line, @"^\w+\s?=[^=]+$")) // key=value
                {
                    var kv = line.Split('=');
                    var stringKey = kv[0].Trim();
                    var stringValue = kv[1].Trim();

                    foreach (var propertyInfo in targetType.GetProperties())
                    {
                        Attributes.IniSerializableAttribute attribute;
                        if (!Utility.ReflectionUtility.TryGetCustomAttribute<Attributes.IniSerializableAttribute>(propertyInfo, 
                            x => !x.NonSerialized && x.Name.Equals(stringKey, StringComparison.OrdinalIgnoreCase), out attribute))
                        {
                            continue;
                        }

                        try
                        {
                            var value = Convert.ChangeType(stringValue, propertyInfo.PropertyType);
                            propertyInfo.SetValue(instance, value);
                            break;
                        }
                        catch (Exception e)
                        {
                            throw new FormatException(string.Format("format error: '{0}'", line), e);
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("illegal text '{0}' in ini config.", line));
                }
            }

            lastLine = line;
            return instance;
        }

        public static string ReadLine(StreamReader streamReader)
        {
            string line = null;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                {
                    continue;
                }

                return line.Trim();
            }

            return null;
        }
    }
}
