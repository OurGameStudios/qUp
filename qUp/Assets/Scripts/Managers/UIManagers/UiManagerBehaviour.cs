using Base.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.UIManagers {
    public class UiManagerBehaviour : BaseManagerMonoBehaviour<UiManager, IUiManagerState> {

        public GameObject phaseInfoPanel;
        public Text haseInfoText;

        protected override void OnAwake() {
            Controller.OnAwake();
        }

        protected override void OnStateHandler(IUiManagerState inState) {
            if (inState is PhaseShow phaseShow) {
                phaseInfoPanel.SetActive(true);
                haseInfoText.text = phaseShow.PhaseText;
            } else if (inState is PhaseHide) {
                phaseInfoPanel.SetActive(false);
            }
        }
    }
}
