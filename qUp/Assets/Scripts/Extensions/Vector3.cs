using UnityEngine;

namespace Extensions {
    static class Vector3Extensions {
        public enum Vector3Axis {
            X, Y, Z
        }

        public static Vector3 ClampAxis(this Vector3 vector3, Vector3Axis axis, float min, float max) {
            if (axis == Vector3Axis.X) {
                vector3.x = Mathf.Clamp(vector3.x, min, max);
            } else if (axis == Vector3Axis.Y) {
                vector3.y = Mathf.Clamp(vector3.y, min, max);
            } else {
                vector3.z = Mathf.Clamp(vector3.z, min, max);
            }

            return vector3;
        }

        public static Vector2 ToVectro2XZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);

        public static Vector3 AddX(this Vector3 vector3, float x) {
            return vector3 + new Vector3(x, 0, 0);
        }

        public static Vector3 AddY(this Vector3 vector3, float y) {
            return vector3 + new Vector3(0, y, 0);
        }

        public static Vector3 AddZ(this Vector3 vector3, float z) {
            return vector3 + new Vector3(0, 0, z);
        }

        public static Vector3 AddVector2(this Vector3 vector3, Vector2 vector2) {
            return vector3 + new Vector3(vector2.x, 0, vector2.y);
        }

        public static Vector3 Clone(this Vector3 vector3) {
            return new Vector3(vector3.x, vector3.y, vector3.z);
        }
    }
}
