using System;

namespace Common {
    public class LazyWeakReference<T> : Lazy<WeakReference<T>> where T : class {
        public LazyWeakReference(Func<T> provider) : base(() => new WeakReference<T>(provider?.Invoke())) { }

        public T GetOrNull() {
            Value.TryGetTarget(out var item);
            return item;
        }
    }
}
