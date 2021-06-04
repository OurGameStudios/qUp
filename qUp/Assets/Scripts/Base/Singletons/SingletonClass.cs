using System;

namespace Base.Singletons {
    /// <summary>
    /// Should only be accessed from short-lived objects. Long-lived objects should use the interface through the Loader
    /// </summary>
    /// <typeparam name="T">Is the same as the class inheriting this</typeparam>
    public abstract class SingletonClass<T> : IDisposable where T : SingletonClass<T> {
        private static SingletonClass<T> instance;

        /// <summary>
        /// Should only be accessed from shot-lived objects
        /// </summary>
        public static T Instance => (T) instance;
        
        private event Action Unsubscribe;

        protected SingletonClass() {
            instance = this;
        }

        protected void AddToDispose(Action disposeAction) {
            Unsubscribe += disposeAction;
        }

        public void Dispose() {
            instance = null;
            Unsubscribe?.Invoke();
            Unsubscribe = null;
        }

    }
}
