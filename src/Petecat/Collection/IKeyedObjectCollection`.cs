using System;
using System.Collections.Generic;

namespace Petecat.Collection
{
    public interface IKeyedObjectCollection<TKey, TValue> where TValue : IKeyedObject<TKey>
    {
        /// <summary>
        /// 根据键获取值，若值不存在则抛出NotExistsValueException
        /// </summary>
        TValue this[TKey key] { get; }

        /// <summary>
        /// 根据键获取值，若值不存在则返回默认值
        /// </summary>
        TValue Get(TKey key, TValue defaultValue);

        /// <summary>
        /// 根据键获取值并且检测是否匹配指定的表达式，若值不存在或不匹配则返回默认值
        /// </summary>
        TValue Get(TKey key, Predicate<TValue> match, TValue defaultValue);

        /// <summary>
        /// 检查是否包含指定的键
        /// </summary>
        bool ContainsKey(TKey key);

        /// <summary>
        /// 返回包含的键集合
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// 返回包含的值集合
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// 添加新的值，若添加成功则返回添加的值
        /// </summary>
        TValue Add(TValue addedKeyedObject);

        /// <summary>
        /// 删除新的值，若删除成功则返回删除的值
        /// </summary>
        TValue Remove(TValue removedKeyedObject);
    }
}
