using System;
namespace Petecat.Data.Ini
{
    public class KeyElement : IElement, Collection.IKeyedObject<string>
    {
        public KeyElement(string key)
        {
            Key = key;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Format()
        {
            return string.Format("{0}={1}", Key, Value);
        }

        public T ReadObject<T>()
        {
            return (T)Convert.ChangeType(Value, typeof(T));
        }

        public void WriteObject(object instance)
        {
            Value = instance.ToString();
        }
    }
}
