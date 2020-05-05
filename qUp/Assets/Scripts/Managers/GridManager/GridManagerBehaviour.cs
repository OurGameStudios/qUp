using System.Collections.Generic;
using Base.Managers;
using Extensions;
using UnityEditor;
using UnityEngine;

namespace Managers.GridManager {
    public class GridManagerBehaviour : BaseManagerMonoBehaviour<GridManager, GridManagerState> {
        private List<(Vector3, string)> gizmos = new List<(Vector3, string)>();
        
        protected override void OnStateHandler(GridManagerState inState) {
        }
    }
}
