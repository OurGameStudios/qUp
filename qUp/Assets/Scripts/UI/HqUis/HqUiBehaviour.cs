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
                BindButton(unitInfoState.MenuPosition, unitInfoState.UnitCost, unitInfoState.UiImage);
            }
        }

        private void BindButton(int menuPosition, string unitCost, Sprite uiImage) {
            if (menuPosition >= buttons.Count) {
                buttons.Add(HqUnitButton.InstantiateButton(hqUnitButtonPrefab, transform));
            }
            buttons[menuPosition].Bind(() => { Controller.OnClick(menuPosition); }, unitCost, uiImage);
            buttons[menuPosition].gameObject.SetActive(true);
        }
    }
}
