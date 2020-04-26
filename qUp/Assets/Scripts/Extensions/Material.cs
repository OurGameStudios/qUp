using UnityEngine;

namespace Extensions {
    public static class MaterialExtension {
        public static Material Clone(this Material material) => new Material(material);

        public static Material SetColor(this Material material, Color color) =>
            material.Also((it) => material.color = color);
    }
}
