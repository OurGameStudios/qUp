using System.Collections.Generic;
using System.Linq;
using Actors.Tiles;
using Extensions;
using Managers.GridManager.GridInfos;

namespace Common {
    public static class Pathfinder {
        public static Dictionary<GridCoords, GridCoords?> FindRangeWeighted(GridCoords start, int movementRange) {
            var frontier = new PriorityQueue<int, GridCoords>();
            var cameFrom = new Dictionary<GridCoords, GridCoords?>();
            var costSoFar = new Dictionary<GridCoords, int>();

            frontier.Add(0, start);
            cameFrom.Add(start, null);
            costSoFar.Add(start, 0);

            var safetyBreak = 100;
            var safetyCount = 0;

            while (!frontier.IsEmpty() && safetyCount < safetyBreak) {
                var current = frontier.Pop();

                //If point found break

                //get neighbour cords should return coords with matching tick point// max should be max of grid
                var neighbours = current.GetNeighbourCoords();
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    //newCost > movementRange to limit the range of search
                    if (costSoFar.ContainsKey(next))
                        if (newCost >= costSoFar[next])
                            continue;
                    if (newCost > movementRange + 1) continue;
                    costSoFar.AddOrUpdate(next, newCost);
                    frontier.Add(newCost, next);
                    cameFrom.AddOrUpdate(next, current);
                }

                safetyCount++;
            }

            return cameFrom;
        }

        public static Dictionary<TileTickInfo, TileTickInfo> FindRange(TileTickInfo start, int movementRange,
                                                                       Dictionary<GridCoords, TileInfo> graph) {
            var frontier = new PriorityQueue<int, TileTickInfo>();
            var cameFrom = new Dictionary<TileTickInfo, TileTickInfo>();
            var costSoFar = new Dictionary<TileTickInfo, int>();
            var visited = new HashSet<TileInfo> {start.TileInfo};

            frontier.Add(0, start);
            cameFrom.Add(start, null);
            costSoFar.Add(start, 0);

            var safetyBreak = 100;
            var safetyCount = 0;

            while (!frontier.IsEmpty() && safetyCount < safetyBreak) {
                var current = frontier.Pop();

                var neighbours = GetNeighbours(current, graph);
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    if (costSoFar.ContainsKey(next))
                        if (newCost >= costSoFar[next])
                            continue;
                    if (newCost > movementRange + 1) continue;
                    if (visited.Contains(next.TileInfo)) continue;

                    costSoFar.AddOrUpdate(next, newCost);
                    frontier.Add(newCost, next);
                    cameFrom.AddOrUpdate(next, current);
                    visited.Add(next.TileInfo);
                }

                safetyCount++;
            }

            return cameFrom;
        }

        private static List<TileTickInfo> GetNeighbours(TileTickInfo tileTickInfo,
                                                        Dictionary<GridCoords, TileInfo> graph) {
            var tileCords = tileTickInfo.TileInfo.Coords;
            var nextTick = tileTickInfo.Tick + 1;

            var tileNeighbours = tileCords.GetNeighbourCoords();
            var tileNeighboursInfos = graph.GetValues(tileNeighbours);
            return tileNeighboursInfos.FindAll(it => it.ticks.Count > nextTick)
                                      .ConvertAll(it => it.ticks[nextTick])
                                      .FindAll(it => !it.IsOverflown);
        }


        public static Dictionary<TileTickInfo, TileTickInfo> FindRangeV2(
            TileTickInfo start, int movementRange, Dictionary<GridCoords, TileInfo> graph) {

            var frontier = new PriorityQueue<int, TileInfo>();
            var cameFrom = new Dictionary<TileInfo, TileInfo>();
            var costSoFar = new Dictionary<TileInfo, int>();

            frontier.Add(0, start.TileInfo);
            cameFrom.Add(start.TileInfo, null);
            costSoFar.Add(start.TileInfo, start.Tick);

            var safetyBreak = 100;
            var safetyCount = 0;

            while (!frontier.IsEmpty() && safetyCount < safetyBreak) {
                var current = frontier.Pop();

                var neighbours = GetNeighboursV2(current, graph, costSoFar[current]);
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    if (costSoFar.ContainsKey(next))
                        if (newCost >= costSoFar[next])
                            continue;
                    if (newCost > movementRange) continue;

                    costSoFar.AddOrUpdate(next, newCost);
                    frontier.Add(newCost, next);
                    cameFrom.AddOrUpdate(next, current);
                }


                safetyCount++;
            }

            AddForgottenTiles(start, movementRange, cameFrom, costSoFar, graph);

            return cameFrom.ToDictionary(pair => pair.Key.ticks[costSoFar[pair.Key]], pair => pair.Value?.ticks[costSoFar[pair.Value]]);
        }

        private static List<TileInfo> GetNeighboursV2(TileInfo currentTileInfo, Dictionary<GridCoords, TileInfo> graph,
                                                      int currentTick) {
            var tileCords = currentTileInfo.Tile.Coords;
            var nextTick = currentTick + 1;

            var tileNeighbours = tileCords.GetNeighbourCoords();
            var tileNeighboursInfos = graph.GetValues(tileNeighbours);

            return tileNeighboursInfos.FindAll(it => {
                if (it.ticks.Count > nextTick) return !it.ticks[nextTick].IsOverflown;
                return false;
            });
        }

        private static void AddForgottenTiles(
            TileTickInfo start, int movementRange, Dictionary<TileInfo, TileInfo> cameFrom, Dictionary<TileInfo, int> costSoFar,Dictionary<GridCoords, TileInfo> graph) {

            var tilesInRange = start.TileInfo.Coords.InRange(movementRange - start.Tick);

            var tilesNotInPath = tilesInRange.Where(tile => cameFrom.Keys.FirstOrDefault(it => it.Coords == tile) == null)?.ToList();

            var forgottenTiles = tilesNotInPath.FindAll(graph.ContainsKey)
                                               .ConvertAll(it => graph[it])
                                               .FindAll(it => it.ticks.TrueForAll(tick => !tick.IsOverflown));
            if (forgottenTiles.IsEmpty()) return;

            var orderForgottenTiles = forgottenTiles.OrderBy(it => it.Coords.DistanceTo(start.TileInfo.Coords));

            foreach (var forgottenTile in orderForgottenTiles) {
                var neighbourCoords = forgottenTile.Coords.GetNeighbourCoords();

                var neighbours = graph.GetValues(neighbourCoords);
                var neighbourCosts = costSoFar.GetPairsFromKeys(neighbours).ToList();
                if (neighbourCosts.IsEmpty()) continue;

                var bestNeighbour = neighbourCosts.OrderBy(pair => pair.Value).FirstOrDefault().Key;
                if (bestNeighbour.IsNull()) continue;

                var firstAvailableTick = forgottenTile.ticks.First(tick => !tick.IsOverflown);

                costSoFar.Add(forgottenTile, firstAvailableTick.Tick);
                cameFrom.Add(forgottenTile, bestNeighbour);
            }
        }
    }
}
