using Base.MonoBehaviours;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Units {
    public class UnitBehaviour : BaseMonoBehaviour<Unit, UnitState> {
        public UnitData data;
        public UnitShader unitShader;

        public static Unit Instantiate(UnitData data, Vector3 position) {
            return Instantiate(data.prefab, position, Quaternion.identity)
                                .GetComponent<UnitBehaviour>()
                                .Controller;
        }

        protected override void OnAwake() {
            Controller.Init(data);
            unitShader = new UnitShader(transform.GetComponent<MeshRenderer>().material);
        }

        private void OnMouseDown() {
            Controller.OnClick();
        }

        protected override void OnStateHandler(UnitState inState) {
            if (inState is UnitSelected unitSelected) {
                OnSelected();
            }
        }

        private void OnSelected() {
            unitShader.EnableHighlight();
        }
    }
}
