using System.Collections.Generic;

namespace Extensions {
    public static class DictionaryExtensions {

        public static void AddOrUpdate<K, V>(this Dictionary<K, V> dictionary, K key, V value) {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            } else {
                dictionary.Add(key, value);
            }
        }
        
        public static V GetOrNull<K, V>(this Dictionary<K, V> dictionary, K key) where V : class {
            return dictionary.ContainsKey(key) ? dictionary[key] : null;
        }

        public static V GetOrElse<K, V>(this Dictionary<K, V> dictionary, K key, V elseValue) where V : class {
            return dictionary.ContainsKey(key) ? dictionary[key] : elseValue;
        }

        public static List<V> GetValues<K, V>(this Dictionary<K, V> dictionary, List<K> keys) where V : class =>
            keys.FindAll(dictionary.ContainsKey).ConvertAll(key => dictionary[key]);
    }
}
