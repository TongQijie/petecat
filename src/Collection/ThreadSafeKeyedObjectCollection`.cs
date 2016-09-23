using System.Collections.Generic;

namespace Petecat.Collection
{
    public class ThreadSafeKeyedObjectCollection<TKey, TValue> : KeyedObjectCollectionBase<TKey, TValue> where TValue : IKeyedObject<TKey>
    {
        public ThreadSafeKeyedObjectCollection()
            : base()
        {
        }

        public ThreadSafeKeyedObjectCollection(IEqualityComparer<TKey> equalityComparer)
            : base(equalityComparer)
        {
        }

        private object _ThreadSafeLocker = new object();

        public override TValue Add(TValue addedKeyedObject)
        {
            if (!ContainsKey(addedKeyedObject.Key))
            {
                lock (_ThreadSafeLocker)
                {
                    if (!ContainsKey(addedKeyedObject.Key))
                    {
                        return base.Add(addedKeyedObject);
                    }
                }
            }

            return default(TValue);
        }

        public override TValue Remove(TValue removedKeyedObject)
        {
            if (ContainsKey(removedKeyedObject.Key))
            {
                lock (_ThreadSafeLocker)
                {
                    if (ContainsKey(removedKeyedObject.Key))
                    {
                        return base.Remove(removedKeyedObject);
                    }
                }
            }

            return default(TValue);
        }

        public void AddRange(ICollection<TValue> addedKeyedObjects)
        {
            foreach (var addedKeyedObject in addedKeyedObjects)
            {
                this.Add(addedKeyedObject);
            }
        }

        public void RemoveAll()
        {
            foreach (var value in base.Values)
            {
                this.Remove(value);
            }
        }
    }
}