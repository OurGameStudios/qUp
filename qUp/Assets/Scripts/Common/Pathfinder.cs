using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Units;
using Actors.Units.Interface;
using Extensions;
using Managers.GridManagers;
using Managers.GridManagers.GridInfos;
using Priority_Queue;

namespace Common {
    public class Pathfinder {

        private GridCoords neighbourCoords;
        private readonly FastPriorityQueue<TileInfo> frontier;
        private readonly Dictionary<TileInfo, TileInfo> cameFrom;
        private readonly Dictionary<TileInfo, int> costSoFar;
        private readonly List<TileInfo> neighbours;
        private TileInfo start;
        private Player currentPlayer;

        private readonly FastPriorityQueue<TileInfo> forgottenTilesFast;

        public Pathfinder(int maxNumOfTiles = GridManager.MAX_NUM_OF_TILES) {
            neighbourCoords = new GridCoords();
            frontier = new FastPriorityQueue<TileInfo>(maxNumOfTiles);
            cameFrom = new Dictionary<TileInfo, TileInfo>(maxNumOfTiles);
            costSoFar = new Dictionary<TileInfo, int>(maxNumOfTiles);
            neighbours = new List<TileInfo>(6);
            forgottenTilesFast = new FastPriorityQueue<TileInfo>(maxNumOfTiles);
            start = null;
        }

        public void FindRange(
            Player player,
            TileTickInfo start,
            List<IUnit> units,
            int movementRange,
            IReadOnlyDictionary<GridCoords, TileInfo> graph,
            ref Dictionary<TileTickInfo, TileTickInfo> pathsInRange) {
            currentPlayer = player;
            movementRange += 1;
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

                GetNeighbours(current, units, graph, costSoFar[current], movementRange);
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

            AddForgottenTiles(units.Count, movementRange, graph);
            foreach (var pair in cameFrom) {
                pathsInRange.Add(pair.Key.ticks[costSoFar[pair.Key]],
                    pair.Value?.ticks[costSoFar[pair.Value]]);
            }
        }

        private void GetNeighbours(TileInfo currentTileInfo, List<IUnit> units,
                                   IReadOnlyDictionary<GridCoords, TileInfo> graph,
                                   int currentTick, int movementRange) {
            var tileCords = currentTileInfo.Tile.Coords;
            var nextTick = currentTick + 1;

            neighbours.Clear();

            foreach (var neighbourTransform in GridCoords.NeighbourTransforms) {
                neighbourCoords.SetCoords(tileCords.x + neighbourTransform.x, tileCords.y + neighbourTransform.y);
                if (!graph.TryGetValue(neighbourCoords, out var tile)) continue;
                if (tile.ticks.Count <= nextTick || nextTick >= movementRange) continue;
                if (GetNumberOfUnitsOnTileTick(units, tile.ticks[nextTick]) >= units.Count) {
                    neighbours.Add(tile);
                } else if (tile != start && !forgottenTilesFast.Contains(tile)) {
                    forgottenTilesFast.Enqueue(tile, tile.Coords.DistanceTo(start.Coords));
                }
            }
        }

        private int GetNumberOfUnitsOnTileTick(List<IUnit> unitsOnTile, TileTickInfo tileTickInfo) {
            return GridManager.MAX_NUM_OF_UNITS - unitsOnTile.Aggregate(tileTickInfo.GetUnitCount(currentPlayer),
                (current, t) => current - (tileTickInfo.ContainsUnit(currentPlayer, t) ? 1 : 0));
        }

        private void AddForgottenTiles(int numOfUnits, int movementRange,
                                       IReadOnlyDictionary<GridCoords, TileInfo> graph) {
            if (forgottenTilesFast.Count == 0) return;

            TileInfo bestNeighbour = null;
            var minTileCost = 100;
            TileInfo forgottenTile;

            while (forgottenTilesFast.Count > 0) {
                forgottenTile = forgottenTilesFast.Dequeue();
                if (cameFrom.Keys.Contains(forgottenTile)) continue;
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
                    if (GridManager.MAX_NUM_OF_UNITS - tick.GetUnitCount(currentPlayer) >= numOfUnits && tick.Tick > costSoFar[bestNeighbour] &&
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
