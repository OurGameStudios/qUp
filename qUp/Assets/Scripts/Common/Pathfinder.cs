using System.Collections.Generic;
using Extensions;
using Managers.GridManager.GridInfos;
using Priority_Queue;

namespace Common {
    public class Pathfinder {
        public const int MAX_NUM_OF_UNITS = 3;
        public const int MAX_NUM_OF_TILES = 200;

        private GridCoords neighbourCoords;
        private readonly FastPriorityQueue<TileInfo> frontier;
        private readonly Dictionary<TileInfo, TileInfo> cameFrom;
        private readonly Dictionary<TileInfo, int> costSoFar;
        private readonly List<TileInfo> neighbours;
        private TileInfo start;

        private readonly FastPriorityQueue<TileInfo> forgottenTilesFast;

        public Pathfinder(int maxNumOfTiles = MAX_NUM_OF_TILES) {
            neighbourCoords = new GridCoords();
            frontier = new FastPriorityQueue<TileInfo>(maxNumOfTiles);
            cameFrom = new Dictionary<TileInfo, TileInfo>(maxNumOfTiles);
            costSoFar = new Dictionary<TileInfo, int>(maxNumOfTiles);
            neighbours = new List<TileInfo>(6);
            forgottenTilesFast = new FastPriorityQueue<TileInfo>(maxNumOfTiles);
            start = null;
        }

        public void FindRange(
            TileTickInfo start, 
            int numOfUnits, 
            int movementRange, 
            IReadOnlyDictionary<GridCoords, TileInfo> graph,
            ref Dictionary<TileTickInfo, TileTickInfo> pathsInRange) {
            this.start = start.TileInfo;
            frontier.Clear();
            cameFrom.Clear();
            costSoFar.Clear();
            forgottenTilesFast.Clear();
            pathsInRange.Clear();

            frontier.Enqueue(this.start, 0);
            cameFrom.Add(this.start, null);
            costSoFar.Add(this.start, start.Tick);

            while (frontier.Count != 0) {
                var current = frontier.Dequeue();

                GetNeighbours(current, graph, costSoFar[current], movementRange, numOfUnits);
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    if (costSoFar.ContainsKey(next))
                        if (newCost >= costSoFar[next])
                            continue;
                    
                    if (forgottenTilesFast.Contains(next))
                        forgottenTilesFast.Remove(next);
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, newCost);
                    cameFrom[next] = current;
                }
            }

            AddForgottenTiles(numOfUnits, movementRange, graph);
            foreach (var pair in cameFrom) {
                pathsInRange.Add(pair.Key.ticks[costSoFar[pair.Key]],
                    pair.Value?.ticks[costSoFar[pair.Value]]);
            }
        }

        private void GetNeighbours(TileInfo currentTileInfo,
                                          IReadOnlyDictionary<GridCoords, TileInfo> graph,
                                          int currentTick, int movementRange, int numOfUnits) {
            
            var tileCords = currentTileInfo.Tile.Coords;
            var nextTick = currentTick + 1;

            neighbours.Clear();

            foreach (var neighbourTransform in GridCoords.NeighbourTransforms) {
                neighbourCoords.SetCoords(tileCords.x + neighbourTransform.x, tileCords.y + neighbourTransform.y);
                if (!graph.TryGetValue(neighbourCoords, out var tile)) continue;
                if (tile.ticks.Count <= nextTick || nextTick >= movementRange) continue;
                if (MAX_NUM_OF_UNITS - tile.ticks[nextTick].units.Count > numOfUnits) {
                    neighbours.Add(tile);
                } else if (tile != start && !forgottenTilesFast.Contains(tile)) {
                    forgottenTilesFast.Enqueue(tile, tile.Coords.DistanceTo(start.Coords));
                }
            }
        }

        private void AddForgottenTiles(int numOfUnits, int movementRange,
                                              IReadOnlyDictionary<GridCoords, TileInfo> graph) {
            if (forgottenTilesFast.Count == 0) return;

            TileInfo bestNeighbour = null;
            var minTileCost = 100;
            TileInfo forgottenTile;

            while (forgottenTilesFast.Count > 0) {
                forgottenTile = forgottenTilesFast.Dequeue(); 
                for (var i = 0; i < 6; i++) {
                    neighbourCoords.SetCoords(forgottenTile.Coords.x + GridCoords.NeighbourTransforms[i].x,
                        forgottenTile.Coords.y + GridCoords.NeighbourTransforms[i].y);
                    if (!graph.TryGetValue(neighbourCoords, out var tile)) continue;
                    if (!costSoFar.TryGetValue(tile, out var tileCost)) continue;
                    if (tileCost >= minTileCost) continue;
                    bestNeighbour = tile;
                    minTileCost = tileCost;
                }

                if (bestNeighbour.IsNull()) continue;

                foreach (var tick in forgottenTile.ticks) {
                    if (MAX_NUM_OF_UNITS - tick.units.Count > numOfUnits && tick.Tick > costSoFar[bestNeighbour] &&
                        tick.Tick <= movementRange) {
                        costSoFar.Add(forgottenTile, tick.Tick);
                        cameFrom.Add(forgottenTile, bestNeighbour);
                        break;
                    }
                }

                bestNeighbour = null;
                minTileCost = 0;
                
            }
        }
    }
}
