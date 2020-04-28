using Actors.Players;
using Base.MonoBehaviours;
using Common;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Hqs {
    public class HqBehaviour : BaseMonoBehaviour<Hq, HqState> {
        private PlayerBaseShader playerBaseShader;

        public static void Instantiate(Vector3 position, GridCoords coords, Player owner) {
            Instantiate(owner.BasePrefab, position, Quaternion.identity)
                .GetComponentInChildren<HqBehaviour>()
                .InitBase(owner, coords);
        }

        private void InitBase(Player owner, GridCoords coords) {
            Controller.Init(coords, owner);
            playerBaseShader = new PlayerBaseShader(GetComponent<MeshRenderer>().material);
            playerBaseShader.SetColor(owner.PlayerColor);
            name = owner.PlayerName + " Base";
        }

        protected override void OnStateHandler(HqState inBaseState) {
            if (inBaseState is HqSelection selectionState) {
                playerBaseShader.SetIsSelected(selectionState.IsSelected);
            }
        }
    }
}
