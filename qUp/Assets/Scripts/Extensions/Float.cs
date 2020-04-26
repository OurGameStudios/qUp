using System.Linq;
using UnityEngine;

namespace Extensions {
    static class FloatExtensions {
        public static float SumScaledValue(this float value, params float[] multipliers) {
            if (multipliers.Length == 1) {
                return value * (1 + multipliers[0]);
            }

            return value * multipliers.Sum();
        }

        public static float Sqrt(this float value) => Mathf.Sqrt(value);

        public static float Sqr(this float value) => Mathf.Pow(value, 2);

        public static bool IsBetween(this float value, float min, float max) => !(value < min) && !(value > max);
        }
}
