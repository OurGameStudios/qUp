using System.Collections.Generic;

namespace Extensions {
    public static class HashSetExtensions {
        public static bool IsEmpty<T>(this HashSet<T> set) => set.Count == 0;
    }
}
