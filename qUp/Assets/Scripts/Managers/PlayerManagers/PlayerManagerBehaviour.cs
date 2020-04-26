using Base.Managers;

namespace Managers.PlayerManagers {
    public class PlayerManagerBehaviour : BaseManagerMonoBehaviour<PlayerManager, PlayerManagerState> {
        public PlayerManagerData data;

        protected override void OnStateHandler(PlayerManagerState inState) { }

        protected override void OnAwake() {
            
            Controller.Init(data);
        }
    }
}
