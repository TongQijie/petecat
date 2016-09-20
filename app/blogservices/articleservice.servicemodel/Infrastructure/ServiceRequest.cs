using System;
using System.Linq;
using System.Runtime.Serialization;

using Petecat.Extension;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class ServiceRequest
    {
        [DataMember(Name = "actionName")]
        public string ActionName { get; set; }

        [DataMember(Name = "keyValues")]
        public KeyValuePair[] KeyValues { get; set; }

        [DataMember(Name = "paging")]
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
