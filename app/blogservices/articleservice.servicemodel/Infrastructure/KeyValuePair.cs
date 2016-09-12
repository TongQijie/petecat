namespace ArticleService.ServiceModel.Infrastructure
{
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

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
