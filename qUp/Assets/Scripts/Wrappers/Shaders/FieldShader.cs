using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class FieldShader : BaseShader {
        public FieldShader([NotNull] Material material) : base(material) {}
        
        private static readonly int MarkingsColor = Shader.PropertyToID("MarkingsColor");
        private static readonly int HighlightOn = Shader.PropertyToID("HighlightOn");
        private static readonly int HighlightOnColor = Shader.PropertyToID("HighlightOnColor");
        private static readonly int HighlightStartTime = Shader.PropertyToID("HighlightStartTime");
        
        public void SetMarkingsColor(Color color) => SetColor(MarkingsColor, color);

        public Color GetMarkingsColor() => GetColor(MarkingsColor);

        public void SetHighlightOn(bool isOn) {
            SetBool(HighlightOn, isOn);
            if (isOn) SetFloat(HighlightStartTime, -Time.timeSinceLevelLoad);
        }

        public void SetHighlightOn(bool isOn, Color highlightColor) {
            SetBool(HighlightOn, isOn);
            SetColor(HighlightOnColor, highlightColor);
        }
    }
}
