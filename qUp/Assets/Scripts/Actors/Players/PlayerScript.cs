using System;
using Base.Scripts;

namespace Actors.Players {
    public class PlayerScript : BaseScript<Player, PlayerState> {

        public PlayerData data;
        
        public PlayerScript(PlayerData data) {
            Controller.Init(data);
        }

        protected override void OnStateHandler(PlayerState inState) {
            throw new NotImplementedException();
        }

        public Player ExposeController() => Controller;
    }
}
