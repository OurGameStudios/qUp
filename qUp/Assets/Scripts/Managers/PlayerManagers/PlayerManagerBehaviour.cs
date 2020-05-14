using Actors.Units;
using Base.Managers;
using Common;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManagerBehaviour : BaseManagerMonoBehaviour<PlayerManager, IPlayerManagerState> {
        public PlayerManagerData data;

        protected override void OnStateHandler(IPlayerManagerState inState) {
            if (inState is UnitSpawn spawnUnitState) {
                SpawnUnit(spawnUnitState.SpawnPosition, spawnUnitState.UnitData, spawnUnitState.Coords);
            }
        }

        protected override void OnAwake() {
            Controller.Init(data);
        }

        private void SpawnUnit(Vector3 position, UnitData unitData, GridCoords coords) {
            var unit = UnitBehaviour.Instantiate(unitData, position);
            unit.SetCoords(coords);
        }
    }
}
