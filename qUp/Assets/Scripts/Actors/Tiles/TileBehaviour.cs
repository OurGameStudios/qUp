using Base.MonoBehaviours;
using Common;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Tiles {
    public class TileBehaviour : BaseMonoBehaviour<Tile, TileState> {
        private FieldShader fieldShader;

        private bool isHoverHighlightEnabled = true;

        public static void Instantiate(GameObject prefab, Transform parent, Vector3 position, GridCoords coords) {
            Instantiate(prefab, position, Quaternion.identity, parent)
                .GetComponent<TileBehaviour>().Init(coords);
        }

        private void Init(GridCoords coords) {
            Controller.Init(coords);
        }

        protected override void OnStateHandler(TileState inState) {
            if (inState is MarkingsChange) {
                fieldShader.SetHighlightOn(true);
                fieldShader.SetAnimationTimeOffset(-Time.timeSinceLevelLoad);
                isHoverHighlightEnabled = false;
            } else if (inState is HighlightActivated highlightActivatedState) {
                fieldShader.SetHighlightColor(highlightActivatedState.HighlightColor);
                fieldShader.SetHighlightOn(true);
                fieldShader.SetAnimationTimeOffset(-Time.timeSinceLevelLoad);
            } else if (inState is Idle) {
                fieldShader.SetHighlightOn(false);
                isHoverHighlightEnabled = true;
            }
        }

        private void OnMouseDown() {
            Controller.OnClick();
        }

        private void OnMouseEnter() {
            if (isHoverHighlightEnabled) {
                fieldShader.SetHighlightOn(true);
                fieldShader.SetAnimationTimeOffset(-Time.timeSinceLevelLoad);
            }
        }

        private void OnMouseExit() {
            if (isHoverHighlightEnabled) {
                fieldShader.SetHighlightOn(false);
            }
        }

        private void Start() {
            fieldShader = new FieldShader(GetComponent<MeshRenderer>().material);
        }
    }
}
