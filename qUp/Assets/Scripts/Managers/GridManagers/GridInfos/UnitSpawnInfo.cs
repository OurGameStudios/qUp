using Actors.Units;
using Common;
using UnityEngine;

namespace Managers.GridManagers.GridInfos {
    public class UnitSpawnInfo {

        public Vector3 position { get; }
        public GridCoords coords { get;}
        
        public UnitData unitData { get; }

        public GameObject ghost;
        
        public UnitSpawnInfo(Vector3 position, GridCoords coords, UnitData unitData) {
            this.position = position;
            this.coords = coords;
            this.unitData = unitData;
        }
    }
}
