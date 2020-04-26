using System;

namespace Extensions {
    static class ObjectExtensions {
        public static T Also<T>(this T it, Action<T> action) {
            action.Invoke(it);
            return it;
        }

        public static T Run<T>(this T it, Func<T, T> func) => func.Invoke(it);

        public static void Let<T>(this T it, Action<T> action) => action.Invoke(it);

        public static bool IsNull<T>(this T it) => it == null;

        public static bool IsNotNull<T>(this T it) => it != null;

        public static bool IsDefault<T>(this T it) => it.Equals(default);

        public static bool IsNotDefault<T>(this T it) => !it.Equals(default);

        public static T nullIfDefault<T>(this T it) where T : class => it.IsDefault() ? null : it;

        public static T ReturnOrRunIfNull<T>(this T it, Func<T> func) => it.IsNotNull() ? it : func.Invoke();

        public static T ReturnOrRunIfDefault<T>(this T it, Func<T> func) => it.IsNotDefault() ? it : func.Invoke();
    }
}
