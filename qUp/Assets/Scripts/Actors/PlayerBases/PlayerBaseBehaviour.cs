using Actors.Players;
using Base;
using Base.MonoBehaviours;
using Common;
using UnityEngine;
using Wrappers.Shaders;
using Wrappers.Shaders.Base;

namespace Actors.PlayerBases {
    public class PlayerBaseBehaviour : BaseMonoBehaviour<PlayerBase, PlayerBaseState> {
        private PlayerBaseShader playerBaseShader;

        public static void Instantiate(Vector3 position, GridCoords coords, Player owner) {
            Instantiate(owner.BasePrefab, position, Quaternion.identity)
                .GetComponentInChildren<PlayerBaseBehaviour>()
                .InitBase(owner, coords);
        }

        private void InitBase(Player owner, GridCoords coords) {
            Controller.Init(coords, owner);
            playerBaseShader = new PlayerBaseShader(GetComponent<MeshRenderer>().material);
            playerBaseShader.SetColor(owner.PlayerColor);
            name = owner.PlayerName + " Base";
        }

        protected override void OnStateHandler(PlayerBaseState inBaseState) {
            if (inBaseState is BaseSelection selectionState) {
                playerBaseShader.SetIsSelected(selectionState.IsSelected);
            }
        }

        private void OnMouseDown() => Controller.OnClick();
    }
}
