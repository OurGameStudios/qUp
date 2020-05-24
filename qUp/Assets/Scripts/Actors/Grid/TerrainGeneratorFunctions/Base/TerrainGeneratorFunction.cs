using UnityEngine;

namespace Actors.Grid.TerrainGeneratorFunctions.Base {
    public abstract class TerrainGeneratorFunction : ScriptableObject {
        public abstract float SampleTerrain(Vector2 worldCoordinates);
        public abstract float SampleTerrain(float x, float y);
    }
}
