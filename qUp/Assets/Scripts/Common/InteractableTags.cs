﻿using UnityEngine;

namespace Common {
    public static class InteractableTags {
        private const string UNIT_TAG = "Unit";
        private const string TILE_TAG = "Tile";
        
        public static bool IsClickable(GameObject gameObject) => 
            gameObject.CompareTag(TILE_TAG) || gameObject.CompareTag(UNIT_TAG);

        public static bool? IsHoverable(GameObject gameObject) => gameObject?.CompareTag(TILE_TAG);
    }
}