using UnityEngine;

namespace Extensions {
    internal static class FloatExtensions {

        public static float Sqrt(this float value) => Mathf.Sqrt(value);

        public static float Sqr(this float value) => Mathf.Pow(value, 2);
    }
}
