using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petecat.Data.Ini
{
    public class SectionElement : IElement, Collection.IKeyedObject<string>
    {
        public SectionElement(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        private Collection.ThreadSafeKeyedObjectCollection<string, KeyElement> _KeyElements = null;

        public Collection.ThreadSafeKeyedObjectCollection<string, KeyElement> KeyElements
        {
            get { return _KeyElements ?? (_KeyElements = new Collection.ThreadSafeKeyedObjectCollection<string, KeyElement>()); }
        }

        public T ReadObject<T>()
        {
            var instance = Activator.CreateInstance<T>();

            var targetType = typeof(T);
            foreach (var propertyInfo in targetType.GetProperties())
            {
                var iniElementAttribute = propertyInfo.GetCustomAttributes(false).OfType<IniKeyAttribute>().FirstOrDefault();
                if (iniElementAttribute == null)
                {
                    continue;
                }

                var elementName = propertyInfo.Name;
                if (!string.IsNullOrWhiteSpace(iniElementAttribute.ElementName))
                {
                    elementName = iniElementAttribute.ElementName;
                }

                if (KeyElements.ContainsKey(elementName))
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(KeyElements[elementName].Value, propertyInfo.PropertyType));
                }
                else if (iniElementAttribute.DefaultValue != null)
                {
                    propertyInfo.SetValue(instance, iniElementAttribute.DefaultValue);
                }
            }

            return instance;
        }

        public void WriteObject(object instance)
        {
            var sourceType = instance.GetType();
            foreach (var propertyInfo in sourceType.GetProperties())
            {
                var iniElementAttribute = propertyInfo.GetCustomAttributes(false).OfType<IniKeyAttribute>().FirstOrDefault();
                if (iniElementAttribute == null)
                {
                    continue;
                }

                var elementName = propertyInfo.Name;
                if (!string.IsNullOrWhiteSpace(iniElementAttribute.ElementName))
                {
                    elementName = iniElementAttribute.ElementName;
                }

                var keyElement = new KeyElement(elementName);
                keyElement.WriteObject(propertyInfo.GetValue(instance));

                KeyElements.Add(keyElement);
            }
        }

        public string Format()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("[{0}]", Key));
            foreach (var keyElement in KeyElements.Values)
            {
                stringBuilder.AppendLine(keyElement.Format());
            }

            return stringBuilder.ToString();
        }
    }
}
