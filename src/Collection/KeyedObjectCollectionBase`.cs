using System;
using System.Collections.Generic;
using System.Linq;

namespace Petecat.Collection
{
    public class KeyedObjectCollectionBase<TKey, TValue> : IKeyedObjectCollection<TKey, TValue> where TValue : IKeyedObject<TKey>
    {
        private List<TValue> _Values = null;

        private IEqualityComparer<TKey> _EqualityComparer = null;

        public KeyedObjectCollectionBase()
        {
            _Values = new List<TValue>();

            if (typeof(TKey) == typeof(string))
            {
                _EqualityComparer = new CaseInsensitiveStringEqualityComparer() as IEqualityComparer<TKey>;
            }
            else
            {
                _EqualityComparer = new DefaultKeyedObjectEqualityComparer<TKey>();
            }
        }

        public KeyedObjectCollectionBase(IEqualityComparer<TKey> equalityComparer) : this()
        {
            if (equalityComparer != null)
            {
                _EqualityComparer = equalityComparer;
            }
        }

        /// <summary>
        /// 根据键获取值，若值不存在则抛出NotExistsValueException
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                var value = _Values.FirstOrDefault(x => _EqualityComparer.Equals(x.Key, key));
                if (value == null)
                {
                    throw new KeyNotFoundException(string.Format("键[{0}]不存在。", key));
                }
                else
                {
                    return value;
                }
            }
        }

        /// <summary>
        /// 根据键获取值，若值不存在则返回默认值
        /// </summary>
        public virtual TValue Get(TKey key, TValue defaultValue)
        {
            var value = _Values.FirstOrDefault(x => _EqualityComparer.Equals(x.Key, key));
            if (value == null)
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 根据键获取值并且检测是否匹配指定的表达式，若值不存在或不匹配则返回默认值
        /// </summary>
        public virtual TValue Get(TKey key, Predicate<TValue> match, TValue defaultValue)
        {
            var value = _Values.FirstOrDefault(x => _EqualityComparer.Equals(x.Key, key));
            if (value == null || !match(value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 检查是否包含指定的键
        /// </summary>
        public bool ContainsKey(TKey key)
        {
            return _Values.Exists(x => _EqualityComparer.Equals(x.Key, key));
        }

        /// <summary>
        /// 返回包含的键集合
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                return _Values.Select(x => x.Key);
            }
        }

        /// <summary>
        /// 返回包含的值集合
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                return _Values;
            }
        }

        /// <summary>
        /// 添加新的值，若添加成功则返回添加的值
        /// </summary>
        public virtual TValue Add(TValue addedKeyedObject)
        {
            if (!ContainsKey(addedKeyedObject.Key))
            {
                _Values.Add(addedKeyedObject);
                return addedKeyedObject;
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 删除新的值，若删除成功则返回删除的值
        /// </summary>
        public virtual TValue Remove(TValue removedKeyedObject)
        {
            if (ContainsKey(removedKeyedObject.Key))
            {
                _Values.Remove(removedKeyedObject);
                return removedKeyedObject;
            }
            else
            {
                return default(TValue);
            }
        }
    }
}
