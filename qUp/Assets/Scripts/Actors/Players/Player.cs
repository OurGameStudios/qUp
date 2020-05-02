using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Common;
using UnityEngine;

namespace Actors.Players {
    public class Player : BaseController<PlayerState> {
        protected override bool Expose => true;

        private PlayerData data;

        public void Init(PlayerData inData) {
            data = inData;
        }

        public (GridCoords coords, Player owner) GetHqInfo() => (data.HqCoordinates, this);

        public GridCoords GetBaseCoordinates() => data.HqCoordinates;

        public Color PlayerColor => data.PlayerColor;

        public GameObject BasePrefab => data.HqPrefab;

        public string PlayerName => data.PlayerName;

        public List<UnitData> UnitDatas => data.UnitDatas;
    }
}
