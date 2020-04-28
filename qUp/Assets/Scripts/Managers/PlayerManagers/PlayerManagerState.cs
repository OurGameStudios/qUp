using Actors.Units;
using Base.Interfaces;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManagerState : IState { }

    public class SpawnUnit : PlayerManagerState {
        public Vector3 SpawnPosition { get; }
        public UnitData UnitData { get; }
        public SpawnUnit(Vector3 spawnPosition, UnitData unitData) {
            SpawnPosition = spawnPosition;
            UnitData = unitData;
        }
    }
}
