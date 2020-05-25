using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class UnitShader : BaseShader {
        public UnitShader([NotNull] Material material) : base(material) { }
        
        private static readonly int HighlightTOffset = Shader.PropertyToID("HighlightTOffset");
        private static readonly int IsHighlightOn = Shader.PropertyToID("IsHighlightOn");
        private static readonly int Color = Shader.PropertyToID("MeshColor");
        
        public void EnableHighlight() {
            SetFloat(HighlightTOffset, Time.time);
            SetBool(IsHighlightOn, true);
        }

        public void DisableHighlight() {
            SetBool(IsHighlightOn, false);
        }

        public void SetUnitPlayerColor(Color color) {
            SetColor(Color, color);
        }
    }
}
