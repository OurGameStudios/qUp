using System;
using Actors.Players;
using Base.MonoBehaviours;
using Common;
using Extensions;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Hqs {
    public class HqBehaviour : BaseMonoBehaviour<Hq, IHqState> {
        private PlayerBaseShader playerBaseShader;

        public static void Instantiate(Vector3 position, GridCoords coords, Player owner, Func<Vector2, float> sampleHeight) {
            Instantiate(owner.BasePrefab, position, Quaternion.identity)
                .GetComponentInChildren<HqBehaviour>()
                .InitBase(owner, coords, sampleHeight);
        }

        private void InitBase(Player owner, GridCoords coords, Func<Vector2, float> sampleHeight) {
            Controller.Init(coords, owner);
            playerBaseShader = new PlayerBaseShader(transform.GetChild(0).GetComponent<MeshRenderer>().material);
            playerBaseShader.SetColor(owner.PlayerColor);
            name = owner.PlayerName + " Base";
            DisplaceVertices(sampleHeight);
        }

        protected override void OnStateHandler(IHqState inBaseState) {
            if (inBaseState is HqSelection selectionState) {
                playerBaseShader.SetIsSelected(selectionState.IsSelected);
            }
        }
        
        private void DisplaceVertices(Func<Vector2, float> sampleHeight) {
            var child = transform.GetChild(0);
            var mesh = child.GetComponent<MeshFilter>().mesh;
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++) {
                var vertexWorldPosition = child.transform.TransformPoint(vertices[i]);
                var vertexHeight = sampleHeight.Invoke(vertexWorldPosition.ToVectro2XZ());
                vertices[i] = vertices[i].AddY(vertexHeight);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            child.GetComponent<MeshFilter>().mesh = mesh;

            var hqChild = transform.GetChild(1);
            var position = hqChild.position;
            var hqYposition = sampleHeight(position.ToVectro2XZ());
            position = position.AddY(hqYposition);
            hqChild.position = position;
        }
    }
}
