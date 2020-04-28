using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Tiles;
using Base.Managers;
using Common;
using Managers.ApiManagers;

namespace Managers.PlayerManagers {
    public class PlayerManager : BaseManager<PlayerManagerState> {

        private PlayerInteractor playerInteractor = ApiManager.ProvideInteractor<PlayerInteractor>();

        private List<PlayerScript> players;

        public void Init(PlayerManagerData data) {
            players = data.PlayerDatas.ConvertAll(it => new PlayerScript(it));
        }

        public Player GetCurrentPlayer() => players.First().ExposeController();

        public void SpawnUnit(Tile onTile) {
            
        }
    }
}
