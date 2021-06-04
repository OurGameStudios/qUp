using System.Collections.Generic;
using Actors.Players;
using Actors.Units;
using Common;
using Common.PriorityQueues;
using UnityEngine;

namespace Actors.Tiles {
    public interface ITile : IFastPriorityQueueNode {
        IPlayer Owner { get; }

        public GridCoords GetCoords();

        public void SpawnUnitSelected(UnitData unit);
        void SpawnTileSelected();
        void SelectedForSpawn(UnitData unitData);
        void SetAsSpawnTile(IPlayer forPlayer);
        bool IsAvailableForTick(int tick, IPlayer player, IUnit unit);
        void OnPathRange();
        void RemoveUnitFromTick(IPlayer player, int tick, bool isLast);
        void ShowSetPath();
        void OnPathSet(IPlayer player, int tick, IUnit unit, bool isOrigin, bool isLast);
        void ResetPlanningPhaseState(IPlayer player);

        Vector3 GetTileCenter();
        void OnMovedOver(IPlayer player, int tick, IUnit unit);
        List<IUnit> GetCombatantUnitsFor(int tick);
        void CombatOnTile(bool isActive);
        void CombatResolved(IPlayer winner);
        void AddResourceUnit(IUnit resourceUnit, int tick);
        bool IsSpawnTile();
        List<IUnit> GetResourceUnitsForTick(int tick);
        void RemoveResourceUnitForTick(IUnit unit, int tick);
        IPlayer TileOwnershipForTick(int tick);
        void ResourceUnitKilled(IUnit unit);
    }
}
