using System.Collections.Generic;
using Base.Interfaces;
using UnityEngine;

namespace Managers.GridManager {
    public abstract class GridManagerState : IState { }

    public class Test : GridManagerState {
        public List<(Vector3 position, string text)> gizmos;
    }

    public class UnitSelected : GridManagerState {
        public object Unit { get; }

        public UnitSelected(object unit) {
            Unit = unit;
        }
    }

    public class GroupSelected : GridManagerState { }

    public class BaseSelected : GridManagerState { }
}
