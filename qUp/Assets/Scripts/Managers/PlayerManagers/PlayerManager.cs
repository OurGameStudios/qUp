using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Base.Managers;
using Common;
using Extensions;
using Managers.UIManagers;
using UnityEngine;

namespace Managers.PlayerManagers {
    public class PlayerManager : BaseManager<PlayerManagerState> {
        private List<PlayerScript> players;

        public void Init(PlayerManagerData data) {
            players = data.PlayerDatas.ConvertAll(it => new PlayerScript(it));
        }

        public Player GetCurrentPlayer() => players.First().ExposeController();

        public void SpawnUnit(Vector3 tilePosition, GridCoords coords) {
            var unitData = GlobalManager.GetManager<UiManager>().ProvideSelectedUnit();
            var spawnPosition = tilePosition.AddY(unitData.prefab.transform.localScale.y / 2);
            SetState(new SpawnUnit(spawnPosition, unitData, coords));
        }
    }
}