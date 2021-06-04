using System.Collections.Generic;
using Actors.Tiles;
using Actors.Units;
using Common;
using UnityEngine;

namespace Actors.Players {
    public interface IPlayer {
        public (GridCoords coords, IPlayer owner) GetHqInfo();

        public GameObject GetHqPrefab();

        public Color GetPlayerColor();

        public string GetPlayerName();

        public List<UnitData> GetSpawnableUnits();

        public List<ITile> GetSpawnTiles();
        void AddSpawnTile(ITile spawnTile);
        int GetPlayerIndex();

        void SetRealHqCoords(GridCoords hqCoords);
        void IncreaseIncome(int incomeIncrease);
        void DecreaseIncome(int incomeDecrease);
        void SubscribeToPhaseManager();
        int AvailableIncome { get; }
        int TotalIncome { get; }
        int Points { get; set; }
    }
}
