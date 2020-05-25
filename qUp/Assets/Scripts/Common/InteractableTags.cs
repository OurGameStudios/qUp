using JetBrains.Annotations;
using UnityEngine;

namespace Common {
    public static class InteractableTags {
        private const string UNIT_TAG = "Unit";
        public const string TILE_TAG = "Tile";
        
        public static bool IsClickable([NotNull] GameObject gameObject) => 
            gameObject.CompareTag(TILE_TAG) || gameObject.CompareTag(UNIT_TAG);

        public static bool? IsHoverable(GameObject gameObject) => gameObject?.CompareTag(TILE_TAG);
    }
}
