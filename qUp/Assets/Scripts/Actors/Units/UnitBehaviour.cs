using Base.MonoBehaviours;
using Common;
using UnityEngine;
using UnityEngine.AI;
using Wrappers.Shaders;

namespace Actors.Units {
    public class UnitBehaviour : BaseMonoBehaviour<Unit, IUnitState> {
        public UnitData data;
        public UnitShader unitShader;

        public Vector3 moveTo;
        public bool move;

        public static Unit Instantiate(UnitData data, Vector3 position) {
            return Instantiate(data.prefab, position, Quaternion.identity)
                   .GetComponent<UnitBehaviour>()
                   .Controller;
        }

        private void Update() {
            if (move) {
                GetComponent<NavMeshAgent>()?.SetDestination(moveTo);
                move = false;
            }
        }

        protected override void OnAwake() {
            Controller.Init(data, gameObject);
            unitShader = new UnitShader(transform.GetComponent<MeshRenderer>().material);
        }

        protected override void OnStateHandler(IUnitState inState) {
            if (inState is UnitSelected) {
                OnSelected();
            }
        }

        private void OnSelected() {
            unitShader.EnableHighlight();
        }
    }
}
