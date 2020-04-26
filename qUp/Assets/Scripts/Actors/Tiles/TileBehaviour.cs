using Base;
using Base.MonoBehaviours;
using Common;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Tiles {
    public class TileBehaviour : BaseMonoBehaviour<Tile, TileState> {
        private FieldShader fieldShader;

        public static void Instantiate(GameObject prefab, Transform parent, Vector3 position, GridCoords coords) {
            Instantiate(prefab, position, Quaternion.identity, parent)
                .GetComponent<TileBehaviour>().Init(coords);
        }

        private void Init(GridCoords coords) {
            Controller.Init(coords);
        }

        protected override void OnStateHandler(TileState inState) {
            if (inState is Highlight highlight) {
                fieldShader.SetHighlightOn(true);
                fieldShader.SetAnimationTimeOffset(-Time.timeSinceLevelLoad);
            }
        }

        private void OnMouseDown() {
            Controller.OnClick();
        }

        private void OnMouseEnter() {
            fieldShader.SetHighlightOn(true);
            fieldShader.SetAnimationTimeOffset(-Time.timeSinceLevelLoad);
        }

        private void OnMouseExit() {
            fieldShader.SetHighlightOn(false);
        }

        private void Start() {
            fieldShader = new FieldShader(GetComponent<MeshRenderer>().material);
        }
    }
}
