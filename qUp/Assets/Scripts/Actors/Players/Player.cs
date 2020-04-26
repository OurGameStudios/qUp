using Base;
using Base.MonoBehaviours;
using Common;
using UnityEngine;

namespace Actors.Players {
    public class Player : BaseController<PlayerState> {

        protected override bool Expose => true;

        private PlayerData data;

        public void Init(PlayerData data) {
            this.data = data;
        }

        public (GridCoords coords, Player owner) GetBaseInfo() => (data.BaseCoordinates, this);
        
        public GridCoords GetBaseCoordinates() => data.BaseCoordinates;

        public Color PlayerColor => data.PlayerColor;

        public GameObject BasePrefab => data.BasePrefab;

        public string PlayerName => data.PlayerName;
    }
}
