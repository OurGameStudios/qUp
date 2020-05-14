using Base.Managers;

namespace Managers.UIManagers {
    public class UiManagerBehaviour : BaseManagerMonoBehaviour<UiManager, IUiManagerState> {

        protected override void OnAwake() {
            Controller.OnAwake();
        }

        protected override void OnStateHandler(IUiManagerState inState) {
        }
    }
}
