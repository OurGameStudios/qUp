using System.Collections.Generic;
using Actors.Units;

namespace Managers.GridManager.GridInfos {
    public class TileTickInfo {

        public TileTickInfo(TileInfo tileInfo) {
            TileInfo = tileInfo;
        }
        
        public TileInfo TileInfo { get; }
        public List<Unit> units = new List<Unit>();
        public List<Unit> owerflownUnits = new List<Unit>();
    }
}
