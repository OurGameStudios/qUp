using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Grid.Generator;
using Actors.Players;
using Actors.Tiles;
using Actors.Units.Interface;
using Base.Managers;
using Common;
using Extensions;
using Managers.ApiManagers;
using Managers.GridManagers.GridInfos;
using Managers.InputManagers;
using Managers.PlayerManagers;
using Managers.PlayManagers;
using Managers.UIManagers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers.GridManagers {
    public class GridManager : BaseManager<IGridManagerState> {
        public const int MAX_NUM_OF_UNITS = 3;
        public const int MAX_NUM_OF_TILES = 200;
        public const int MAX_TICKS = 5;

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

        private readonly Lazy<PlayManager> playManagerLazy =
            new Lazy<PlayManager>(ApiManager.ProvideManager<PlayManager>);

        private PlayManager PlayManager => playManagerLazy.Value;

        private readonly Lazy<CoroutineHandler> coroutineHandler =
            new Lazy<CoroutineHandler>(ApiManager.ProvideManager<CoroutineHandler>);

        private CoroutineHandler CoroutineHandler => coroutineHandler.Value;

        private readonly Lazy<InputManagerBehaviour> inputManager =
            new Lazy<InputManagerBehaviour>(ApiManager.ProvideManager<InputManagerBehaviour>);

        private InputManagerBehaviour InputManager => inputManager.Value;

        private readonly Lazy<UiManager> uiManagerLazy =
            new Lazy<UiManager>(ApiManager.ProvideManager<UiManager>);

        private UiManager UiManager => uiManagerLazy.Value;

        private GridInteractor gridInteractor = ApiManager.ProvideInteractor<GridInteractor>();

        private GridCoords maxCoords;

        private readonly Dictionary<GridCoords, TileInfo> grid = new Dictionary<GridCoords, TileInfo>(MAX_NUM_OF_TILES);

        private Dictionary<TileInfo, GridCoords> conflictedTiles =
            new Dictionary<TileInfo, GridCoords>(MAX_NUM_OF_TILES);

        private readonly Dictionary<IUnit, List<TileTickInfo>> unitPath =
            new Dictionary<IUnit, List<TileTickInfo>>(MAX_NUM_OF_TILES);

        private readonly Dictionary<IUnit, Player> playerUnits = new Dictionary<IUnit, Player>(MAX_NUM_OF_TILES);

        private readonly Dictionary<Player, List<TileInfo>> playersResourceTiles =
            new Dictionary<Player, List<TileInfo>>(MAX_NUM_OF_TILES);

        private readonly Dictionary<IUnit, ResourceUnitInfo> resourceUnits =
            new Dictionary<IUnit, ResourceUnitInfo>(MAX_NUM_OF_TILES);

        private List<TileInfo> path;

        private GridCoords hqCoords;
        private readonly List<TileInfo> spawnTiles = new List<TileInfo>(2);

        private Dictionary<TileTickInfo, TileTickInfo> pathsInRange;

        private readonly Pathfinder pathfinder;

        private bool isUnitSelected;
        private readonly List<IUnit> selectedUnits = new List<IUnit>(MAX_NUM_OF_UNITS);
        private int currentTick;
        private readonly List<TileTickInfo> currentSelectedPath = new List<TileTickInfo>(MAX_TICKS);

        private FocusType focusType = FocusType.None;

        public GridManager() {
            pathsInRange = new Dictionary<TileTickInfo, TileTickInfo>(MAX_NUM_OF_TILES);
            pathfinder = new Pathfinder();
            for (var i = 0; i <= MAX_TICKS; i++) {
                executions.Add(i, new HashSet<TileTickInfo>());
            }
        }

        public void RegisterTile(Tile tile) {
            grid.Add(tile.Coords, new TileInfo(tile.Coords, tile, PlayerManager.GetAllPlayers()));
            //TODO max cords shouldn't be set each time new tile is registered
            maxCoords = gridInteractor.GetMaxCoords();

            //TODO needs improvement
            foreach (var player in PlayerManager.GetAllPlayers()) {
                var hqCoords = player.GetBaseCoordinates();
                if (hqCoords.x > maxCoords.x) {
                    hqCoords.x = maxCoords.x;
                }

                if (hqCoords.y > maxCoords.y) {
                    hqCoords.y = maxCoords.y;
                }

                if (tile.Coords.IsNeighbourOf(hqCoords)) {
                    tile.SetOwnership(player);
                }
            }
        }

        public void RegisterUnit(IUnit unit, GridCoords coords) {
            var ticks = grid[coords].ticks;

            //TODO spawned unit should take 5 ticks
            //TODO wrong units should spawn on prepping phase
            ticks[0].AddUnit(PlayerManager.GetCurrentPlayer(), unit);
            PlayerManager.GetCurrentPlayer().RegisterUnitUpkeep(unit.GetUpkeep(), unit.GetCost());
            UiManager.RegisterUnitSpawned();

            unitPath.Add(unit, new List<TileTickInfo> {ticks[0]});
            playerUnits.Add(unit, PlayerManager.GetCurrentPlayer());
            unit.SetUnitColor(PlayerManager.GetCurrentPlayer().PlayerColor);
        }

        public void RegisterResourceUnit(IUnit unit, GridCoords coords) {
            var resourceUnitInfo = new ResourceUnitInfo(coords);
            resourceUnits[unit] = resourceUnitInfo;

            var ticks = grid[coords].ticks;
            foreach (var tick in ticks) {
                tick.AddResourceUnit(resourceUnitInfo);
            }
            
            unit.SetUnitColor(PlayerManager.GetCurrentPlayer().PlayerColor);
        }

        public void UnitToSpawnSelected() {
            ClearFocus();
            var currentPlayer = PlayerManager.GetCurrentPlayer();
            hqCoords = currentPlayer.GetBaseCoordinates();
            if (hqCoords.x > maxCoords.x) {
                hqCoords.x = maxCoords.x;
            }

            if (hqCoords.y > maxCoords.y) {
                hqCoords.y = maxCoords.y;
            }

            focusType = FocusType.HQ;

            //TODO this is bad code adding spawn tiles each time unit to spawn is selected
            foreach (var tileInfo in grid.GetValues(GridCoords.GetNeighbourCoords(hqCoords))) {
                if (tileInfo.ticks[0].GetUnitCount(PlayerManager.GetCurrentPlayer()) >= MAX_NUM_OF_UNITS) continue;
                spawnTiles.Add(tileInfo);
                tileInfo.Tile.ActivateHighlight(Color.green);
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
            if (focusType == FocusType.HQ) {
                HandleHq(coords);
                return;
            }

            if (grid[coords].ticks[currentTick].GetUnitCount(PlayerManager.GetCurrentPlayer()) > 0) {
                SelectUnit(grid[coords].ticks[currentTick].GetUnits(PlayerManager.GetCurrentPlayer())[0]);
                return;
            }

            InputManager.OnUnitDeselected();
            ClearFocus();
        }

        public void SelectUnitPath(GridCoords coords) {
            if (focusType == FocusType.InteractableUnit) {
                SetPath(coords);
            }
        }

        private int groupRange;

        //TODO if unit is not from current player, UI should be notified to display
        public void SelectUnit(IUnit unit) {
            if (selectedUnits.Contains(unit) || playerUnits[unit] != PlayerManager.GetCurrentPlayer()) return;
            ClearFocus();
            selectedUnits.AddRange(unitPath[unit][currentTick].GetUnits(PlayerManager.GetCurrentPlayer()));

            //TODO this method needs to notify UI to display selected group UI
            InputManager.OnUnitSelected();

            groupRange = selectedUnits.Min(x => x.GetTickPoints());

            //TODO need to check if last is better then inverting the list in pathfinder
            pathsInRange.Clear();
            pathfinder.FindRange(PlayerManager.GetCurrentPlayer(),
                unitPath[unit].Last(),
                selectedUnits,
                groupRange,
                grid,
                ref pathsInRange);

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
                tileTickInfo.TileInfo.Tile.ActivateHighlight(pathHighlightColor);
            }
        }

        private void ClearFocus() {
            if (focusType == FocusType.None) return;

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
                unit.DeactivateHighlight();
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

                    unitPath[unit][0].RemoveUnit(PlayerManager.GetCurrentPlayer(), unit);
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
                foreach (var unit in selectedUnits.Where(unit =>
                    !tileTickInfo.ContainsUnit(PlayerManager.GetCurrentPlayer(), unit))) {
                    tileTickInfo.AddUnit(PlayerManager.GetCurrentPlayer(), unit);
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
                next.TileInfo.Tile.ActivateHighlight(pathHighlightColor);
            } while ((next = pathsInRange[next]) != null);

            hasPathChanged = true;
            return true;
        }

        private readonly List<IUnit> dispatchedUnitList = new List<IUnit>(200);

        private readonly Dictionary<int, HashSet<TileTickInfo>> executions =
            new Dictionary<int, HashSet<TileTickInfo>>(200);

        private bool hasPaths;

        public void SetupForNextPlayer() {
            ClearFocus();
        }

        public void StartExecution() {
            ClearFocus();
            hasPaths = false;
            foreach (var unitPathPair in unitPath) {
                foreach (var t in unitPathPair.Value) {
                    executions[t.Tick].Add(t);
                    hasPaths = true;
                }
            }

            if (!hasPaths) PlayManager.NextPhase();
            else {
                DispatchTickToUnits();
            }
        }

        private void EndExecution() {
            currentTick = 0;
            for (var i = 0; i <= MAX_TICKS; i++) {
                foreach (var tileTickInfo in executions[i]) {
                    tileTickInfo.ClearUnits();
                }

                executions[i].Clear();
            }

            foreach (var unitPathPair in unitPath) {
                unitPathPair.Value.Repopulate(unitPathPair.Value[0].TileInfo.ticks[0]);
                unitPathPair.Value[0].TileInfo.ticks[0].AddUnit(playerUnits[unitPathPair.Key], unitPathPair.Key);
            }

            PlayManager.NextPhase();
        }

        private void DispatchTickToUnits() {
            currentTick++;
            if (currentTick > MAX_TICKS) {
                EndExecution();
                return;
            }

            foreach (var tileTickInfo in executions[currentTick]) {
                foreach (var unit in tileTickInfo.GetUnits()) {
                    unit.MoveToNextTile(tileTickInfo.TileInfo.Tile.ProvideTilePosition(), tileTickInfo.IsCombatTile());
                    dispatchedUnitList.Add(unit);
                }
            }

            if (dispatchedUnitList.Count == 0) EndExecution();
        }

        public void UnitMovementCompleted(IUnit unit) {
            dispatchedUnitList.Remove(unit);
            ChangeTileOwnership(unit);
            if (dispatchedUnitList.Count == 0) {
                CoroutineHandler.DoOnNextFrame(DispatchTickToUnits);
            }
        }

        private void ChangeTileOwnership(IUnit unit) {
            var tileInfo = unitPath[unit][unitPath[unit].Count - 1 - currentTick].TileInfo;
            var owner = playerUnits[unit];
            if (tileInfo.Tile.GetOwner() == null) {
                RegisterTileOwnership(tileInfo, owner);
            } else if (tileInfo.Tile.GetOwner() != owner && currentTick >= unitPath[unit].Count - 1) {
                RegisterTileOwnership(tileInfo, owner);
            }
        }

        private void RegisterTileOwnership(TileInfo tileInfo, Player owner) {
            if (tileInfo.Tile.IsResourceTile) {
                if (!playersResourceTiles.ContainsKey(owner))
                    playersResourceTiles.Add(owner, new List<TileInfo>());
                if (tileInfo.owner != null)
                    playersResourceTiles[tileInfo.owner].Remove(tileInfo);

                playersResourceTiles[owner].Add(tileInfo);
            }

            tileInfo.Tile.SetOwnership(owner);
            tileInfo.owner = owner;
        }

        public void StartPrepping() {
            foreach (var player in PlayerManager.GetAllPlayers()) {
                if (playersResourceTiles.ContainsKey(player)) {
                    foreach (var resourceTile in playersResourceTiles[player]) {
                        if (!resourceTile.ticks.Any(it => it.ContaisOriginatedResourceUnit(resourceTile.Coords))) {
                            PlayerManager.SpawnResourceUnit(resourceTile.Tile.ProvideTilePosition(),
                                resourceTile.Coords);
                        }
                    }
                }
            }

            PlayManager.NextPhase();
        }
    }
}
