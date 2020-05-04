using System.Collections.Generic;
using System.Linq;
using Common;

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

        public static IEnumerable<V> GetValues<K, V>(this Dictionary<K, V> dictionary, IEnumerable<K> keys) where V : class =>
            keys.Intersect(dictionary.Keys).Select(key => dictionary[key]);

        public static IEnumerable<KeyValuePair<K, V>> GetPairsFromKeys<K, V>(this Dictionary<K, V> dictionary, List<K> keys) =>
            dictionary.Where(pair => keys.Contains(pair.Key));

        public static object GetValues(this Dictionary<object, object> dictionary, GridCoords[] tileNeighbours) {
            throw new System.NotImplementedException();
        }
    }
}
