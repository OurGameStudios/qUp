using JetBrains.Annotations;
using UnityEngine;
using Wrappers.Shaders.Base;

namespace Wrappers.Shaders {
    
    public class PuckShader : BaseShader{
        
        public PuckShader([NotNull] Material material) : base(material) { }
        
        private static readonly int PlayerColor = Shader.PropertyToID("playerColor");
        
        public void SetPlayerColor(Color color) => SetColor(PlayerColor, color);
    }
}
