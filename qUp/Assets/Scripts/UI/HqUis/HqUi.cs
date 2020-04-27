using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Managers;
using Managers.UIManagers;

namespace UI.HqUis {
    public class HqUi : BaseController<HqUiState> {
        protected override bool Expose => true;

        private List<UnitData> units;

        public void Init(List<UnitData> units) {
            this.units = units;
            for (var i = 0; i < units.Count; i++) {
                SetState(new UnitInfo(i, units[i].prefab, units[i].name, units[i].cost.ToString()));
            }
        }

        public void OnClick(int menuPosition) {
            GlobalManager.GetManager<UiManager>().UnitToSpawnSelected(units[menuPosition]);
        }
    }
}
