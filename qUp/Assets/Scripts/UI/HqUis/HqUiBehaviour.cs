using System.Collections.Generic;
using Base.MonoBehaviours;
using UI.HqUis.HqUnitButtons;
using UnityEngine;

namespace UI.HqUis {
    public class HqUiBehaviour : BaseMonoBehaviour<HqUi, IHqUiState> {
        [SerializeField]
        private GameObject hqUnitButtonPrefab;

        private List<HqUnitButton> buttons = new List<HqUnitButton>();

        protected override void OnStateHandler(IHqUiState inState) {
            if (inState is UnitInfo unitInfoState) {
                BindButton(unitInfoState.MenuPosition, unitInfoState.UnitName, unitInfoState.UnitCost);
            }
        }

        private void BindButton(int menuPosition, string unitName, string unitCost) {
            if (menuPosition >= buttons.Count) {
                buttons.Add(HqUnitButton.InstantiateButton(hqUnitButtonPrefab, transform));
            }
            buttons[menuPosition].Bind(() => { Controller.OnClick(menuPosition); }, unitName, unitCost);
            buttons[menuPosition].gameObject.SetActive(true);
        }
    }
}
