using System.Collections.Generic;
using System.Linq;
using Actors.Tiles;
using Actors.Units;
using Base.Managers;
using Common;
using Extensions;
using Managers.GridManager.GridInfos;
using Managers.PlayerManagers;
using UnityEngine;

namespace Managers.GridManager {
    public class GridManager : BaseManager<GridManagerState> {
        private GridCoords maxCoords;

        private Dictionary<GridCoords, TileInfo> grid = new Dictionary<GridCoords, TileInfo>();
        private Dictionary<TileInfo, GridCoords> conflictedTiles = new Dictionary<TileInfo, GridCoords>();
        private Dictionary<Unit, List<TileTickInfo>> unitPath = new Dictionary<Unit, List<TileTickInfo>>();
        
        private List<TileInfo> path;

        private Unit selectedUnit;
        
        private bool isHqSelected;
        private GridCoords hqCoords;

        private Dictionary<TileTickInfo, TileTickInfo> pathsInRange;

        private Pathfinder pathfinder;

        public GridManager() {
            pathsInRange = new Dictionary<TileTickInfo, TileTickInfo>(StaticPathfinder.MAX_NUM_OF_TILES);
            pathfinder = new Pathfinder();
        }

        public void RegisterTile(Tile tile) {
            grid.Add(tile.Coords, new TileInfo(tile.Coords, tile));
        }

        public void RegisterUnit(Unit unit, GridCoords coords) {
            var ticks = grid[coords].ticks;
            //Testing purposes
            // ticks.ForEach(it => it.units.Add(unit));
            ticks[0].units.Add(unit);

            unitPath.Add(unit, new List<TileTickInfo>{ticks[0]});
        }

        public void UnitToSpawnSelected() {
            var currentPlayer = GlobalManager.GetManager<PlayerManager>().GetCurrentPlayer();
            hqCoords = currentPlayer.GetBaseCoordinates();
            
            isHqSelected = true;

            grid.GetValues(hqCoords.GetNeighbourCoordsOfGrid(maxCoords)).ToList()
                .ForEach(it => it.Tile.ApplyMarkings(Color.green));
        }

        public bool HandleHq(GridCoords coords) {
            if (isHqSelected) {
                var isHqNeighbour = coords.IsNeighbourOf(hqCoords);
                if (isHqNeighbour) {
                    GlobalManager.GetManager<PlayerManager>().SpawnUnit(grid[coords].Tile.ProvideTilePosition(), coords);
                }
                
                grid.GetValues(hqCoords.GetNeighbourCoordsOfGrid(maxCoords)).ToList()
                    .ForEach(it => it.Tile.ResetMarkings());
                
                isHqSelected = false;

                return isHqNeighbour;
            }

            return false;
        }

        public void SelectTile(GridCoords coords) {
            // pathOrigin = grid[coords];
            // var originUnits = pathOrigin.ticks.First().units;
            // if (originUnits.Count == 1) {
            //     SetState(new UnitSelected(originUnits.First()));
            // } else {
            //     SetState(new GroupSelected());
            // }
            for (int i = 0; i < 5; i++) {
                grid[coords].ticks[i].units.AddRange(new []{new Unit(), new Unit(), new Unit()});
            }

            if (!HandleHq(coords)) {
                //temp
            }
        }

        public void SelectUnit(Unit unit) {
            selectedUnit = unit;

            var pathOrigin = unitPath[unit][0];
            pathfinder.FindRange(pathOrigin, 1, unit.data.tickPoints, grid, ref pathsInRange);

            foreach (var tileTickInfoPair in pathsInRange) {
                tileTickInfoPair.Key?.TileInfo.Tile.ActivateHighlight(Color.red);
            }

            // SetState(new Test().Also(it=> {
            //     it.gizmos = pathRange.Keys.ToList()
            //                          .ConvertAll(value =>
            //                              (value?.TileInfo.Tile.ProvideTilePosition() ?? Vector3.zero, value?.Tick.ToString() ?? "fail"));
            // }));

            // var originCoords = unitPath[unit][0].TileInfo.Coords;
            // var pathRange = Pathfinder.FindRangeWeighted(originCoords, unit.data.tickPoints);
            //
            // foreach (var coords in pathRange.Values) {
            //     if (coords != null) {
            //         grid.GetOrNull((GridCoords)coords)?.Tile.ActivateHighlight(Color.red);
            //     }
            // }
        }
    }
}
