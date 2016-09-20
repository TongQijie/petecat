using System.Runtime.Serialization;

namespace ArticleService.ServiceModel.Infrastructure
{
    [DataContract]
    public class KeyValuePair
    {
        public KeyValuePair()
        {
        }

        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
