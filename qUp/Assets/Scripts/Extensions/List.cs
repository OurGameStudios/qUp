using System;
using System.Collections.Generic;

namespace Extensions {
    static class ListExtensions {

        public static bool IsEmpty<T>(this List<T> list) => list.Count == 0;
        public static List<T> RemoveLast<T>(this List<T> list) => list.Also((it) => it.RemoveAt(it.Count - 1));

        public static void Repopulate<T>(this List<T> list, IEnumerable<T> range) {
            list.Clear();
            list.AddRange(range);
        }
        
        public static void Repopulate<T>(this List<T> list, T value) {
            list.Clear();
            list.Add(value);
        }

        public static T GetRandom<T>(this List<T> list) where T : class {
            return list.Count == 0 ? null : list[new Random().Next(0, list.Count)];
        }
        
        public static T GetRandom<T>(this List<T> list, int seed) where T : class {
            return list.Count == 0 ? null : list[new Random(seed).Next(0, list.Count)];
        }
    }
}
