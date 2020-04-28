using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Tiles;
using Actors.Units;
using Base.Managers;
using Common;
using Extensions;
using Managers.ApiManagers;
using Managers.UIManagers;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManager : BaseManager<PlayerManagerState> {

        private PlayerInteractor playerInteractor = ApiManager.ProvideInteractor<PlayerInteractor>();

        private List<PlayerScript> players;

        public void Init(PlayerManagerData data) {
            players = data.PlayerDatas.ConvertAll(it => new PlayerScript(it));
        }

        public Player GetCurrentPlayer() => players.First().ExposeController();

        public void SpawnUnit(Tile onTile) {
            var tilePosition = onTile.ProvideSpawnPoint();
            var unitData = GlobalManager.GetManager<UiManager>().ProvideSelectedUnit();
            var spawnPosition = tilePosition.AddY(unitData.prefab.transform.localScale.y);
            SetState(new SpawnUnit(spawnPosition, unitData));
        }
    }
}
