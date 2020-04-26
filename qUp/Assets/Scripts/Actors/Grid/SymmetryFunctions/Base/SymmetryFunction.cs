using Actors.Grid.GeneratorFunctions.Base;
using Common;
using UnityEngine;

namespace Actors.Grid.SymmetryFunctions.Base {
    public abstract class SymmetryFunction : ScriptableObject {
        protected GeneratorFunction GeneratorFunction { get; private set; }

        public void SupplyGeneratorFunction(GeneratorFunction function) => GeneratorFunction = function;

        public abstract GameObject ProvideTile(GridCoords coords, GridCoords maxCoords);
    }
}
