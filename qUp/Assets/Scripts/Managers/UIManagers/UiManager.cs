using Actors.Hqs;
using Actors.Units;
using Base.Managers;
using Managers.ApiManagers;
using UI.HqUis;
using UI.InfoUis.SpawnUis;

namespace Managers.UIManagers {
    public class UiManager : BaseManager<UiManagerState> {
        public HqUiInteractor hqUiInteractor = ApiManager.ProvideInteractor<HqUiInteractor>();
        public SpawnUiInteractor spawnUiInteractor = ApiManager.ProvideInteractor<SpawnUiInteractor>();

        public void SetSelectedItem(Hq hq) {
            SetState(new BaseSelected(hq.ToString()));
        }

        public void OnStart() {
            hqUiInteractor.InitHqUi();
        }

        public void UnitToSpawnSelected(UnitData unitData) {
            spawnUiInteractor.ShowMenu(unitData.unitUiImage, unitData.name, unitData.cost, unitData.hp, unitData.attack, unitData.tickPoints);
            GlobalManager.GetManager<GridManager.GridManager>().UnitToSpawnSelected();
        }
    }
}
