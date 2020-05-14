using System;
using Base.Scripts;

namespace Actors.Players {
    public class PlayerScript : BaseScript<Player, IPlayerState> {

        public PlayerData data;
        
        public PlayerScript(PlayerData data) {
            Controller.Init(data);
        }

        protected override void OnStateHandler(IPlayerState inState) {
            throw new NotImplementedException();
        }

        public Player ExposeController() => Controller;
    }
}
