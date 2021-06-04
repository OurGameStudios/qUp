using System;

namespace Extensions {
    internal static class StringExtensions {
        public static string Format(this string text, params object[] args) => String.Format(text, args);
    }
}
