using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Tiles;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Handlers;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Units {
    public class ResourceUnit : BaseMonoBehaviour<UnitData>, IUnit, ITickWorker {

        public UnitData Data => data;

        public string UnitName => data.unitName;

        public int TickPoints => data.tickPoints;

        public int Hp => data.hp;

        public IPlayer Owner { get; private set; }

        private List<ITile> path = new List<ITile>();

        public List<ITile> Path => path;

        [SerializeField]
        private MeshRenderer puckRenderer;

        private PuckShader puckShader;

        private int Cost => data.cost;

        public bool hasLostCombat;

        public bool reachedLastPathTile;

        private Vector3 position;

        private ITile currentTile;

        private ITile originTile;

        private static float Speed => 5f;

        private Vector3 moveTo = Vector3.zero;

        private void Awake() {
            puckShader = new PuckShader(puckRenderer.materials[1]);
            gameObject.name = GetInstanceID().ToString();
        }

        public void SetActive(bool isActive) {
            gameObject.SetActive(isActive);
            // We need to subscribe/unsubscribe manually here because we will be doing it multiple times
            if (isActive) {
                ExecutionHandler.TickDispatch.Subscribe(OnTickDispatched);
                reachedLastPathTile = false;
                hasLostCombat = false;
            } else {
                ExecutionHandler.TickDispatch.Unsubscribe(OnTickDispatched);
                ResourceUnitsHandler.RemoveFromActiveUnits(this);
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
            puckShader.SetPlayerColor(owner.GetPlayerColor());
        }

        public void SetOriginTile(ITile tile) {
            originTile = tile;
            currentTile = originTile;
            ResourceUnitsHandler.AddToActiveUnits(this);
        }

        public ITile GetOriginTile() => originTile;

        public void SetPath(List<ITile> path) {
            // Resource unit take their on path at the moment
        }

        public void LostPathAt(int tick) {
            // Resource units can't lose path due to other units
        }

        public int GetDamageFor(IUnit unit) => 0;

        public bool HasMoreWork() => path.Count > 1 && !hasLostCombat && !reachedLastPathTile;

        private void OnTickDispatched(int tick) {
            if (!HasMoreWork()) {
                ExecutionHandler.TickWorkerDequeued(this);
                return;
            }

            TickCombat(tick);
            path[0].RemoveResourceUnitForTick(this, tick - 1);
            path.RemoveFirst();
            var tileCenter = path[0].GetTileCenter();
            moveTo.Set(tileCenter.x, tileCenter.y, tileCenter.z);
            if (path[0] != currentTile) {
                StartCoroutine(UnitMovement(tick));
                ExecutionHandler.TickWorkerStarted(this);
            } else {
                reachedLastPathTile = true;
                ExecutionHandler.TickWorkerDequeued(this);
                OnTickCompleted(tick);
            }
        }

        private void TickCombat(int tick) {
            var combatants = path.First().GetCombatantUnitsFor(tick);
            if (combatants.Count > 1) {
                hasLostCombat = CombatHandler.GetCombatWinner(combatants) != Owner;
            }
        }

        private void OnTickCompleted(int tick) {
            var needsToDeclareWorkFinished = path[0] != currentTile;
            SetPosition(path[0].GetTileCenter());
            
            if (hasLostCombat) {
                path[0].ResourceUnitKilled(this);
            }
            
            if (path[0].IsSpawnTile()) {
                OnResourceTransfer();
                // we are setting that this unit lost combat here since we want to remove it in
                // the same way as if it was defeated in combat
                hasLostCombat = true;
            }

            if (hasLostCombat) {
                for (var i = 0; i < path.Count; i++) {
                    path[i].RemoveResourceUnitForTick(this, tick + i);
                }

                path.Clear();
                currentTile = null;
                needsToDeclareWorkFinished = true;
                ExecutionHandler.TickWorkerDequeued(this);
            } else if (path.Count == 1) {
                currentTile = path[0];
            } else {
                currentTile = path[0];
            }

            if (hasLostCombat) {
                hasLostCombat = false;
                UnitPool.ReturnResourceUnit(this);
            }

            if (needsToDeclareWorkFinished) {
                ExecutionHandler.TickWorkerFinished(this);
            }
        }

        private IEnumerator UnitMovement(int tick) {
            while (Vector3.Distance(transform.position, moveTo) > 1f) {
                TransformLerp(moveTo, Time.deltaTime * Speed);
                RotateLerp(moveTo, Time.deltaTime * Speed);
                yield return this;
            }

            OnTickCompleted(tick);
        }

        private void TransformLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            position.Set(position.x + (b.x - position.x) * t, b.y, position.z + (b.z - position.z) * t);
            transform.position = position;
        }

        private void RotateLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            var transformPosition = transform.position;
            b.y = transformPosition.y;
            var toRotation = Quaternion.LookRotation(b - transformPosition);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, t);
        }

        public void PlanPath() {
            reachedLastPathTile = false;
            Pathfinder.GetResourcePath(Owner, currentTile, this, ref path);
            currentTile = path[0];

            for (var i = 0; i < path.Count; i++) {
                path[i].AddResourceUnit(this, i);
            }

            if (path.Any(tile => tile != currentTile)) {
                ExecutionHandler.TickWorkerQueued(this);
            }
        }

        private void OnResourceTransfer() {
            Owner.IncreaseIncome(Cost);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if (gameObject.activeSelf) {
                ExecutionHandler.TickDispatch.Unsubscribe(OnTickDispatched);
            }
        }
    }
}
