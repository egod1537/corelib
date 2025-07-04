using System;
using System.Collections.Generic;

namespace Corelib.Utils
{
    /// <summary>
    /// Serializable dictionary using enum names as keys so changes in enum order
    /// or values do not invalidate serialized data.
    /// </summary>
    [Serializable]
    public class StableEnumDictionary<TKey, TValue> : SerializableDictionary<string, TValue>
        where TKey : Enum
    {
        private static string KeyToString(TKey key) => Enum.GetName(typeof(TKey), key);

        public TValue this[TKey key]
        {
            get => base[KeyToString(key)];
            set => base[KeyToString(key)] = value;
        }

        public bool ContainsKey(TKey key) => base.ContainsKey(KeyToString(key));

        public void Add(TKey key, TValue value) => base.Add(KeyToString(key), value);

        public bool Remove(TKey key) => base.Remove(KeyToString(key));

        public bool TryGetValue(TKey key, out TValue value) => base.TryGetValue(KeyToString(key), out value);

        /// <summary>
        /// Enumerates all key value pairs using enum keys.
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, TValue>> EnumPairs
        {
            get
            {
                foreach (var kv in this)
                {
                    if (Enum.TryParse(typeof(TKey), kv.Key, out var parsed))
                        yield return new KeyValuePair<TKey, TValue>((TKey)parsed, kv.Value);
                }
            }
        }
    }
}
