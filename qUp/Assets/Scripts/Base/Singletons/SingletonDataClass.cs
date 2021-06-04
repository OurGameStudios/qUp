using System;
using Base.Common;
using Base.Managers;

namespace Base.Singletons {
    /// <summary>
    /// Class for implementing Singleton pattern without auto initialization
    /// </summary>
    /// <typeparam name="T">Is the same as the class inheriting this</typeparam>
    /// <typeparam name="TData"></typeparam>
    public abstract class SingletonDataClass<T, TData> : BaseClassManager<TData>, IDisposable
        where TData : BaseData where T : SingletonDataClass<T, TData> {

        private static SingletonDataClass<T, TData> instance;
        
        public static T Instance => (T) instance;

        protected SingletonDataClass() {
            instance = this;
        }

        public void Dispose() {
            instance = null;
        }
    }
}
