using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class PlayerBaseShader : BaseShader {
        public PlayerBaseShader([NotNull] Material material) : base(material) { }

        private static readonly int MeshColor = Shader.PropertyToID("MeshColor");
        private static readonly int IsSelected = Shader.PropertyToID("IsSelected");
        private static readonly int HighlightTOffset = Shader.PropertyToID("HighlightTOffset");
        private static readonly int PlayerColor = Shader.PropertyToID("playerColor");

        public void SetIsSelected(bool isSelected) {
            SetFloat(HighlightTOffset, Time.time);
            SetBool(IsSelected, isSelected);
        }
        public void SetColor(Color color) => SetColor(MeshColor, color);

        public void SetPlayerColor(Color color) => SetColor(PlayerColor, color);
    }
}
