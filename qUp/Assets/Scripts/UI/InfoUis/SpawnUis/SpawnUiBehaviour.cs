using Base.MonoBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoUis.SpawnUis {
    public class SpawnUiBehaviour : BaseMonoBehaviour<SpawnUi, SpawnUiState> {
        public Image unitImage;
        private UnityEngine.UIElements.Image test;
        public Text unitName;
        public Text unitCost;
        public Text unitHp;
        public Text unitAtt;
        public Text unitTp;

        protected override void OnAwake() {
            gameObject.SetActive(false);
        }

        public void InitMenu(Sprite unitSprite, string name, string cost, string hp, string att, string tp) {
            unitImage.sprite = unitSprite;
            unitName.text = name;
            unitCost.text = cost;
            unitHp.text = hp;
            unitAtt.text = att;
            unitTp.text = tp;
            gameObject.SetActive(true);
        }

        protected override void OnStateHandler(SpawnUiState inState) {
            if (inState is SetUI setUiState) {
                InitMenu(setUiState.UnitSprite,
                    setUiState.UnitName,
                    setUiState.UnitCost,
                    setUiState.UnitHp,
                    setUiState.UnitAtt,
                    setUiState.UnitTp);
            } else if (inState is ShowUI) {
                gameObject.SetActive(true);
            } else if (inState is HideUI) {
                gameObject.SetActive(false);
            }
        }
    }
}
