using System;
using Actors.Grid.Generator;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Managers.ApiManagers;
using UnityEngine;
using Wrappers.Shaders;
using Random = UnityEngine.Random;

namespace Actors.Tiles {
    public class TileBehaviour : BaseMonoBehaviour<Tile, ITileState> {
        private GridInteractor gridInteractor = ApiManager.ProvideInteractor<GridInteractor>();

        private FieldShader fieldShader;

        private bool isHoverHighlightEnabled = true;

        [SerializeField]
        private bool isResourceField;

        public static void Instantiate(GameObject prefab, Transform parent, Vector3 position, GridCoords coords,
                                       Func<Vector2, float> sampleHeight) {
            Instantiate(prefab, position, Quaternion.identity, parent)
                .GetComponent<TileBehaviour>()
                .Init(coords, sampleHeight);
        }

        private void Init(GridCoords coords, Func<Vector2, float> sampleHeight) {
            Controller.Init(coords, transform.position, gameObject);
            DisplaceVertices(sampleHeight);
            if (isResourceField) {
                InstantiateResourceDecorator(sampleHeight);
            }
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
        }

        private void InstantiateResourceDecorator(Func<Vector2, float> sampleHeight) {
            Instantiate(gridInteractor.GetResourceDecorator(),
                transform.position.AddY(sampleHeight(transform.position.ToVectro2XZ())),
                Quaternion.Euler(0, Random.value, 0),
                transform);
        }

        protected override void OnStateHandler(ITileState inState) {
            if (inState is HighlightActivated highlightActivatedState) {
                fieldShader.SetHighlightOn(true, highlightActivatedState.HighlightColor);
            } else if (inState is Idle) {
                fieldShader.SetHighlightOn(false);
            }
        }

        protected override void OnAwake() {
            fieldShader = new FieldShader(GetComponent<MeshRenderer>().material);
            Controller.InitColors(fieldShader.GetMarkingsColor());
        }
    }
}
