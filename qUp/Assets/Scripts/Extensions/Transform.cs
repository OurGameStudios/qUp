using UnityEngine;

namespace Extensions {
    internal static class TransformExtensions {
        public static void RotateAroundClamped(this Transform transform, Vector3 pivot, float angle, float minAngle,
                                               float maxAngle) {
            var rotation = transform.rotation.eulerAngles;
            var clampedAngle = Mathf.Clamp(angle + rotation.x, minAngle, maxAngle);
            var deltaAngle = clampedAngle - rotation.x;
            transform.RotateAround(pivot, transform.right, deltaAngle);
        }
    }
}
