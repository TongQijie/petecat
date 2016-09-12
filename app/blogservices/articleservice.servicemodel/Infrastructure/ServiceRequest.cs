using System;
using System.Linq;

using Petecat.Extension;

namespace ArticleService.ServiceModel.Infrastructure
{
    public class ServiceRequest
    {
        public string ActionName { get; set; }

        public KeyValuePair[] KeyValues { get; set; }

        public Paging Paging { get; set; }

        public string GetValue(string key)
        {
            if (KeyValues == null || KeyValues.Length == 0)
            {
                return null;
            }

            var keyValue = KeyValues.FirstOrDefault(x => !string.IsNullOrEmpty(x.Key) && x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (keyValue == null)
            {
                return null;
            }

            return keyValue.Value;
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            var value = GetValue(key);
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        public void SetValue(string key, string value)
        {
            if (KeyValues == null)
            {
                KeyValues = new KeyValuePair[0];
            }

            KeyValues.Append(new KeyValuePair(key, value));
        }
    }
}
