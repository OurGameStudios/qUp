using System;
using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using Managers.GridManagers.GridInfos;
using Managers.UIManagers;
using UnityEngine;

namespace Actors.Players {
    public class Player : BaseController<IPlayerState> {
        
        private readonly Lazy<UiManager> uiManagerLazy = new Lazy<UiManager>(ApiManager.ProvideManager<UiManager>);
        private UiManager UiManager => uiManagerLazy.Value;
        protected override bool Expose => true;

        private PlayerData data;

        private int income;
        private int upkeep;
        private int turnCost;

        private List<GridCoords> incomeSources = new List<GridCoords>(200);
        
        private List<UnitSpawnInfo> spawnCostSources = new List<UnitSpawnInfo>(6); 

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

        public void RegisterResourceIncome(int income, GridCoords incomeSource) {
            if (!incomeSources.Contains(incomeSource)) {
                incomeSources.Add(incomeSource);
                this.income += income;
            }
        }

        public void UnregisterResourceIncome(int income, GridCoords incomeSource) {
            if (incomeSources.Contains(incomeSource)) {
                incomeSources.Remove(incomeSource);
                this.income -= income;
            }
        }

        public int GetIncome() => income;

        public int GetAvailableIncome() => income - turnCost;

        public int GetTurnCost() => turnCost;

        public void RegisterUnitSpawnCost(UnitSpawnInfo unitSpawnInfo) {
            turnCost += unitSpawnInfo.unitData.cost;
            spawnCostSources.Add(unitSpawnInfo);
            UiManager.UpdateResourceUi();
        }
        
        public void UnregisterUnitSpawnCost(UnitSpawnInfo unitSpawnInfo) {
            turnCost -= unitSpawnInfo.unitData.cost;
            spawnCostSources.Remove(unitSpawnInfo);
            UiManager.UpdateResourceUi();
        }

        public void ResetTurnCost() => turnCost = upkeep;

        public UnitData GetResourceUnitData() => data.ResourceUnitData;
    }
}
