using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    public class PlayerBaseShader : BaseShader {
        public PlayerBaseShader([NotNull] Material material) : base(material) { }

        private static readonly int meshColor = Shader.PropertyToID("MeshColor");
        private static readonly int isSelected = Shader.PropertyToID("IsSelected");
        private static readonly int highlightTOffset = Shader.PropertyToID("HighlightTOffset");
        private static readonly int playerColor = Shader.PropertyToID("playerColor");

        public void SetIsSelected(bool isSelected) {
            SetFloat(highlightTOffset, Time.time);
            SetBool(PlayerBaseShader.isSelected, isSelected);
        }
        public void SetColor(Color color) => SetColor(meshColor, color);

        public void SetPlayerColor(Color color) => SetColor(playerColor, color);
    }
}
