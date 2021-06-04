using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Units;
using Base.Singletons;
using Common;
using UnityEngine;

namespace Handlers.PlayerHandlers {
    public class PlayerHandler : SingletonDataClass<PlayerHandler, PlayerHandlerData>, IPlayerHandler {

        private List<IPlayer> players;

        private int currentPlayerIndex;

        public PlayerHandler() {
            players = Data.PlayerDatas.ConvertAll(it => (IPlayer) new Player(it));
        }

        public static void SubscribePlayersToPhaseManager() {
            foreach (var player in Instance.players) {
                player.SubscribeToPhaseManager();
            }
        }

        /// <summary>
        /// Returns a list of coordinates of hqs and players pairs.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<(GridCoords coords, IPlayer owner)> GetPlayerHqs() =>
            Instance.players.ConvertAll(it => it.GetHqInfo());

        /// <summary>
        /// Returns a player set as a current player
        /// </summary>
        /// <returns>Current player</returns>
        public static IPlayer GetCurrentPlayer() => Instance.players[Instance.currentPlayerIndex];

        /// <summary>
        /// Checks if current player is the last player.
        /// </summary>
        /// <returns>true if current player is the last player.</returns>
        public static bool IsLastPlayer() => Instance.players.Count == Instance.currentPlayerIndex + 1;
        
        /// <summary>
        /// Sets the next player as current player. If current player is the last player, sets the first player as
        /// current player.
        /// </summary>
        public static void NextPlayer() {
            if (Instance.currentPlayerIndex + 1 >= Instance.players.Count) {
                Instance.currentPlayerIndex = 0;
            } else {
                Instance.currentPlayerIndex++;
            }
        } 

        /// <summary>
        /// Registers all units a player can spawn and collects them into a pool. Sets the max tick of configuration
        /// by max tick of a spawnable unit.
        /// </summary>
        public void RegisterAllUnits() {
            foreach (var spawnableUnit in Instance.players.SelectMany(player => player.GetSpawnableUnits())) {
                UnitPool.RegisterUnitData(spawnableUnit);
                Configuration.Instance.TrySetMaxTick(spawnableUnit.tickPoints);
            }
        }

        /// <summary>
        /// Gets the index of a player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int GetPlayerIndex(IPlayer player) => Instance.players.IndexOf(player);

        public static int GetPlayerCount() => Instance.players.Count;

        /// <summary>
        /// Notifies spawn tiles of current player that spawn unit is selected
        /// </summary>
        /// <param name="unitData">Selected spawn unit</param>
        public static void NotifySpawnTilesUnitSpawnSelected(UnitData unitData) {
            foreach (var spawnTile in GetCurrentPlayer().GetSpawnTiles()) {
                spawnTile.SpawnUnitSelected(unitData);
            }
        }

        /// <summary>
        /// Notifies spawn tiles of current player that spawn tile is selected
        /// </summary>
        /// <param name="previousUnit"></param>
        /// <param name="unit"></param>
        public static void NotifySpawnTileSelected(UnitData previousUnit, UnitData unit) {
            foreach (var spawnTile in GetCurrentPlayer().GetSpawnTiles()) {
                spawnTile.SpawnTileSelected();
            }

            if (previousUnit != null) {
                GetCurrentPlayer().IncreaseIncome(previousUnit.cost);
            }

            if (unit != null) {
                GetCurrentPlayer().DecreaseIncome(unit.cost);
            }
        }

        public static List<Color> GetOrderedPlayerColors() => Instance.players.ConvertAll(player => player.GetPlayerColor());
    }
}
