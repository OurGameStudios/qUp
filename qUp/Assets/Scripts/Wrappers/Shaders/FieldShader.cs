using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class FieldShader : BaseShader {
        public FieldShader([NotNull] Material material) : base(material) {}
        
        private static readonly int markingsColor = Shader.PropertyToID("MarkingsColor");
        private static readonly int highlightOn = Shader.PropertyToID("HighlightOn");
        private static readonly int highlightOnColor = Shader.PropertyToID("HighlightOnColor");
        private static readonly int highlightStartTime = Shader.PropertyToID("HighlightStartTime");
        
        public void SetMarkingsColor(Color color) => SetColor(markingsColor, color);

        public Color GetMarkingsColor() => GetColor(markingsColor);

        public void SetHighlightOn(bool isOn) {
            SetBool(highlightOn, isOn);
            if (isOn) SetFloat(highlightStartTime, -Time.timeSinceLevelLoad);
        }

        public void SetHighlightOn(bool isOn, Color highlightColor) {
            SetBool(highlightOn, isOn);
            if (isOn) SetColor(highlightOnColor, highlightColor);
        }
    }
}
