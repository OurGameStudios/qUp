using Common;
using UnityEngine;

namespace Actors.Grid.GeneratorFunctions.Base {
    public abstract class GeneratorFunction : ScriptableObject {
        public abstract GameObject GeneratePrefab(GridCoords coords, GridCoords maxCoords);
    }
}
