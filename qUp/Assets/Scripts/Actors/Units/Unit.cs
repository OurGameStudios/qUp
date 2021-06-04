using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Grid.Generator;
using Actors.Players;
using Actors.Tiles;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Handlers;
using Handlers.PhaseHandlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Wrappers.Shaders;
using static Common.Constants;

namespace Actors.Units {
    public class Unit : BaseMonoBehaviour<UnitData>, IUnit, IPointerClickHandler, ITickWorker {

        public UnitData Data => data;

        public string UnitName => data.unitName;

        public int TickPoints => data.tickPoints;

        public int Hp => data.hp;

        public IPlayer Owner { get; private set; }

        /// <summary>
        /// Path including 0th tick (starting position)
        /// </summary>
        private readonly List<ITile> path = new List<ITile>();

        public List<ITile> Path => path;

        [SerializeField]
        private MeshRenderer puckRenderer;

        private PuckShader puckShader;

        private ITile currentTile;

        private int UpKeep => data.upkeep;

        private IPlayer combatWinner;

        private Vector3 position;
        private float Speed => 5f;

        private Vector3 moveTo = Vector3.zero;

        private void Awake() {
            puckShader = new PuckShader(puckRenderer.materials[1]);
        }

        public void OnPointerClick(PointerEventData eventData) {
            InteractionHandler.OnUnitSelected(this, currentTile);
        }

        public void SetActive(bool isActive) {
            gameObject.SetActive(isActive);
            // We need to subscribe/unsubscribe manually here because we will be doing it multiple times
            if (isActive) {
                ExecutionHandler.TickDispatch.Subscribe(OnTickDispatched);
                PhaseHandler.PlanningPhase.Subscribe(OnPlanningPhase);
            } else {
                ExecutionHandler.TickDispatch.Unsubscribe(OnTickDispatched);
                PhaseHandler.PlanningPhase.Unsubscribe(OnPlanningPhase);
            }
        }

        public bool IsActive() => gameObject.activeSelf;

        public void SetPosition(Vector3 position) {
            gameObject.transform.position = position;
            this.position = position;
        }

        public void SetTile(ITile tile) => currentTile = tile;

        public void SetOwner(IPlayer owner) {
            Owner = owner;
            combatWinner = Owner;
            puckShader.SetPlayerColor(owner.GetPlayerColor());
        }

        public void SetOriginTile(ITile tile) {
            throw new NotImplementedException();
        }

        public ITile GetOriginTile() => throw new NotImplementedException();

        public void SetPath(List<ITile> path) {
            this.path.Clear();
            this.path.AddRange(path);
            for (var i = path.Count - 1; i < Configuration.GetMaxTick(); i++) {
                path.Add(path.Last());
            }
            // Path contains the 0th tick so we must check if there are more than 1 parts to register some work
            if (path.Count > 1 && path.Any(tile => tile != path[0])) {
                ExecutionHandler.TickWorkerQueued(this);
            }
        }

        public void LostPathAt(int tick) {
            path.RemoveRange(tick, path.Count - tick);
        }

        public int GetDamageFor(IUnit unit) =>
            data.damages.FirstOrDefault(it => it.unitDatas == unit.Data)?.damage ?? 0;

        // Path should be left with one part as the new path origin. If combat winner is null or this then it can
        // continue working
        public bool HasMoreWork() => path.Count > 1;// && !HasLostCombat();

        private void OnPlanningPhase(IPlayer player) {
            if (Owner == player) {
                Owner.DecreaseIncome(UpKeep);
                path[0].OnPathSet(Owner, ZERO_TICK, this, true, true);
                currentTile = path[0];
            }
        }

        /// <summary>
        /// Removes first part of path (origin) and sets the second part of the path (first after removal of origin)
        /// as destination. Registers as tick worker to execution handler. Removes itself from the origin tile.
        /// </summary>
        /// <param name="tick"></param>
        private void OnTickDispatched(int tick) {
            TickCombat(tick);
            if (!HasMoreWork()) {
                if (HasLostCombat()) {
                    IEnumerator Delay() {
                        yield return new WaitForEndOfFrame();
                        OnCombatLost(tick);
                        combatWinner = Owner;
                        UnitPool.ReturnUnit(this);
                        ExecutionHandler.TickWorkerDequeued(this);
                    }

                    StartCoroutine(Delay());
                }
                return;
            }
            // TODO start moving animation
            path[0].RemoveUnitFromTick(Owner, tick - 1, false);
            path.RemoveFirst();
            var tileCenter = path[0].GetTileCenter();
            moveTo.Set(tileCenter.x, tileCenter.y, tileCenter.z);
            StartCoroutine(UnitMovement(tick));
            ExecutionHandler.TickWorkerStarted(this);
        }

        /// <summary>
        /// If there are more than 1 units on a tile on a tick we consider that a combat and instruct CombatHandler
        /// to handle the combat. At the moment it doesn't matter that combat handler is called twice since there
        /// is always only one outcome and the result of combat handler is setting units hasLost check.
        ///
        /// This also signals the tile that combat is occurring on it.
        /// </summary>
        /// <param name="tick"></param>
        private void TickCombat(int tick) {
            // We are taking second tile on path because we want to calculate combat outcome before moving units
            // If unit has no next tile, we are taking current tile
            var combatTile = path[path.Count > 1 ? 1 : 0];
            var combatants = combatTile.GetCombatantUnitsFor(tick);
            if (combatants.Count > 1) {
                combatWinner = CombatHandler.GetCombatWinner(combatants);
                combatTile.CombatOnTile(true);
            }
        }

        private bool HasLostCombat() => combatWinner != Owner;

        /// <summary>
        /// Notifies execution handler that tick worker is finished. Sets position to tiles center.
        /// If hasLostCombat check is true then it removes all future path parts and removes itself from the tile.
        /// If there is single part of path -> notifies that tile that it is there on zero tick.
        /// </summary>
        private void OnTickCompleted(int tick) {
            // TODO should sample terrain and set down
            SetPosition(path[0].GetTileCenter());
            if (HasLostCombat()) {
                path[0].CombatOnTile(false);
                OnCombatLost(tick);
            } else if (path.Count == 1) {
                path[0].OnPathSet(Owner, ZERO_TICK, this, true, true);
                currentTile = path[0];
            } else {
                path[0].OnMovedOver(Owner, tick, this);
                currentTile = path[0];
            }

            if (HasLostCombat()) {
                combatWinner = Owner;
                UnitPool.ReturnUnit(this);
            }

            ExecutionHandler.TickWorkerFinished(this);
        }

        // TODO unit movement should be separated in three stages
        // TODO Should use restartable coroutine, but for some reason it doesn't work here
        // Should be split in pickup -> move -> put down
        // pickup and put down could be done via animations
        private IEnumerator UnitMovement(int tick) {
            yield return this; // delay movement for a turn
            while (Vector3.Distance(transform.position, moveTo) > 1f) {
                TransformLerp(moveTo, Time.deltaTime * Speed);
                RotateLerp(moveTo, Time.deltaTime * Speed);
                yield return this;
            }

            OnTickCompleted(tick);
        }

        private void TransformLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            var x = position.x + (b.x - position.x) * t;
            var z = position.z + (b.z - position.z) * t;
            var y = GridGeneratorHandler.SampleTerrain(x, z);
            position.Set(x, y, z);
            transform.position = position;
        }

        private void RotateLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            var transformPosition = transform.position;
            b.y = transformPosition.y;
            var toRotation = Quaternion.LookRotation(b - transformPosition);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, t);
        }

        private void OnCombatLost(int tick) {
            path[0].CombatResolved(combatWinner);
            for (var i = 0; i < path.Count; i++) {
                path[i].RemoveUnitFromTick(Owner, i + tick, false);
            }
            
            path.Clear();
            currentTile = null;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (gameObject.activeSelf) {
                ExecutionHandler.TickDispatch.Unsubscribe(OnTickDispatched);
                PhaseHandler.PlanningPhase.Unsubscribe(OnPlanningPhase);
            }
        }
    }
}
