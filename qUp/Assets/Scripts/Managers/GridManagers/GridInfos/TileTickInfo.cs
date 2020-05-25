using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Units;
using Extensions;

namespace Managers.GridManagers.GridInfos {
    public class TileTickInfo {
        public TileInfo TileInfo { get; }

        public int Tick { get; }

        public TileTickInfo(TileInfo tileInfo, int tick, List<Player> players) {
            TileInfo = tileInfo;
            Tick = tick;
            foreach (var player in players) {
                units[player] = new List<Unit>();
            }
        }

        public bool IsOverflown => !owerflownUnits.IsEmpty();

        private readonly Dictionary<Player, List<Unit>> units = new Dictionary<Player, List<Unit>>(200);
        private readonly List<Unit> owerflownUnits = new List<Unit>();
        public void AddUnit(Player player, Unit unit) => units[player].Add(unit);

        public List<Unit> GetUnits(Player player) => units[player];

        public List<Unit> GetUnits() => units.SelectMany(it => it.Value).ToList();

        public void ClearUnits(Player player) => units[player].Clear();

        public bool IsCombatTile() => units.Select(it => it.Value.Count > 0).Count() > 1;

        public void ClearUnits() {
            foreach (var playerUnits in units) {
                playerUnits.Value.Clear();
            }
        }

        public void RemoveUnit(Player player, Unit unit) => units[player].Remove(unit);

        public int GetUnitCount(Player player) => units[player].Count;

        public bool ContainsUnit(Player player, Unit unit) => units[player].Contains(unit);
    }
}
