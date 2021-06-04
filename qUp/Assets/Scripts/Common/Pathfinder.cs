using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Tiles;
using Actors.Units;
using Base.Singletons;
using Common.PriorityQueues;
using Handlers;

namespace Common {
    public class Pathfinder : SingletonClass<Pathfinder> {

        private const int MAX_NUM_OF_TILES = 200;
        private const int MAX_NUM_OF_NEIGHBOURS = 6;

        private GridCoords neighbourCoords;
        private int maxTick = 5;

        private readonly FastPriorityQueue<ITile> frontier;
        private readonly Dictionary<ITile, int> costSoFar;
        private readonly List<ITile> neighbours;
        private ITile start;

        private IPlayer currentPlayer;

        private readonly FastPriorityQueue<ITile> forgottenTilesFast;

        public Pathfinder(int maxNumOfTiles = MAX_NUM_OF_TILES) {
            neighbourCoords = new GridCoords();
            frontier = new FastPriorityQueue<ITile>(maxNumOfTiles);
            costSoFar = new Dictionary<ITile, int>(maxNumOfTiles);
            neighbours = new List<ITile>(MAX_NUM_OF_NEIGHBOURS);
            forgottenTilesFast = new FastPriorityQueue<ITile>(maxNumOfTiles);
            start = null;
        }

        public static void GetPaths(IPlayer player, ITile start, int startTick, IUnit unit,
                                    ref Dictionary<ITile, ITile> pathsInRange) =>
            Instance.FindPaths(player, start, startTick, unit, ref pathsInRange);

        public static void GetResourcePath(IPlayer player, ITile start, IUnit unit, ref List<ITile> pathsInRange) =>
            Instance.FindResourcePath(player, start, unit, ref pathsInRange);

        /// <summary>
        /// Provides a path for resource unit. Path will be have the origin tile, 1-2 movement tiles and rest (up to max tick)
        /// will be filled with last tile in path.
        /// </summary>
        /// <param name="player">Unit owner</param>
        /// <param name="start">Origin tile</param>
        /// <param name="unit">Resource unit</param>
        /// <param name="pathsInRange"></param>
        private void FindResourcePath(IPlayer player, ITile start, IUnit unit, ref List<ITile> pathsInRange) {
            pathsInRange.Clear();
            var unitPath = start.GetCoords()
                                .PathTo(player.GetHqInfo().coords, GridHandler.MaxCoords.y)
                                .ConvertAll(coords => {
                                    GridHandler.TryGetTile(coords, out var tile);
                                    return tile;
                                })
                                .Take(3) // Take 3 because 1st is starting tile
                                .Where(tile => tile != null)
                                .ToList();
            // We need to remove invalid path parts
            var invalidIndex = -1;
            for (var i = 1; i < unitPath.Count; i++) {
                if (!IsPathOnTickValidForResourceUnitPath(unitPath, unit, i)) {
                    invalidIndex = i;
                    break;
                }
            }

            if (invalidIndex > 0) {
                unitPath.RemoveRange(invalidIndex, unitPath.Count - invalidIndex);
            }

            // We subtract 1 from unitPath because we dont want to subtract 0th tick from max ticks
            var pathTicks = unitPath.Count - 1;
            // Add the last tile as rest of the ticks
            for (var i = 0; i < Configuration.GetMaxTick() - pathTicks; i++) {
                unitPath.Add(unitPath.Last());
            }

            pathsInRange.AddRange(unitPath);
        }

        private static bool IsPathOnTickValidForResourceUnitPath(IReadOnlyList<ITile> path, IUnit unit, int tick) {
            var tileOwner = path[tick].TileOwnershipForTick(tick);
            var hasUnitFromSameOrigin = path[tick]
                                        .GetResourceUnitsForTick(tick)
                                        .Any(tileUnit => tileUnit.GetOriginTile() == unit.GetOriginTile());
            return (tileOwner == unit.Owner || tileOwner == null) && !hasUnitFromSameOrigin;
        }

        private void FindPaths(IPlayer player, ITile start, int startTick, IUnit unit,
                               ref Dictionary<ITile, ITile> pathsInRange) {
            currentPlayer = player;
            this.start = start;
            var movementRange = unit.TickPoints - startTick;


            frontier.Clear();
            costSoFar.Clear();
            forgottenTilesFast.Clear();
            pathsInRange.Clear();

            frontier.Enqueue(this.start, 0);
            pathsInRange.Add(this.start, null);
            costSoFar.Add(this.start, startTick);

            while (frontier.Count != 0) {
                var current = frontier.Dequeue();

                GetNeighbours(current, costSoFar[current], movementRange, unit);
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    if (costSoFar.ContainsKey(next)) {
                        if (newCost >= costSoFar[next]) {
                            continue;
                        }
                    }

                    if (forgottenTilesFast.Contains(next)) {
                        forgottenTilesFast.Remove(next);
                    }

                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, newCost);
                    pathsInRange[next] = current;
                }
            }

            AddForgottenTiles(movementRange, ref pathsInRange);
        }

        private void GetNeighbours(ITile currentTile, int currentTick, int movementRange, IUnit unit) {
            var tileCoords = currentTile.GetCoords();
            var nextTick = currentTick + 1;

            neighbours.Clear();

            foreach (var neighbourTransform in GridCoords.NeighbourTransforms) {
                neighbourCoords.SetCoords(tileCoords.x + neighbourTransform.x, tileCoords.y + neighbourTransform.y);
                if (!GridHandler.TryGetTile(neighbourCoords, out var tile)) continue;
                if (movementRange <= nextTick) continue;
                if (tile.IsAvailableForTick(nextTick, currentPlayer, unit)) {
                    neighbours.Add(tile);
                } else if (tile != start && !forgottenTilesFast.Contains(tile)) {
                    forgottenTilesFast.Enqueue(tile, tile.GetCoords().DistanceTo(start.GetCoords()));
                }
            }
        }

        private void AddForgottenTiles(int movementRange, ref Dictionary<ITile, ITile> pathsInRange) {
            if (forgottenTilesFast.Count == 0) return;

            ITile bestNeighbour = null;
            var minTileCost = int.MaxValue;

            while (forgottenTilesFast.Count > 0) {
                var forgottenTile = forgottenTilesFast.Dequeue();
                if (pathsInRange.ContainsKey(forgottenTile)) continue;
                foreach (var neighbourTransform in GridCoords.NeighbourTransforms) {
                    neighbourCoords.SetCoords(forgottenTile.GetCoords().x + neighbourTransform.x,
                        forgottenTile.GetCoords().y + neighbourTransform.y);
                    if (!GridHandler.TryGetTile(neighbourCoords, out var tile)) continue;
                    if (!costSoFar.TryGetValue(tile, out var tileCost)) continue;
                    if (tileCost >= minTileCost) continue;
                    bestNeighbour = tile;
                    minTileCost = tileCost;
                }

                if (bestNeighbour == null) continue;

                for (var tick = 0; tick < maxTick; tick++) {
                    if (tick > costSoFar[bestNeighbour!] && tick <= movementRange) {
                        costSoFar.Add(forgottenTile, tick);
                        pathsInRange.Add(forgottenTile, bestNeighbour);
                        break;
                    }
                }

                bestNeighbour = null;
                minTileCost = int.MinValue;
            }
        }
    }
}
