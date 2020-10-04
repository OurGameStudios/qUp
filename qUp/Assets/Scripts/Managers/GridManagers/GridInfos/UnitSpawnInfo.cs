using Common;
using UnityEngine;

namespace Managers.GridManagers.GridInfos {
    public class UnitSpawnInfo {

        public Vector3 position { get; }
        public GridCoords coords { get;}
        
        public UnitSpawnInfo(Vector3 position, GridCoords coords) {
            this.position = position;
            this.coords = coords;
        }
    }
}
