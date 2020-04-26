using System.Collections.Generic;

namespace Extensions {
    public static class DictionaryExtensions {
        public static V GetOrNull<K, V>(this Dictionary<K, V> dictionary, K key) where V : class {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }

        public static V GetOrElse<K, V>(this Dictionary<K, V> dictionary, K key, V elseValue) where V : class {
            return dictionary.ContainsKey(key) ? dictionary[key] : elseValue;
        }

        public static K IfContains<K, V>(this Dictionary<K, V> dictionary, K key) where K : class =>
            dictionary.ContainsKey(key) ? key : null;

        public static List<V> GetValues<K, V>(this Dictionary<K, V> dictionary, List<K> keys) where V : class =>
            keys.FindAll(dictionary.ContainsKey).ConvertAll(key => dictionary[key]);
    }
}
