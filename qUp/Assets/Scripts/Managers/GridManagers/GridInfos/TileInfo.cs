using System.Collections.Generic;
using Actors.Players;
using Actors.Tiles;
using Common;
using Priority_Queue;

namespace Managers.GridManagers.GridInfos {
    public class TileInfo : IFastPriorityQueueNode {
        public float Priority { get; set; }
        public int QueueIndex { get; set; }

        public GridCoords Coords { get; }
        public Tile Tile { get; }
        public List<Player> players;
        public List<TileTickInfo> ticks;
        public List<TileTickInfo> conflictedTicks;
        public Player owner;

        public TileInfo(GridCoords coords, Tile tile, List<Player> players) {
            Coords = coords;
            Tile = tile;
            this.players = players;
            ticks = new List<TileTickInfo> {
                                               new TileTickInfo(this, 0, players), 
                                               new TileTickInfo(this, 1, players),
                                               new TileTickInfo(this, 2, players), 
                                               new TileTickInfo(this, 3, players),
                                               new TileTickInfo(this, 4, players), 
                                               new TileTickInfo(this, 5, players)
                                           };
        }
    }
}
