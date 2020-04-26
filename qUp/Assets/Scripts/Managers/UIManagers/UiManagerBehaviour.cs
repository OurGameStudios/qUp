using Base.Managers;
using Managers.CameraManagers;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.UIManagers {
    public class UiManagerBehaviour : BaseManagerMonoBehaviour<UiManager, UiManagerState> {

        public Text selectedItemText;
        public GameObject panel;

        private void OnMouseEnter() {
            GlobalManager.GetManager<CameraManager>().DisableCameraPan();
        }

        protected override void OnStateHandler(UiManagerState inState) {
            if (inState is BaseSelected baseSelectedState) {
                selectedItemText.text = baseSelectedState.Name;
                panel.SetActive(true);
            }
        }
    }
}
