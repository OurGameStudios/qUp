using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Tiles;
using Actors.Units;
using Base.Managers;
using Common;
using Extensions;
using Managers.ApiManagers;
using Managers.GridManagers.GridInfos;
using Managers.PlayerManagers;
using UnityEngine;

namespace Managers.GridManagers {
    public class GridManager : BaseManager<IGridManagerState> {
        
        public const int MAX_NUM_OF_UNITS = 3;
        public const int MAX_NUM_OF_TILES = 200;
        
        private enum FocusType {
            None,
            HQ,
            InteractableUnit,
            UninteractableUnit
        }

        private Color rangeHighlightColor = Color.red;
        private Color pathHighlightColor = Color.green;

        private readonly Lazy<PlayerManager> playerManagerLazy =
            new Lazy<PlayerManager>(ApiManager.ProvideManager<PlayerManager>);

        private PlayerManager PlayerManager => playerManagerLazy.Value;

        private GridCoords maxCoords;

        private readonly Dictionary<GridCoords, TileInfo> grid = new Dictionary<GridCoords, TileInfo>();
        private Dictionary<TileInfo, GridCoords> conflictedTiles = new Dictionary<TileInfo, GridCoords>();
        private readonly Dictionary<Unit, List<TileTickInfo>> unitPath = new Dictionary<Unit, List<TileTickInfo>>();

        private List<TileInfo> path;

        private GridCoords hqCoords;
        private List<TileInfo> spawnTiles = new List<TileInfo>(2);

        private Dictionary<TileTickInfo, TileTickInfo> pathsInRange;

        private Pathfinder pathfinder;

        private bool isUnitSelected;
        private readonly List<Unit> selectedUnits = new List<Unit>(3);
        private int currentTick = 0;
        private List<TileTickInfo> currentSelectedPath = new List<TileTickInfo>(5);

        private FocusType focusType = FocusType.None;

        public GridManager() {
            pathsInRange = new Dictionary<TileTickInfo, TileTickInfo>(StaticPathfinder.MAX_NUM_OF_TILES);
            pathfinder = new Pathfinder();
        }

        public void RegisterTile(Tile tile) {
            grid.Add(tile.Coords, new TileInfo(tile.Coords, tile));
        }

        public void RegisterUnit(Unit unit, GridCoords coords) {
            var ticks = grid[coords].ticks;

            //TODO spawned unit should take 5 ticks
            ticks[0].units.Add(unit);

            unitPath.Add(unit, new List<TileTickInfo> {ticks[0]});
        }

        public void UnitToSpawnSelected() {
            ClearFocus();
            var currentPlayer = PlayerManager.GetCurrentPlayer();
            hqCoords = currentPlayer.GetBaseCoordinates();

            focusType = FocusType.HQ;

            foreach (var tileInfo in grid.GetValues(GridCoords.GetNeighbourCoords(hqCoords))) {
                if (tileInfo.ticks[0].units.Count >= MAX_NUM_OF_UNITS) continue;
                spawnTiles.Add(tileInfo);
                tileInfo.Tile.ActivateHighlight(color : Color.green);
            }

            if (spawnTiles.Count != 0) return;
            ClearFocus();
        }

        private void HandleHq(GridCoords coords) {
            spawnTiles.FirstOrDefault(it => it.Coords == coords)
                      ?.Tile?.Let(it => PlayerManager.SpawnUnit(it.ProvideTilePosition(), coords));
            
            ClearFocus();
        }

        public void SelectTile(GridCoords coords) {
            if (focusType == FocusType.InteractableUnit) {
                if (SetPath(coords)) return;
            } else if (focusType == FocusType.HQ) {
                HandleHq(coords);
                return;
            }
            
            ClearFocus();
        }

        private int groupRange;

        public void SelectUnit(Unit unit) {
            if (selectedUnits.Contains(unit)) return;
            ClearFocus();
            selectedUnits.AddRange(unitPath[unit][currentTick].units);

            groupRange = selectedUnits.Min(x => x.data.tickPoints);

            //TODO need to check if last is better then inverting the list in pathfinder
            pathsInRange.Clear();
            pathfinder.FindRange(unitPath[unit].Last(), selectedUnits, groupRange, grid, ref pathsInRange);

            ShowUnitRange();
            ShowGroupPaths();

            focusType = FocusType.InteractableUnit;
        }

        private void ShowUnitRange() {
            foreach (var tileTickInfoPair in pathsInRange) {
                tileTickInfoPair.Key?.TileInfo.Tile.ActivateHighlight(color: rangeHighlightColor);
            }
        }

        private void ShowGroupPaths() {
            foreach (var tileTickInfo in selectedUnits.SelectMany(unit => unitPath[unit])) {
                tileTickInfo.TileInfo.Tile.ActivateHighlight(rangeHighlightColor, pathHighlightColor);
            }
        }

        private void ClearFocus() {
            if (focusType == FocusType.None) {
                return;
            }
            if (focusType == FocusType.InteractableUnit) {
                ClearUnitFocus();
            } else if (focusType == FocusType.UninteractableUnit) {
                //TODO for units such as resource carriers
            } else if (focusType == FocusType.HQ) {
                ClearHqFocus();
            }

            focusType = FocusType.None;
        }

        private void ClearHqFocus() {
            foreach (var tileInfo in spawnTiles) {
                tileInfo.Tile.DeactivateHighlight();
            }
            spawnTiles.Clear();
        }

        private bool hasPathChanged;

        private void ClearUnitFocus() {
            //Clear tile highlight in range
            foreach (var tileTickInfo in pathsInRange) {
                tileTickInfo.Key.TileInfo.Tile.DeactivateHighlight();
            }

            //Clears tile highlight on path, this isn't good because we clear some tiles twice. However it needs to be done
            //because we could have added a unit to group which has less ticks and range will be less, so we can't just
            //clear those tiles.
            foreach (var unit in selectedUnits) {
                for (var i = 0; i < unitPath[unit].Count; i++)
                    unitPath[unit][i].TileInfo.Tile.DeactivateHighlight();
            }

            //If path has changed save it
            if (hasPathChanged) SavePath();
            //Clear temp tiles in range
            pathsInRange.Clear();
            //Clear temp tiles in current path
            currentSelectedPath.Clear();

            //TODO if needed hide UI

            selectedUnits.Clear();
        }

        private void ClearUnitPreviousPath() {
            foreach (var unit in selectedUnits) {
                while (unitPath[unit].Count > 0) {
                    if (unitPath[unit].Count - 1 <= groupRange) {
                        unitPath[unit][0].TileInfo.Tile.ActivateHighlight(color: rangeHighlightColor);
                    } else {
                        unitPath[unit][0].TileInfo.Tile.DeactivateHighlight();
                    }

                    unitPath[unit][0].units.Remove(unit);
                    unitPath[unit].RemoveAt(0);
                }
            }

            foreach (var tileTickInfo in currentSelectedPath) {
                tileTickInfo.TileInfo.Tile.ActivateHighlight(color: rangeHighlightColor);
            }

            currentSelectedPath.Clear();
        }

        private void SavePath() {
            foreach (var unit in selectedUnits) {
                unitPath[unit].Repopulate(currentSelectedPath);
            }

            foreach (var tileTickInfo in currentSelectedPath) {
                //Todo this needs to be changed to take overflowing of units into account
                //TODO better logic for this, this is costly
                foreach (var unit in selectedUnits.Where(unit => !tileTickInfo.units.Contains(unit))) {
                    tileTickInfo.units.Add(unit);
                }
            }

            hasPathChanged = false;
        }

        private bool SetPath(GridCoords coords) {
            TileTickInfo target;
            if ((target = pathsInRange.FirstOrDefault(it => it.Key.TileInfo.Tile.Coords == coords).Key) ==
                null) return false;
            ClearUnitPreviousPath();

            var next = target;
            do {
                currentSelectedPath.Add(next);
                next.TileInfo.Tile.ActivateHighlight(rangeHighlightColor, pathHighlightColor);
            } while ((next = pathsInRange[next]) != null);

            hasPathChanged = true;
            return true;
        }
    }
}
