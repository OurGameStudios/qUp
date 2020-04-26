using Actors.Grid.SymmetryFunctions.Base;
using Common;
using UnityEngine;

namespace Actors.Grid.SymmetryFunctions {
    [CreateAssetMenu(fileName = "NoSymmetryFunction", menuName = "Grid/Symmetry Functions/No symmetry")]
    public class NoSymmetryFunction : SymmetryFunction {
        public override GameObject ProvideTile(GridCoords coords, GridCoords maxCoords) {
            return GeneratorFunction.GeneratePrefab(coords, maxCoords);
        }
    }
}
