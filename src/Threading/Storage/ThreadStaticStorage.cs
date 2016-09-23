using System;
using System.Collections;

namespace Petecat.Threading.Storage
{
    public class ThreadStaticStorage : IThreadStorage
    {
        [ThreadStatic]
        private static Hashtable _Data = null;

        private static Hashtable Data { get { return _Data ?? (_Data = new Hashtable()); } }

        public T Get<T>(string name)
        {
            return (T)Data[name];
        }

        public T Get<T>(string name, T defaultValue)
        {
            if (Data.ContainsKey(name))
            {
                return (T)Data[name];
            }
            else
            {
                return defaultValue;
            }
        }

        public void Set(string name, object value)
        {
            Data[name] = value;
        }

        public void Remove(string name)
        {
            Data.Remove(name);
        }
    }
}
