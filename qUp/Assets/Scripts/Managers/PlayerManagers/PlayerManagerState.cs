using Actors.Units;
using Base.Interfaces;
using Common;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManagerState : IState { }

    public class SpawnUnit : PlayerManagerState {
        public Vector3 SpawnPosition { get; }
        public UnitData UnitData { get; }

        public GridCoords Coords { get; }
        public SpawnUnit(Vector3 spawnPosition, UnitData unitData, GridCoords coords) {
            SpawnPosition = spawnPosition;
            UnitData = unitData;
            Coords = coords;
        }
    }
}
