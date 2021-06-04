using System;
using System.Collections.Generic;
using Actors.Tiles;
using Actors.Units;
using Common;
using Handlers;
using Handlers.PhaseHandlers;
using Handlers.PlayerHandlers;
using UI;
using UnityEngine;

namespace Actors.Players {
    public class Player : IPlayer, IDisposable {
        private readonly PlayerData data;

        private readonly List<ITile> spawnTiles = new List<ITile>();

        private GridCoords? hqCoords;

        private int availableAvailableIncome;

        public int TotalIncome { get; private set; }

        public int AvailableIncome {
            get => availableAvailableIncome;
            private set => availableAvailableIncome = value < 0 ? 0 : value;
        }

        private int points;

        public int Points {
            get => points;
            set {
                points = value;
                PlayerPointsUi.UpdatePlayerPoints(this, points);
                TestWinCondition(points);
            }
        }

        public Player(PlayerData data) {
            this.data = data;
            TotalIncome = data.BaseIncome;
            AvailableIncome = data.BaseIncome;
        }

        public void SubscribeToPhaseManager() {
            PhaseHandler.PlayerChange.Subscribe(OnPlayerChangePhase);
        }

        public (GridCoords coords, IPlayer owner) GetHqInfo() => (hqCoords ?? data.HqCoordinates, this);
        
        public GameObject GetHqPrefab() => data.HqPrefab;

        public Color GetPlayerColor() => data.PlayerColor;

        public string GetPlayerName() => data.PlayerName;

        public List<UnitData> GetSpawnableUnits() => data.UnitDatas;

        public List<ITile> GetSpawnTiles() => spawnTiles;

        public void AddSpawnTile(ITile spawnTile) => spawnTiles.Add(spawnTile);

        public int GetPlayerIndex() => PlayerHandler.GetPlayerIndex(this);

        public void SetRealHqCoords(GridCoords hqCoords) => this.hqCoords = hqCoords;

        public void IncreaseIncome(int incomeIncrease) {
            AvailableIncome += incomeIncrease;
            TotalIncome += incomeIncrease;
            ResourceUi.UpdatePlayerResource(this);
        }

        public void DecreaseIncome(int incomeDecrease) {
            AvailableIncome -= incomeDecrease;
            ResourceUi.UpdatePlayerResource(this);
        }

        private void OnPlayerChangePhase(IPlayer player) {
            if (player == this) {
                AvailableIncome = data.BaseIncome;
                TotalIncome = data.BaseIncome;
            }
        }

        public void Dispose() {
            PhaseHandler.PlanningPhase.Unsubscribe(OnPlayerChangePhase);
        }

        private void TestWinCondition(int points) {
            if (points >= Configuration.GetMaxPoints()) {
                PhaseHandler.EndGame(this);
            }
        }
    }
}
