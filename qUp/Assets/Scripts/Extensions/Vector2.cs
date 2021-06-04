using UnityEngine;

namespace Extensions {
    internal static class Vector2Extensions {
        public static Vector3 ToVector3XZ(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);
    }
}
