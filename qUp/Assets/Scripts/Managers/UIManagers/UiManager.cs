using Actors.Units;
using Base.Managers;
using Managers.ApiManagers;
using Managers.GridManagers;
using UI.HqUis;
using UI.InfoUis.SpawnUis;

namespace Managers.UIManagers {
    public class UiManager : BaseManager<IUiManagerState> {
        // private HqUiInteractor hqUiInteractor;
        // private SpawnUiInteractor spawnUiInteractor;
        // private GridManager gridManager;

        private UnitData selectedSpawnUnitData;

        public void UnitToSpawnSelected(UnitData unitData) {
            ApiManager.ProvideInteractor<SpawnUiInteractor>().ShowMenu(unitData.unitUiImage, unitData.name, unitData.cost, unitData.hp, unitData.attack, unitData.tickPoints);
            ApiManager.ProvideManager<GridManager>().UnitToSpawnSelected();
            selectedSpawnUnitData = unitData;
        }

        public UnitData ProvideSelectedUnit() => selectedSpawnUnitData;

        public void OnAwake() {
            ApiManager.ProvideInteractor<HqUiInteractor>().InitHqUi();
        }
    }
}
