using Actors.Players;
using Actors.Units;
using Actors.Units.CombatUnits;
using Actors.Units.ResourceUnits;
using Base.Managers;
using Common;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManagerBehaviour : BaseManagerMonoBehaviour<PlayerManager, IPlayerManagerState> {
        public PlayerManagerData data;

        protected override void OnStateHandler(IPlayerManagerState inState) {
            if (inState is UnitSpawn spawnUnitState) {
                SpawnUnit(spawnUnitState.SpawnPosition, spawnUnitState.UnitData, spawnUnitState.Coords, spawnUnitState.Player);
            } else if (inState is ResourceUnitSpawn spawnResourceUnitState) {
                SpawnResourceUnit(spawnResourceUnitState.SpawnPosition, spawnResourceUnitState.UnitData, spawnResourceUnitState.Coords, spawnResourceUnitState.Player);
            }
        }

        protected override void OnAwake() {
            Controller.Init(data);
        }

        private void SpawnUnit(Vector3 position, UnitData unitData, GridCoords coords, Player player) {
            var unit = CombatUnitBehaviour.Instantiate(unitData, position, player);
            unit.SetCoords(coords, player);
        }
        
        private void SpawnResourceUnit(Vector3 position, UnitData unitData, GridCoords coords, Player player) {
            var unit = ResourceUnitBehaviour.Instantiate(unitData, position, player);
            unit.SetCoords(coords, player);
        }
    }
}
