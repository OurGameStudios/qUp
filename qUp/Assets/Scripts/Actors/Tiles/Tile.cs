using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Grid.Generator;
using Actors.Players;
using Actors.Units;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Handlers;
using Handlers.PhaseHandlers;
using Handlers.PlayerHandlers;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Wrappers.Shaders;

namespace Actors.Tiles {
    // TODO fix hardcoded colors
    public class Tile : BaseListenerMonoBehaviour,
        ITile,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler {

        private enum TileState {
            Idle,
            SpawnSelection,
            UnitsOrigin,
            PathRange,
            PathSet // when a unit is on this tile for the 0th tick
        }

        [SerializeField]
        private bool isResourceTile;

        [SerializeField]
        private ParticleSystem combatSmokeParticles;

        private IPlayer owner;

        public IPlayer Owner {
            get => owner;
            private set {
                owner = value;
                shader?.SetMarkingsColor(value.GetPlayerColor());
            }
        }

        private GridCoords coords;
        private FieldShader shader;
        private TileState state = TileState.Idle;

        private IPlayer playerSpawnTile;

        /// <summary>
        /// For each player (index in a list) a unit (IUnit) per tick (index in inner list)
        /// </summary>
        private readonly List<List<IUnit>> playerUnitsPerTick = new List<List<IUnit>>();

        private readonly List<List<IUnit>> resourceUnitsPerTick = new List<List<IUnit>> {
            new List<IUnit>(), new List<IUnit>(), new List<IUnit>()
        };

        /// <summary>
        /// UI selected unit to spawn
        /// </summary>
        private UnitData uiSelectedSpawnUnit;
        /// <summary>
        /// Unit prepared to spawn on this tile
        /// </summary>
        [CanBeNull]
        private UnitData preparedUnitToSpawn;

        private GameObject unitGhost;

        private Func<Vector2, float> sampleHeight;

        private const float MESSAGE_ELEVATION = 7;

        public float Priority { get; set; }
        public int QueueIndex { get; set; }

        public static ITile Instantiate(GameObject prefab, Transform parent, Vector3 position, GridCoords coords,
                                        Func<Vector2, float> sampleHeight) {
            var tile = Instantiate(prefab, position, Quaternion.identity, parent).GetComponent<Tile>();
            tile.Init(coords, sampleHeight);
            return tile;
        }

        protected virtual void Init(GridCoords coords, Func<Vector2, float> sampleHeight) {
            this.sampleHeight = sampleHeight;
            DisplaceVertices(sampleHeight);
            this.coords = coords;
            shader = new FieldShader(GetComponent<MeshRenderer>().material);
            AddToDispose(PhaseHandler.PlanningPhase.Subscribe(OnPlanningPhase));
            AddToDispose(PhaseHandler.PlayerChange.Subscribe(OnPlayerChange));
            AddToDispose(PhaseHandler.ExecutionPhase.Subscribe(OnExecutionPhase));
            AddToDispose(PhaseHandler.PreppingPhase.Subscribe(OnPreppingPhase));
            // TODO instantiate resource decorator if needed

            for (var i = 0; i < PlayerHandler.GetPlayerCount(); i++) {
                playerUnitsPerTick.Add(new List<IUnit>());
                // Its <= because we have 0th tick or the tick where the unit starts before moving
                for (var j = 0; j <= Configuration.GetMaxTick(); j++) {
                    playerUnitsPerTick[i].Add(null);
                }
            }

            if (isResourceTile) {
                Instantiate(GridGeneratorHandler.GetResourceDecorator()).transform.position = GetTileCenter();
            }
        }

        /// <summary>
        /// Sets playerSpawnTile. This is used for preventing enemy players on stepping on this tile.
        /// </summary>
        /// <param name="forPlayer">Owner of neighbour hq</param>
        public void SetAsSpawnTile(IPlayer forPlayer) {
            playerSpawnTile = forPlayer;
            Owner = playerSpawnTile;
            forPlayer.AddSpawnTile(this);
        }

        private void DisplaceVertices(Func<Vector2, float> sampleHeight) {
            var mesh = GetComponent<MeshFilter>().mesh;
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++) {
                var vertexWorldPosition = transform.TransformPoint(vertices[i]);
                var vertexHeight = sampleHeight.Invoke(vertexWorldPosition.ToVectro2XZ());
                vertices[i] = vertices[i].AddY(vertexHeight);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
            if (combatSmokeParticles != null) {
                combatSmokeParticles.gameObject.transform.position = GetTileCenter().AddY(1f);
            }
        }

        public virtual bool IsAvailableForTick(int tick, IPlayer player, IUnit unit) =>
            (playerUnitsPerTick[player.GetPlayerIndex()][tick] == null ||
                playerUnitsPerTick[player.GetPlayerIndex()][tick] == unit) && !IsSpawnTile();

        public void OnPointerEnter(PointerEventData eventData) {
            if (state == TileState.Idle) {
                shader.SetHighlightOn(true, Color.white);
            } else if (state == TileState.SpawnSelection) {
                // Special case for showing selectedSpawnUnit when unitToSpawn is not null
                shader.SetHighlightOn(true, Color.white);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (state == TileState.Idle) {
                shader.SetHighlightOn(false);
            } else if (state == TileState.SpawnSelection) {
                // Special case for showing selectedSpawnUnit when unitToSpawn is not null
                shader.SetHighlightOn(true, Color.yellow);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (state == TileState.Idle) {
                // no action
                InteractionHandler.OnEmptyTileSelected(this);
            } else if (state == TileState.SpawnSelection) {
                // TODO Special case for showing selectedSpawnUnit when unitToSpawn is not null
                InteractionHandler.OnSpawnTileSelected(this, preparedUnitToSpawn, uiSelectedSpawnUnit);
            } else if (state == TileState.PathRange) {
                GridHandler.SetPath(this);
            } else if (state == TileState.UnitsOrigin) {
                var currentPlayerIndex = PlayerHandler.GetCurrentPlayer().GetPlayerIndex();
                InteractionHandler.OnUnitSelected(playerUnitsPerTick[currentPlayerIndex][0], this);
            }
        }

        public GridCoords GetCoords() => coords;

        public void SpawnUnitSelected(UnitData unit) {
            shader.SetHighlightOn(true, Color.yellow);
            state = TileState.SpawnSelection;
            uiSelectedSpawnUnit = unit;
        }

        public void SpawnTileSelected() {
            shader.SetHighlightOn(false);
            state = TileState.Idle;
            uiSelectedSpawnUnit = null;
        }

        public void SelectedForSpawn(UnitData unitData) {
            if (unitData.cost > playerSpawnTile.AvailableIncome) {
                WorldUi.ShowMessageForPlayer(Localization.NOT_ENOUGH_RESOURCE, GetMessagePosition(), playerSpawnTile);
                return;
            }

            if (unitGhost != null && unitData != preparedUnitToSpawn) {
                UnitPool.ReturnGhost(preparedUnitToSpawn, unitGhost);
            } else if (unitGhost != null && unitData == preparedUnitToSpawn) {
                return;
            }

            unitGhost = UnitPool.TakeGhost(unitData);
            preparedUnitToSpawn = unitData;
            unitGhost.transform.position = GetTileCenter();
        }

        private void OnPlanningPhase(IPlayer player) {
            var playerIndex = PlayerHandler.GetPlayerIndex(player);
            var unitsPerTick = playerUnitsPerTick[playerIndex];
            if (unitsPerTick[0] != null) {
                state = TileState.UnitsOrigin;
                shader.SetHighlightOn(true, Color.gray);
            } else {
                state = TileState.Idle;
                shader.SetHighlightOn(false);
            }
        }

        protected virtual void OnPreppingPhase() {
            if (combatSmokeParticles != null) {
                CombatOnTile(false);
            }
            if (preparedUnitToSpawn != null) {
                UnitPool.ReturnGhost(preparedUnitToSpawn, unitGhost);


                // TODO need to make a check if a unit is already occuping the tile before I can spawn
                var unit = UnitPool.TakeUnit(preparedUnitToSpawn);
                unit.SetPosition(GetTileCenter());
                unit.SetTile(this);
                unit.SetOwner(Owner);
                unit.SetPath(new List<ITile> {this});
                OnPathSet(playerSpawnTile, 0, unit, true, true);
                WorldUi.ShowMessageForPlayer(Localization.SPAWNED_UNIT.Format(unit.UnitName),
                    GetMessagePosition(),
                    Owner);
                PhaseHandler.DelayNewRound();
            }

            if (isResourceTile && Owner != null &&
                !resourceUnitsPerTick
                 .Where(list => list.IsNotEmpty())
                 .Select(list => list[0])
                 .Where(unit => unit != null)
                 .Any(unit => (Tile) unit.GetOriginTile() == this)) {
                var unit = UnitPool.TakeResourceUnit();
                unit.SetPosition(GetTileCenter());
                unit.SetTile(this);
                unit.SetOriginTile(this);
                unit.SetOwner(Owner);
                WorldUi.ShowMessageForPlayer(Localization.SPAWNED_UNIT.Format(unit.UnitName),
                    GetMessagePosition(),
                    Owner);
                PhaseHandler.DelayNewRound();
            }

            // Clear resource units since they will set their on path on execution
            foreach (var resourceUnits in resourceUnitsPerTick) {
                resourceUnits.Clear();
            }

            // Precaution for resetting the tile on prepping phase. 
            foreach (var playerUnitList in playerUnitsPerTick) {
                for (var i = 1; i < playerUnitList.Count; i++) {
                    playerUnitList[i] = null;
                }

                var unit = playerUnitList.First();
                if (unit != null) {
                    unit.SetPath(new List<ITile> {this});
                    SetUnitValidPath(unit.Owner, unit, 0, true);
                }
            }

            preparedUnitToSpawn = null;
            uiSelectedSpawnUnit = null;
            unitGhost = null;
        }

        private void OnPlayerChange(IPlayer player) {
            SetIdleState();
            if (unitGhost != null) {
                UnitPool.ReturnGhost(preparedUnitToSpawn, unitGhost);
            }
        }

        private void OnExecutionPhase() {
            SetIdleState();
        }

        private void SetIdleState() {
            state = TileState.Idle;
            shader.SetHighlightOn(false);
        }

        public void OnPathRange() {
            state = TileState.PathRange;
            shader.SetHighlightOn(true, Color.white);
        }

        public void ShowSetPath() {
            state = TileState.PathSet;
            shader.SetHighlightOn(true, Color.green);
        }

        // TODO bug is here almost certainly: when last tile is filled for the unit
        // Steps to reproduce:
        // 1. Set the path on tiles 1, 2, 3, 4, 5
        // 2. With another unit set path a, b, 5
        // Second unit will override the first units path on tile.
        public void OnPathSet(IPlayer player, int tick, IUnit unit, bool isOrigin, bool isLast) {
            if (!isOrigin) {
                ShowSetPath();
            } else {
                if (Owner != unit.Owner && Owner != null) {
                    WorldUi.ShowMessageForPlayer(Localization.AREA_TAKEN,
                        GetMessagePosition(),
                        unit.Owner,
                        WorldMessageUi.WorldMessageDuration.Long);
                }

                Owner = unit.Owner;
            }

            SetUnitValidPath(player, unit, tick, isLast);
        }

        private void SetUnitValidPath(IPlayer player, IUnit unit, int tick, bool isLast) {
            if (isLast) {
                for (var i = tick; i < playerUnitsPerTick[player.GetPlayerIndex()].Count; i++) {
                    // we need to override previous units path
                    var previousUnit = playerUnitsPerTick[player.GetPlayerIndex()][i];
                    if (previousUnit != unit) {
                        previousUnit?.LostPathAt(tick);
                    }

                    playerUnitsPerTick[player.GetPlayerIndex()][i] = unit;
                }
            } else {
                playerUnitsPerTick[player.GetPlayerIndex()][tick]?.LostPathAt(tick);
                playerUnitsPerTick[player.GetPlayerIndex()][tick] = unit;
            }
        }

        public void OnMovedOver(IPlayer player, int tick, IUnit unit) {
            Owner ??= unit.Owner;
            RemoveUnitFromTick(player, tick, false);
        }

        public void AddResourceUnit(IUnit resourceUnit, int tick) {
            while (resourceUnitsPerTick.Count <= tick) {
                resourceUnitsPerTick.Add(new List<IUnit>());
            }

            if (!resourceUnitsPerTick[tick].Contains(resourceUnit)) {
                resourceUnitsPerTick[tick].Add(resourceUnit);
            }
        }

        public List<IUnit> GetResourceUnitsForTick(int tick) {
            return resourceUnitsPerTick.Count <= tick
                ? new List<IUnit>()
                : resourceUnitsPerTick[tick].Select(unit => unit).ToList();
        }

        public void RemoveResourceUnitForTick(IUnit unit, int tick) {
            if (resourceUnitsPerTick.Count <= tick) return;
            if (!resourceUnitsPerTick[tick].Contains(unit)) return;
            resourceUnitsPerTick[tick].Remove(unit);
        }

        public void ResetPlanningPhaseState(IPlayer player) {
            OnPlanningPhase(player);
        }

        public void RemoveUnitFromTick(IPlayer player, int tick, bool isLast) {
            if (isLast) {
                for (var i = tick; i < playerUnitsPerTick[player.GetPlayerIndex()].Count; i++) {
                    playerUnitsPerTick[player.GetPlayerIndex()][i] = null;
                }
            } else {
                playerUnitsPerTick[player.GetPlayerIndex()][tick] = null;
            }
        }

        public bool IsSpawnTile() => playerSpawnTile != null;

        public List<IUnit> GetCombatantUnitsFor(int tick) =>
            playerUnitsPerTick.Select(list => list.Count > tick ? list[tick] : default)
                              // Check if units current tile is this tile
                              // .Where(unit => unit != default && ReferenceEquals(unit.Path[0], this))
                              .Where(unit => unit != default)
                              .ToList();

        public void ResourceUnitKilled(IUnit unit) {
            WorldUi.ShowMessageForPlayer(Localization.RESOURCE_DENIED, GetMessagePosition(), unit?.Owner);
        }

        public void CombatOnTile(bool isActive) {
            if (isActive) {
                combatSmokeParticles.Play();
            } else {
                combatSmokeParticles.Stop();
            }
        }

        public void CombatResolved(IUnit winner, IUnit loser) {
            WorldUi.ShowMessageForPlayer(
                winner != null ? Localization.COMBAT_PLAYER_WON.Format(winner.Owner) : Localization.COMBAT_DRAW,
                GetMessagePosition(),
                winner?.Owner);
        }

        public Vector3 GetTileCenter() {
            var position = transform.position;
            return position.AddY(sampleHeight.Invoke(position.ToVectro2XZ()));
        }

        public IPlayer TileOwnershipForTick(int tick) {
            var ownerOnTick = Owner;
            for (var i = 0; i <= tick; i++) {
                var units = playerUnitsPerTick.Select(list => list.Count > i ? list[i] : default)
                                              .Where(unit => unit != default)
                                              .ToList();
                if (units.Count == 0) continue;
                // if this is a free tile (not yet owned)
                if (ownerOnTick == null) {
                    // if there are more than 1 units on this tile on tick then calculate the combat results otherwise
                    // take the first unit as owner
                    ownerOnTick = units.Count > 1 ? CombatHandler.GetCombatWinner(units) : units.First().Owner;
                } else {
                    if (units.Count > 1) {
                        var combatWinner = CombatHandler.GetCombatWinner(units);
                        // if there are more than 1 units on this tile on tick and the winners path ends here, than
                        // it is the owner
                        if (units.First(unit => unit.Owner == combatWinner).Path.Count < i) {
                            ownerOnTick = combatWinner;
                        }
                    } else {
                        // if the units path ends here it is the owner
                        if (units.First().Path.Count < i) {
                            ownerOnTick = units.First().Owner;
                        }
                    }
                }
            }

            return ownerOnTick;
        }

        private Vector3 GetMessagePosition() => GetTileCenter().AddY(MESSAGE_ELEVATION);
    }
}
