using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class FieldShader : BaseShader {
        public FieldShader([NotNull] Material material) : base(material) {}
        
        private static readonly int HighlightColor = Shader.PropertyToID("HighlightColor");
        private static readonly int HighlightOn = Shader.PropertyToID("HighlightOn");
        private static readonly int HighlightOnColor = Shader.PropertyToID("HighlightOnColor");
        private static readonly int HighlightStartTime = Shader.PropertyToID("HighlightStartTime");
        
        public void SetHighlightColor(Color color) => SetColor(HighlightColor, color);

        public void SetHighlightOn(bool isOn) => SetBool(HighlightOn, isOn);

        public void SetHighlightOn(bool isOn, Color highlightColor) {
            SetBool(HighlightOn, isOn);
            SetColor(HighlightOnColor, highlightColor);
        }

        public void SetAnimationTimeOffset(float timeOffset) => SetFloat(HighlightStartTime, timeOffset);
    }
}
