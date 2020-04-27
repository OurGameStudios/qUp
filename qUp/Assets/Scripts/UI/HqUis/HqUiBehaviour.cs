using System.Collections.Generic;
using Base.MonoBehaviours;
using UI.HqUis.HqUnitButtons;
using UnityEngine;

namespace UI.HqUis {
    public class HqUiBehaviour : BaseMonoBehaviour<HqUi, HqUiState> {
        [SerializeField]
        private GameObject hqUnitButtonPrefab;

        private List<HqUnitButton> buttons = new List<HqUnitButton>();

        protected override void OnStateHandler(HqUiState inState) {
            if (inState is UnitInfo unitInfoState) {
                bindButton(unitInfoState.MenuPosition, unitInfoState.UnitName, unitInfoState.UnitCost);
            }
        }

        private void bindButton(int menuPosition, string unitName, string unitCost) {
            if (menuPosition >= buttons.Count) {
                buttons.Add(HqUnitButton.InstantiateButton(hqUnitButtonPrefab, transform));
            }
            buttons[menuPosition].bind(() => { Controller.OnClick(menuPosition); }, unitName, unitCost);
            buttons[menuPosition].gameObject.SetActive(true);
        }
    }
}
