using System.Collections.Generic;
using Actors.Units;

namespace Managers.GridManager.GridInfos {
    public class TileTickInfo {
        public TileInfo tileInfo;
        public List<Unit> units;
        public List<Unit> owerflownUnits;
    }
}
