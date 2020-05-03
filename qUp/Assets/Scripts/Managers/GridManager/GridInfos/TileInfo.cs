using System.Collections.Generic;
using Actors.Tiles;
using Common;

namespace Managers.GridManager.GridInfos {
    public class TileInfo {
        public GridCoords Coords { get; }
        public Tile Tile { get; }
        public List<TileTickInfo> ticks;
        public List<TileTickInfo> conflictedTicks;

        public TileInfo(GridCoords coords, Tile tile) {
            Coords = coords;
            Tile = tile;
            
            ticks = new List<TileTickInfo>{new TileTickInfo(this, 0), new TileTickInfo(this, 1), new TileTickInfo(this, 2), new TileTickInfo(this, 3), new TileTickInfo(this, 4), new TileTickInfo(this, 5)};
        }
    }
}
