using Actors.Grid.TerrainGeneratorFunctions.Base;
using UnityEngine;
using Random = System.Random;

namespace Actors.Grid.TerrainGeneratorFunctions {
    [CreateAssetMenu(fileName = "SimplePerlinNoise", menuName = "Grid/Terrain Generator Functions/Simple Perlin")]
    public class SimplePerlinNoise : TerrainGeneratorFunction {
        public bool onlyPositiveHeight = true;

        public int seed;

        public float density = 50f;
        public float amplitude = 10f;
        public float widthHeightRatio = 1f;

        public Vector2 offset = Vector2.zero;


        public override float SampleTerrain(Vector2 worldCoordinates) {
            var offsetCoordinates = worldCoordinates + offset;
            if (seed != 0) {
                var random = new Random(seed).Next(-10000, 10000);
                offsetCoordinates += new Vector2(random, random);
            } else {
                var random = new Random(Mathf.RoundToInt(Time.time)).Next(-10000, 10000);
                offsetCoordinates += new Vector2(random, random);
            }


            return (Mathf.PerlinNoise(offsetCoordinates.x / density, offsetCoordinates.y * widthHeightRatio / density) -
                    (onlyPositiveHeight ? 0 : 0.5f)) * amplitude;
        }
    }
}
