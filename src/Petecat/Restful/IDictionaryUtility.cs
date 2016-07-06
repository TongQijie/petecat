using System;
using System.Collections.Generic;

namespace Petecat.Restful
{
    /// <summary>
    /// Dictionary utility interface.
    /// </summary>
    public interface IDictionaryUtility
    {
        /// <summary>
        /// Adds a key/value pair to the generic dictionary if the key does not already exist, or updates a key/value pair in the generic dictionary if the key already exists.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="dictionary">The dictionary which will be add or update.</param>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
        /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
        /// <returns>The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
        /// <exception cref="T:System.ArgumentNullException">Key is a null reference (Nothing in Visual Basic).-or-addValueFactory is a null reference (Nothing in Visual Basic).-or-updateValueFactory is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements, System.Int32.MaxValue.</exception>
        TValue AddOrUpdate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory);

        /// <summary>
        /// Adds a key/value pair to the generic dictionary if the key does not already exist, or updates a key/value pair in the generic dictionary if the key already exists.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="dictionary">The dictionary which will be add or update.</param>
        /// <param name="key">The key to be added or whose value should be updated.</param>
        /// <param name="addValue">The value to be added for an absent key.</param>
        /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
        /// <returns>The new value for the key. This will be either be addValue (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
        /// <exception cref="T:System.ArgumentNullException">Key is a null reference (Nothing in Visual Basic).-or-updateValueFactory is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.OverflowException">The dictionary already contains the maximum number of elements, System.Int32.MaxValue.</exception>
        TValue AddOrUpdate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);

        /// <summary>
        /// Union first dictionary and second dictionary to a new dictionary. The exist item in first dictionary will be replaced with second.
        /// </summary>
        /// <typeparam name="TKey">Type of key.</typeparam>
        /// <typeparam name="TValue">Type of value.</typeparam>
        /// <param name="firstDictionary">The first dictionary.</param>
        /// <param name="secondDictionary">The second dictionary.</param>
        /// <returns>Unioned new dictionary.</returns>
        IDictionary<TKey, TValue> UnionDictionary<TKey, TValue>(IDictionary<TKey, TValue> firstDictionary, IDictionary<TKey, TValue> secondDictionary);
    }
}
