using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Units;
using Actors.Units.Interface;
using Common;
using Extensions;

namespace Managers.GridManagers.GridInfos {
    public class TileTickInfo {
        public TileInfo TileInfo { get; }

        public int Tick { get; }

        public TileTickInfo(TileInfo tileInfo, int tick, List<Player> players) {
            TileInfo = tileInfo;
            Tick = tick;
            foreach (var player in players) {
                units[player] = new List<IUnit>(200);
            }
        }

        public bool IsOverflown => !owerflownUnits.IsEmpty();

        private readonly Dictionary<Player, List<IUnit>> units = new Dictionary<Player, List<IUnit>>(200);
        private readonly List<IUnit> owerflownUnits = new List<IUnit>(200);
        public void AddUnit(Player player, IUnit unit) => units[player].Add(unit);

        public List<IUnit> GetUnits(Player player) => units[player];

        public List<IUnit> GetUnits() => units.SelectMany(it => it.Value).ToList();
        
        // private readonly Dictionary<Player, List<ResourceUnit>> resourceUnits = new Dictionary<Player, List<ResourceUnit>>();
        
        private readonly Dictionary<GridCoords, ResourceUnitInfo> resourceUnitInfos = new Dictionary<GridCoords, ResourceUnitInfo>(200);

        public void ClearUnits(Player player) => units[player].Clear();

        public bool IsCombatTile() => units.Select(it => it.Value.Count > 0).Count() > 1;

        public void ClearUnits() {
            foreach (var playerUnits in units) {
                playerUnits.Value.Clear();
            }
        }

        public void RemoveUnit(Player player, IUnit unit) => units[player].Remove(unit);

        public int GetUnitCount(Player player) => units[player].Count;

        public bool ContainsUnit(Player player, IUnit unit) => units[player].Contains(unit);

        public void AddResourceUnit(ResourceUnitInfo info) => resourceUnitInfos.Add(info.Origin, info);

        public bool ContaisOriginatedResourceUnit(GridCoords resourceUnitOrigin) => resourceUnitInfos.ContainsKey(resourceUnitOrigin);

        public void RemoveResourceUnit(ResourceUnitInfo info) => resourceUnitInfos.Remove(info.Origin);
    }
}
