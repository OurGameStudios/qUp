using System.Collections;
using Actors.Grid.Generator;
using Actors.Players;
using Base.MonoBehaviours;
using Managers.ApiManagers;
using Managers.GridManagers;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Units.ResourceUnits {
    public class ResourceUnitBehaviour : BaseMonoBehaviour<ResourceUnit, IResourceUnitState> {
        private GridManager gridManager = ApiManager.ProvideManager<GridManager>();
        private GridInteractor gridInteractor = ApiManager.ProvideInteractor<GridInteractor>();

        public UnitData data;
        public UnitShader unitShader;

        public Vector3 moveTo;

        private IEnumerator unitMovement;
        public float speed = 5f;

        public static ResourceUnit Instantiate(UnitData data, Vector3 position, Player player) {
            return Instantiate(data.prefab, position, Quaternion.identity)
                   .GetComponent<ResourceUnitBehaviour>()
                   .Controller;
        }

        protected override void OnAwake() {
            Controller.Init(data, gameObject);
            unitShader = new UnitShader(transform.GetComponent<MeshRenderer>().material);
            unitMovement = UnitMovement();
            position = transform.position;
        }

        protected override void OnStateHandler(IResourceUnitState inState) {
            if (inState is ResourceUnitSelected) {
                OnSelected();
            } else if (inState is ResourceUnitMovement unitMovementState) {
                moveTo = unitMovementState.Position;
                StartCoroutine(unitMovement);
            } else if (inState is ResourceUnitOwnership ownershipState) {
                unitShader.SetUnitPlayerColor(ownershipState.Color);
            } else if (inState is ResourceUnitHighlight highlightState) {
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
                yield return this;
            }
        }

        private void TransformLerp(Vector3 b, float t) {
            t = Mathf.Clamp01(t);
            position.Set(position.x + (b.x - position.x) * t, gridInteractor.SampleTerrain(b.x, b.z) ?? b.y, position.z + (b.z - position.z) * t);
            transform.position = position;
        }
    }
}
