using Base.Managers;
using Managers.CameraManagers;
using UnityEngine.UI;

namespace Managers.UIManagers {
    public class UiManagerBehaviour : BaseManagerMonoBehaviour<UiManager, UiManagerState> {
        public Text selectedItemText;

        private void OnMouseEnter() {
            GlobalManager.GetManager<CameraManager>().DisableCameraPan();
        }

        private void Start() {
            Controller.OnStart();
        }

        protected override void OnStateHandler(UiManagerState inState) {
            if (inState is BaseSelected baseSelectedState) {
                selectedItemText.text = baseSelectedState.Name;
            }
        }
    }
}
