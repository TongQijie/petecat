using System;
using System.Collections.Generic;
namespace Petecat.Restful
{
    public static class DictionaryUtilityExtensions
    {
        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> me, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            return ECLibraryContainer.Get<IDictionaryUtility>().AddOrUpdate<TKey, TValue>(me, key, addValueFactory, updateValueFactory);
        }

        public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> me, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            return ECLibraryContainer.Get<IDictionaryUtility>().AddOrUpdate<TKey, TValue>(me, key, addValue, updateValueFactory);
        }

        public static IDictionary<TKey, TValue> UnionDictionary<TKey, TValue>(this IDictionary<TKey, TValue> me, IDictionary<TKey, TValue> another)
        {
            return ECLibraryContainer.Get<IDictionaryUtility>().UnionDictionary<TKey, TValue>(me, another);
        }
    }
}
