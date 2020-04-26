using System.Collections.Generic;
using System.Linq;
using Actors.PlayerBases;
using Actors.Tiles;
using Actors.Units;
using Base.Managers;
using Common;
using Extensions;
using Managers.GridManager.GridInfos;
using Managers.PlayerManagers;
using Managers.UIManagers;
using UnityEngine;

namespace Managers.GridManager {
    public class GridManager : BaseManager<GridManagerState> {
        private GridCoords maxCoords;

        private Dictionary<GridCoords, TileInfo> grid = new Dictionary<GridCoords, TileInfo>();
        private Dictionary<TileInfo, GridCoords> conflictedTiles = new Dictionary<TileInfo, GridCoords>();
        private Dictionary<Unit, TileTickInfo> unitPath = new Dictionary<Unit, TileTickInfo>();

        private TileInfo pathOrigin;
        private List<TileInfo> path;

        private Unit selectedUnit;
        private PlayerBase selectedBase;

        public void RegisterTile(Tile tile) {
            grid.Add(tile.Coords, new TileInfo(tile.Coords, tile));
        }

        public void SelectBase(PlayerBase playerBase) {
            var currentPlayer = GlobalManager.GetManager<PlayerManager>().GetCurrentPlayer();
            if (playerBase.Owner == currentPlayer) {
                GlobalManager.GetManager<UiManager>().SetSelectedItem(playerBase);
                selectedBase = playerBase;
                SetState(new BaseSelected());
                playerBase.SetSelected(true);
                grid.GetValues(playerBase.Coords.GetNeighbourCoordsOfGrid(maxCoords))
                    .ForEach(it => it.Tile.SetHighlighted(Color.white));
            }
        }

        public void SelectTile(GridCoords coords) {
            pathOrigin = grid[coords];
            var originUnits = pathOrigin.ticks.First().units;
            if (originUnits.Count == 1) {
                SetState(new UnitSelected(originUnits.First()));
            } else {
                SetState(new GroupSelected());
            }
        }

        public void SelectPath(GridCoords coords) {
            path = FindPath(pathOrigin.Coords, coords);
        }

        public void SelectFixedPath(GridCoords coords) {
            path = FindPath(path.Last().Coords, coords);
        }

        private List<TileInfo> FindPath(GridCoords pathOrigin, GridCoords pathTarget) {
            return grid.GetValues(pathOrigin.PathTo(pathTarget, maxCoords.y));
        }
    }
}
