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
            
            ticks = new List<TileTickInfo>{new TileTickInfo(this), new TileTickInfo(this), new TileTickInfo(this), new TileTickInfo(this), new TileTickInfo(this)};
        }
    }
}
