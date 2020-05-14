using System.Collections.Generic;
using Extensions;
using Managers.GridManagers.GridInfos;
using Priority_Queue;
using UnityEngine;

namespace Common {
    public static class StaticPathfinder {
        public const int MAX_NUM_OF_UNITS = 3;
        public const int MAX_NUM_OF_TILES = 200;

        private static GridCoords _neighbourCoords;
        private static FastPriorityQueue<TileInfo> _frontier;
        private static Dictionary<TileInfo, TileInfo> _cameFrom;
        private static Dictionary<TileInfo, int> _costSoFar;
        private static List<TileInfo> _neighbours;
        private static TileInfo _start;

        private static FastPriorityQueue<TileInfo> _forgottenTilesFast;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializePathFinder() {
            _neighbourCoords = new GridCoords();
            _frontier = new FastPriorityQueue<TileInfo>(MAX_NUM_OF_TILES);
            _cameFrom = new Dictionary<TileInfo, TileInfo>(MAX_NUM_OF_TILES);
            _costSoFar = new Dictionary<TileInfo, int>(MAX_NUM_OF_TILES);
            _neighbours = new List<TileInfo>(6);
            _start = null;
            _forgottenTilesFast = new FastPriorityQueue<TileInfo>(MAX_NUM_OF_TILES);
        }

        public static void FindRange(
            TileTickInfo start, 
            int numOfUnits, 
            int movementRange, 
            IReadOnlyDictionary<GridCoords, TileInfo> graph,
            ref Dictionary<TileTickInfo, TileTickInfo> pathsInRange) {
            _start = start.TileInfo;
            _frontier.Clear();
            _cameFrom.Clear();
            _costSoFar.Clear();
            _forgottenTilesFast.Clear();
            pathsInRange.Clear();

            _frontier.Enqueue(_start, 0);
            _cameFrom.Add(_start, null);
            _costSoFar.Add(_start, start.Tick);

            while (_frontier.Count != 0) {
                var current = _frontier.Dequeue();

                GetNeighbours(current, graph, _costSoFar[current], movementRange, numOfUnits);
                foreach (var next in _neighbours) {
                    var newCost = _costSoFar[current] + 1;

                    if (_costSoFar.ContainsKey(next))
                        if (newCost >= _costSoFar[next])
                            continue;
                    
                    if (_forgottenTilesFast.Contains(next))
                        _forgottenTilesFast.Remove(next);
                    _costSoFar[next] = newCost;
                    _frontier.Enqueue(next, newCost);
                    _cameFrom[next] = current;
                }
            }

            AddForgottenTiles(numOfUnits, movementRange, graph);
            foreach (var pair in _cameFrom) {
                pathsInRange.Add(pair.Key.ticks[_costSoFar[pair.Key]],
                    pair.Value?.ticks[_costSoFar[pair.Value]]);
            }
        }

        private static void GetNeighbours(TileInfo currentTileInfo,
                                          IReadOnlyDictionary<GridCoords, TileInfo> graph,
                                          int currentTick, int movementRange, int numOfUnits) {
            
            var tileCords = currentTileInfo.Tile.Coords;
            var nextTick = currentTick + 1;

            _neighbours.Clear();

            foreach (var neighbourTransform in GridCoords.NeighbourTransforms) {
                _neighbourCoords.SetCoords(tileCords.x + neighbourTransform.x, tileCords.y + neighbourTransform.y);
                if (!graph.TryGetValue(_neighbourCoords, out var tile)) continue;
                if (tile.ticks.Count <= nextTick || nextTick >= movementRange) continue;
                if (MAX_NUM_OF_UNITS - tile.ticks[nextTick].units.Count > numOfUnits) {
                    _neighbours.Add(tile);
                } else if (tile != _start && !_forgottenTilesFast.Contains(tile)) {
                    _forgottenTilesFast.Enqueue(tile, tile.Coords.DistanceTo(_start.Coords));
                }
            }
        }

        private static void AddForgottenTiles(int numOfUnits, int movementRange,
                                              IReadOnlyDictionary<GridCoords, TileInfo> graph) {
            if (_forgottenTilesFast.Count == 0) return;

            TileInfo bestNeighbour = null;
            var minTileCost = 100;
            TileInfo forgottenTile;

            while (_forgottenTilesFast.Count > 0) {
                forgottenTile = _forgottenTilesFast.Dequeue(); 
                for (var i = 0; i < 6; i++) {
                    _neighbourCoords.SetCoords(forgottenTile.Coords.x + GridCoords.NeighbourTransforms[i].x,
                        forgottenTile.Coords.y + GridCoords.NeighbourTransforms[i].y);
                    if (!graph.TryGetValue(_neighbourCoords, out var tile)) continue;
                    if (!_costSoFar.TryGetValue(tile, out var tileCost)) continue;
                    if (tileCost >= minTileCost) continue;
                    bestNeighbour = tile;
                    minTileCost = tileCost;
                }

                if (bestNeighbour.IsNull()) continue;

                foreach (var tick in forgottenTile.ticks) {
                    if (MAX_NUM_OF_UNITS - tick.units.Count > numOfUnits && tick.Tick > _costSoFar[bestNeighbour] &&
                        tick.Tick <= movementRange) {
                        _costSoFar.Add(forgottenTile, tick.Tick);
                        _cameFrom.Add(forgottenTile, bestNeighbour);
                        break;
                    }
                }

                bestNeighbour = null;
                minTileCost = 0;
                
            }
        }
    }
}
