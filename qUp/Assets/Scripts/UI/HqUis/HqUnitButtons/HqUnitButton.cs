using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.HqUis.HqUnitButtons {
    public class HqUnitButton : Button {

        public Text unitName;
        public Text unitCost;

        private Action onButtonClick;

        public static HqUnitButton InstantiateButton(GameObject prefab, Transform parent) {
            var unitSpawnButton = Instantiate(prefab, parent).GetComponent<HqUnitButton>();
            return unitSpawnButton;
        }

        public override void OnPointerClick(PointerEventData eventData) {
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Left) {
                onButtonClick?.Invoke();
            }
        }

        public void Bind(Action onClick, string text, string cost) {
            unitName.text = text;
            unitCost.text = cost;
            onButtonClick = onClick;
        }
    }
}
