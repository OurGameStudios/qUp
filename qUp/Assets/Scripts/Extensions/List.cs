using System;
using System.Collections.Generic;

namespace Extensions {
    internal static class ListExtensions {

        public static bool IsEmpty<T>(this List<T> list) => list.Count == 0;

        public static bool IsNotEmpty<T>(this List<T> list) => list.Count != 0;

        public static void RemoveFirst<T>(this List<T> list) {
            if (list.IsEmpty()) return;
            list.RemoveAt(0);
        }

        public static T GetRandom<T>(this List<T> list) where T : class {
            return list.Count == 0 ? null : list[new Random().Next(0, list.Count)];
        }
    }
}
