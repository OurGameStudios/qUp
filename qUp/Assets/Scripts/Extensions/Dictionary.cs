using System.Collections.Generic;

namespace Extensions {
    public static class DictionaryExtensions {
        public static V GetOrNull<K, V>(this Dictionary<K, V> dictionary, K key) where V : class {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }
    }
}
