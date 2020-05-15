using Base.MonoBehaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InfoUis.SpawnUis {
    public class SpawnUiBehaviour : BaseMonoBehaviour<SpawnUi, ISpawnUiState> {

        public Canvas spawnUiCanvas;
        
        public Image unitImage;
        private UnityEngine.UIElements.Image test;
        public TextMeshProUGUI unitName;
        public TextMeshProUGUI unitCost;
        public TextMeshProUGUI unitHp;
        public TextMeshProUGUI unitAtt;
        public TextMeshProUGUI unitTp;

        protected override void OnAwake() {
            spawnUiCanvas.enabled = false;
        }

        public void InitMenu(Sprite unitSprite, string name, string cost, string hp, string att, string tp) {
            unitImage.sprite = unitSprite;
            unitName.text = name;
            unitCost.text = cost;
            unitHp.text = hp;
            unitAtt.text = att;
            unitTp.text = tp;
            spawnUiCanvas.enabled = true;
        }

        protected override void OnStateHandler(ISpawnUiState inState) {
            if (inState is SetUI setUiState) {
                InitMenu(setUiState.UnitSprite,
                    setUiState.UnitName,
                    setUiState.UnitCost,
                    setUiState.UnitHp,
                    setUiState.UnitAtt,
                    setUiState.UnitTp);
            } else if (inState is ShowUI) {
                spawnUiCanvas.enabled = true; //.gameObject.SetActive(true);
            } else if (inState is HideUI) {
                spawnUiCanvas.enabled = false; //gameObject.SetActive(false);
            }
        }
    }
}
