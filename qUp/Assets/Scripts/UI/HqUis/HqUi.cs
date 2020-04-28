using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Managers;
using Managers.UIManagers;

namespace UI.HqUis {
    public class HqUi : BaseController<HqUiState> {
        protected override bool Expose => true;

        private List<UnitData> units;

        public void Init(List<UnitData> inUnits) {
            this.units = inUnits;
            for (var i = 0; i < inUnits.Count; i++) {
                SetState(new UnitInfo(i, inUnits[i].prefab, inUnits[i].name, inUnits[i].cost.ToString()));
            }
        }

        public void OnClick(int menuPosition) {
            GlobalManager.GetManager<UiManager>().UnitToSpawnSelected(units[menuPosition]);
        }
    }
}
