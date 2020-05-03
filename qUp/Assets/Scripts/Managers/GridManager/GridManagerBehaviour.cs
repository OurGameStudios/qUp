using System.Collections.Generic;
using Base.Managers;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Managers.GridManager {
    public class GridManagerBehaviour : BaseManagerMonoBehaviour<GridManager, GridManagerState> {
        private List<(Vector3, string)> gizmos = new List<(Vector3, string)>();
        
        protected override void OnStateHandler(GridManagerState inState) {
            if (inState is Test test) {
                gizmos = test.gizmos;

            }
        }

        private void OnDrawGizmos() {
            foreach (var gizmo in gizmos) {
                Handles.Label(gizmo.Item1.AddY(10), gizmo.Item2);
            }
        }
    }
}
