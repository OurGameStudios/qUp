using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Common;
using UnityEngine;

namespace Actors.Players {
    public class Player : BaseController<IPlayerState> {
        protected override bool Expose => true;

        private PlayerData data;

        private int income;
        private int upkeep;
        private int turnCost;

        public void Init(PlayerData inData) {
            data = inData;
            income = data.BaseIncome;
        }

        public (GridCoords coords, Player owner) GetHqInfo() => (data.HqCoordinates, this);

        public GridCoords GetBaseCoordinates() => data.HqCoordinates;

        public Color PlayerColor => data.PlayerColor;

        public GameObject BasePrefab => data.HqPrefab;

        public string PlayerName => data.PlayerName;

        public List<UnitData> UnitDatas => data.UnitDatas;

        public void RegisterUnitUpkeep(int upkeep, int cost) {
            this.upkeep += upkeep;
            turnCost += cost;
        }

        public void UnregisterUnitUpkeep(int upkeep) => this.upkeep -= upkeep;

        public int GetUpkeep() => upkeep;

        public void RegisterResourceIncome(int income) => this.income += income;

        public void UnregisterResourceIncome(int income) => this.income += income;

        public int GetIncome() => income;

        public int GetAvailableIncome() => income - turnCost;

        public int GetTurnCost() => turnCost;

        public void ResetTurnCost() => turnCost = upkeep;
    }
}
