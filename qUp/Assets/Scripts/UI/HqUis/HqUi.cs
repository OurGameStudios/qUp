using System;
using System.Collections.Generic;
using Actors.Units;
using Base.MonoBehaviours;
using Managers.ApiManagers;
using Managers.UIManagers;

namespace UI.HqUis {
    public class HqUi : BaseController<IHqUiState> {

        private readonly Lazy<UiManager> uiManagerLazy = new Lazy<UiManager>(ApiManager.ProvideManager<UiManager>);
        private UiManager UiManager => uiManagerLazy.Value;
        
        protected override bool Expose => true;

        private List<UnitData> units;

        public void Init(List<UnitData> inUnits) {
            units = inUnits;
            for (var i = 0; i < inUnits.Count; i++) {
                SetState(UnitInfo.Where(i, inUnits[i].prefab, inUnits[i].name, inUnits[i].cost.ToString()));
            }
        }

        public void OnClick(int menuPosition) {
            UiManager.UnitToSpawnSelected(units[menuPosition]);
        }
    }
}
