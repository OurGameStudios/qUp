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

        private TileInfo pathOrigin;
        private List<TileInfo> path;

        private Unit selectedUnit;
        
        private bool isHqSelected;
        private GridCoords hqCoords;

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

            grid.GetValues(hqCoords.GetNeighbourCoordsOfGrid(maxCoords))
                .ForEach(it => it.Tile.ApplyMarkings(Color.green));
        }

        public bool HandleHq(GridCoords coords) {
            if (isHqSelected) {
                var isHqNeighbour = coords.IsNeighbourOf(hqCoords);
                if (isHqNeighbour) {
                    GlobalManager.GetManager<PlayerManager>().SpawnUnit(grid[coords].Tile.ProvideTilePosition());
                }
                
                grid.GetValues(hqCoords.GetNeighbourCoordsOfGrid(maxCoords))
                    .ForEach(it => it.Tile.ResetMarkings());
                
                isHqSelected = false;

                return isHqNeighbour;
            }

            return false;
        }

        public void SelectTile(GridCoords coords) {
            pathOrigin = grid[coords];
            // var originUnits = pathOrigin.ticks.First().units;
            // if (originUnits.Count == 1) {
            //     SetState(new UnitSelected(originUnits.First()));
            // } else {
            //     SetState(new GroupSelected());
            // }
            if (!HandleHq(coords)) {
                //temp
            }
        }

        public void SelectUnit(Unit unit) {
            selectedUnit = unit;
            
            
        }

        public void SelectPath(GridCoords coords) {
            path = FindPath(pathOrigin.Coords, coords);
        }

        public void SelectFixedPath(GridCoords coords) {
            path = FindPath(path.Last().Coords, coords);
        }

        private List<TileInfo> FindPath(GridCoords inPathOrigin, GridCoords pathTarget) {
            return grid.GetValues(inPathOrigin.PathTo(pathTarget, maxCoords.y));
        }
    }
}
