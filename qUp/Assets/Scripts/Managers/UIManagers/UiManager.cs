using Actors.PlayerBases;
using Base.Managers;

namespace Managers.UIManagers {
    public class UiManager : BaseManager<UiManagerState> {
        public void SetSelectedItem(PlayerBase playerBase) {
            SetState(new BaseSelected(playerBase.ToString()));
        }
    }
}
