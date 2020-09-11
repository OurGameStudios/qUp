using System.Collections.Generic;
using Actors.Players;
using Common;
using Extensions;

namespace Managers.GridManagers.GridInfos {
    public class ResourceUnitInfo {
        public Player Owner { get; }
        public GridCoords Origin { get; }
        
        public List<TileInfo> Path { get;  } 

        public ResourceUnitInfo(GridCoords origin, Player owner, List<TileInfo> path) {
            Origin = origin;
            Owner = owner;
            Path = path.Also(it => it.RemoveAt(0));
        }
    }
}
