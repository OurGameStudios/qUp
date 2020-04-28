using JetBrains.Annotations;
using UnityEngine;

namespace Wrappers.Shaders.Base {
    public abstract class BaseShader {
        protected Material material;

        public BaseShader([NotNull] Material material) {
            this.material = material;
        }

        protected void SetColor(int properyId, Color inColor) => material.SetColor(properyId, inColor);

        protected Color GetColor(int propertyId) => material.GetColor(propertyId);
        
        protected void SetInt(int propertyId, int inInt) => material.SetInt(propertyId, inInt);

        protected void SetFloat(int propertyId, float inFloat) => material.SetFloat(propertyId, inFloat);

        protected void SetBool(int propertyId, bool inBool) => material.SetFloat(propertyId, inBool ? 1f : 0f);
        }
}
