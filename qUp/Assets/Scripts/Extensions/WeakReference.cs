using System;
using JetBrains.Annotations;

namespace Extensions {
    static class WeakReference {
        [CanBeNull]
        public static V GetOrNull<V>(this WeakReference<V> weakReference) where V : class {
            weakReference.TryGetTarget(out var item);
            return item;
        }

        public static bool HasValue<V>(this WeakReference<V> weakReference) where V : class {
            return weakReference.TryGetTarget(out var target);
        }
    }
}