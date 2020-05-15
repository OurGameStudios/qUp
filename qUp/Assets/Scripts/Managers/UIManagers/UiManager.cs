using System;
using Actors.Units;
using Base.Managers;
using Managers.ApiManagers;
using Managers.GridManagers;
using UI.HqUis;
using UI.InfoUis.SpawnUis;

namespace Managers.UIManagers {
    public class UiManager : BaseManager<IUiManagerState> {

        private readonly Lazy<GridManager> gridManagerLazy = new Lazy<GridManager>(ApiManager.ProvideManager<GridManager>);
        private readonly Lazy<SpawnUiInteractor> spawnUiInteractorLazy = new Lazy<SpawnUiInteractor>(ApiManager.ProvideInteractor<SpawnUiInteractor>);

        private GridManager GridManager => gridManagerLazy.Value;
        private SpawnUiInteractor SpawnUiInteractor => spawnUiInteractorLazy.Value;

        private UnitData selectedSpawnUnitData;

        public void UnitToSpawnSelected(UnitData unitData) {
            SpawnUiInteractor.ShowMenu(unitData.unitUiImage, unitData.name, unitData.cost, unitData.hp, unitData.attack, unitData.tickPoints);
            GridManager.UnitToSpawnSelected();
            selectedSpawnUnitData = unitData;
        }

        public UnitData ProvideSelectedUnit() => selectedSpawnUnitData;

        public void OnAwake() {
            ApiManager.ProvideInteractor<HqUiInteractor>().InitHqUi();
        }
    }
}
