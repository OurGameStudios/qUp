using System;
using Base.Interfaces;
using Extensions;
using UnityEngine;

namespace Managers.GridManagers {
    public class LazyStaticGet<T> where T : class, IManager {
        private static WeakReference<T> lazy;

        public static T Get(Func<T> provideMethod) {
            if (lazy?.GetOrNull() == null) {
                var value = provideMethod.Invoke();
                lazy = new WeakReference<T>(value);
                Debug.Log("this is " + nameof(T));
            }

            return lazy?.GetOrNull();
        }
    }
}
