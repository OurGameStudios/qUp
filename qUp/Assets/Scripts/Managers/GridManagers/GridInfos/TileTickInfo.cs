using System.Collections.Generic;
using Actors.Units;
using Extensions;

namespace Managers.GridManagers.GridInfos {
    public class TileTickInfo {

        public TileTickInfo(TileInfo tileInfo, int tick) {
            TileInfo = tileInfo;
            Tick = tick;
        }
        
        public TileInfo TileInfo { get; }
        public int Tick { get; }
        public bool IsOverflown => !owerflownUnits.IsEmpty();
        
        public List<Unit> units = new List<Unit>();
        public List<Unit> owerflownUnits = new List<Unit>();

        public void AddUnit(Unit unit) => units.Add(unit);

        public List<Unit> GetUnits() => units;
    }
}
