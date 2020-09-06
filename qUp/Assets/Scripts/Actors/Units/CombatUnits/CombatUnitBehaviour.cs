using System.Collections;
using Actors.Grid.Generator;
using Actors.Players;
using Base.MonoBehaviours;
using Extensions;
using Managers.ApiManagers;
using Managers.GridManagers;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Units.CombatUnits {
    public class CombatUnitBehaviour : BaseMonoBehaviour<CombatUnit, ICombatUnitState> {
        private GridManager gridManager = ApiManager.ProvideManager<GridManager>();
        private GridInteractor gridInteractor = ApiManager.ProvideInteractor<GridInteractor>();

        public UnitData data;
        public UnitShader unitShader;
        public PuckShader puckShader;

        public Vector3 moveTo;

        private IEnumerator unitMovement;
        public float speed = 5f;

        //TODO remove also!
        public static CombatUnit Instantiate(UnitData data, Vector3 position, Player player) {
            return Instantiate(data.prefab, position, Quaternion.identity)
                   .GetComponent<CombatUnitBehaviour>()
                   .Also(it => it.puckShader.SetPlayerColor(player.PlayerColor))
                   .Controller;
        }

        protected override void OnAwake() {
            Controller.Init(data, gameObject);
            unitShader = new UnitShader(transform.GetComponent<MeshRenderer>().material);
            puckShader = new PuckShader(transform.GetChild(1).GetComponent<MeshRenderer>().materials[1]);
            unitMovement = UnitMovement();
            position = transform.position;
        }

        protected override void OnStateHandler(ICombatUnitState inState) {
            if (inState is CombatUnitSelected) {
                OnSelected();
            } else if (inState is CombatUnitMovement unitMovementState) {
                moveTo = unitMovementState.Position;
                StartCoroutine(unitMovement);
            } else if (inState is CombatUnitOwnership ownershipState) {
                unitShader.SetUnitPlayerColor(ownershipState.Color);
            } else if (inState is Highlight highlightState) {
                if (highlightState.IsHighlightOn) {
                    unitShader.EnableHighlight();
                } else {
                    unitShader.DisableHighlight();
                }
            }
        }

        private void OnSelected() {
            unitShader.EnableHighlight();
        }

        private Vector3 position;

        private IEnumerator UnitMovement() {
            while (true) {
                if (Vector3.Distance(transform.position, moveTo) < 1f) {
                    StopCoroutine(unitMovement);
                    gridManager.UnitMovementCompleted(Controller);
                    yield return this;
                }
                TransformLerp(moveTo, Time.deltaTime * speed);
                RotateLerp(moveTo, Time.deltaTime * speed);
                yield return this;
            }
        }

        private void TransformLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            position.Set(position.x + (b.x - position.x) * t, gridInteractor.SampleTerrain(b.x, b.z) ?? b.y, position.z + (b.z - position.z) * t);
            transform.position = position;
        }

        private void RotateLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            b.y = transform.position.y;
            var toRotation = Quaternion.LookRotation(b - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, t);
        }
    }
}
