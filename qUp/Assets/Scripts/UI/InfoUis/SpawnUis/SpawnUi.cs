using Base.MonoBehaviours;
using UnityEngine;

namespace UI.InfoUis.SpawnUis {
    public class SpawnUi : BaseController<ISpawnUiState> {
        protected override bool Expose => true;

        private Sprite unitSprite;
        private string unitName;
        private int unitCost;
        private int unitHp;
        private int unitAtt;
        private int unitTp;
        
        
  
        public void ShowMenu(Sprite sprite, string name, int cost, int hp, int att, int tp) {
            if (unitSprite != sprite || unitName != name || unitCost != cost || unitHp != hp || unitAtt != att || unitTp != tp) {
                SetState(SetUI.Where(sprite, name, cost.ToString(), hp.ToString(), att.ToString(), tp.ToString()));
            } else {
                SetState(ShowUI.Where());
            }
            unitSprite = sprite;
            unitName = name;
            unitCost = cost;
            unitHp = hp;
            unitAtt = att;
            unitTp = tp;
        }

        public void HideMenu() {
            SetState(new HideUI());
        }
    }
}
