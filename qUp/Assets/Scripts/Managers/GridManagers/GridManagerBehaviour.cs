using Base.Managers;
using UnityEngine;

namespace Managers.GridManagers {
    public class GridManagerBehaviour : BaseManagerMonoBehaviour<GridManager, IGridManagerState> {

        protected override void OnStateHandler(IGridManagerState inState) {
            if (inState is UnitSpawn unitSpawnState) {
                var ghost = Instantiate(unitSpawnState.UnitSpawnInfo.unitData.ghostPrefab, unitSpawnState.SpawnPosition, Quaternion.identity);
                unitSpawnState.UnitSpawnInfo.ghost = ghost;
                Controller.StoreUnitSpawn(unitSpawnState.UnitSpawnInfo);
            } else if (inState is UnitSpawned unitSpawnedState) {
                Destroy(unitSpawnedState.Ghost);
            }
        }
    }
}
