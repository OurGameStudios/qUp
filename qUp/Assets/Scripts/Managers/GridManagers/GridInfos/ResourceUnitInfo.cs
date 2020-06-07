using System.Collections.Generic;
using Common;

namespace Managers.GridManagers.GridInfos {
    public class ResourceUnitInfo {
        public GridCoords Origin { get; }
        public List<TileTickInfo> Path { get; private set; } 

        public ResourceUnitInfo(GridCoords origin) {
            Origin = origin;
        }
    }
}
