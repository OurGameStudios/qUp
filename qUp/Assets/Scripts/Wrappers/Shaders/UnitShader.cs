using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class UnitShader : BaseShader {
        public UnitShader([NotNull] Material material) : base(material) { }
        
        private static readonly int highlightTOffset = Shader.PropertyToID("HighlightTOffset");
        private static readonly int isHighlightOn = Shader.PropertyToID("IsHighlightOn");
        private static readonly int color = Shader.PropertyToID("MeshColor");
        
        public void EnableHighlight() {
            SetFloat(highlightTOffset, Time.time);
            SetBool(isHighlightOn, true);
        }

        public void DisableHighlight() {
            SetBool(isHighlightOn, false);
        }

        public void SetUnitPlayerColor(Color color) {
            SetColor(UnitShader.color, color);
        }
    }
}
