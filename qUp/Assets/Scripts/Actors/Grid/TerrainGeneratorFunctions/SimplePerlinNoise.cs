using System;
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

        [NonSerialized]
        private int? generatedSeed;

        [NonSerialized]
        private int? random;

        public override float SampleTerrain(Vector2 worldCoordinates) =>
            SampleTerrain(worldCoordinates.x, worldCoordinates.y);

        public override float SampleTerrain(float x, float y) {
            x += offset.x;
            y += offset.y;
            if (seed != 0) {
                x += (int) (random ?? (random = new Random(seed).Next(-10000, 10000)));
                y += (int) random;
            } else {
                x += (int) (random ?? (random =
                    new Random((int) (generatedSeed ?? (generatedSeed = Mathf.RoundToInt(Time.time))))
                        .Next(-10000, 10000))
                        );
                y += (int) random;
            }


            return (Mathf.PerlinNoise(x / density, y * widthHeightRatio / density) -
                    (onlyPositiveHeight ? 0 : 0.5f)) * amplitude;
        }
    }
}
