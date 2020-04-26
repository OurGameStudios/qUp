using UnityEngine;

namespace Extensions {
    static class TransformExtenstions {
        public static void RotateAroundClamped(this Transform transform, Vector3 pivot, float angle, float minAngle,
                                               float maxAngle) {
            var rotation = transform.rotation.eulerAngles;
            var clampedAngle = Mathf.Clamp(angle + rotation.x, minAngle, maxAngle);
            var deltaAngle = clampedAngle - rotation.x;
            transform.RotateAround(pivot, transform.right, deltaAngle);
        }

        public static void TranslateClamped(this Transform transform, Vector3 translation, Vector3 minPosition,
                                            Vector3 maxPosition, Space relativeTo) {
            var position = transform.position;
            var clampedX = Mathf.Clamp(position.x + translation.x, minPosition.x, maxPosition.x);
            var clampedY = Mathf.Clamp(position.y + translation.y, minPosition.y, maxPosition.y);
            var clampedZ = Mathf.Clamp(position.z + translation.z, minPosition.z, maxPosition.z);
            var deltaVector = new Vector3(clampedX, clampedY, clampedZ) - translation;
            transform.Translate(deltaVector, relativeTo);
        }
        
        public static void TranslateClamped(this Transform transform, Vector3 translation, Vector2 minPosition,
                                            Vector2 maxPosition, Space relativeTo) {
            var position = transform.position;
            var clampedX = Mathf.Clamp(position.x + translation.x, minPosition.x, maxPosition.x);
            var clampedZ = Mathf.Clamp(position.z + translation.z, minPosition.y, maxPosition.y);
            var deltaVector = new Vector3(clampedX - position.x, position.y, clampedZ - position.z);
            transform.Translate(deltaVector, relativeTo);
        }
    }
}
