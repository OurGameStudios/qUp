using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Grid.GeneratorFunctions.Base;
using Common;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Grid.GeneratorFunctions {
    [CreateAssetMenu(fileName = "RangeFromOriginGeneratorFunction",
        menuName = "Grid/Generator Functions/Range From Origin")]
    public class RangeFromOriginGenerator : GeneratorFunction {
        [Serializable]
        public class RangeSegment {
            public string name;
            public int rangeSegmentLength;
            public List<FieldType> fieldTypes;

            [NonSerialized]
            public int summedRangeSegmentLength = -1;

            public int AddToSummedRange(int previousRangeSegment) =>
                summedRangeSegmentLength = rangeSegmentLength + previousRangeSegment;
        }

        [Serializable]
        public class FieldType {
            public string name;
            public int chance;
            public List<GameObject> prefabs;

            [NonSerialized]
            public int summedChance = -1;

            public int AddToSummedChance(int previousSummedChance) => summedChance = chance + previousSummedChance;
        }

        [Tooltip("Leave seed at 0 for random seed")]
        public int seed;

        [Tooltip("Percentage of maximum distance on map from 0,0 to maxCoords")]
        [Range(0, 1)]
        public float maxRange = 0.5f;

        [NonSerialized]
        private bool isRandomSet;

        [NonSerialized]
        private int convertedMaxRange = -1;

        public List<RangeSegment> rangeSegments;

        public override GameObject GeneratePrefab(GridCoords coords, GridCoords maxCoords) {
            var rangeFromBase = coords.DistanceTo(GridCoords.Origin);
            var rangeSegment = GetRangeSegmentFor(rangeFromBase, maxCoords);
            InitRandom();
            var totalChance = CalculateTotalChance(rangeSegment);
            var random = Random.Range(0, totalChance);
            var prefabs = rangeSegment.fieldTypes.FirstOrDefault(fieldType => random < fieldType.summedChance)?.prefabs;
            return prefabs.GetRandom();
        }

        private void InitRandom() {
            if (!isRandomSet && seed != 0) {
                Random.InitState(seed);
                isRandomSet = true;
            } else if (!isRandomSet && seed == 0) {
                isRandomSet = true;
            }
        }

        private int CalculateTotalChance(RangeSegment rangeSegment) {
            if (rangeSegment.fieldTypes.Last().summedChance == -1) {
                rangeSegment.fieldTypes.First().AddToSummedChance(0);
                for (var i = 1; i < rangeSegment.fieldTypes.Count; i++) {
                    rangeSegment.fieldTypes[i].AddToSummedChance(rangeSegment.fieldTypes[i - 1].summedChance);
                }
            }

            return rangeSegment.fieldTypes.Last().summedChance;
        }


        private RangeSegment GetRangeSegmentFor(int distance, GridCoords maxCoords) {
            if (rangeSegments.Last().summedRangeSegmentLength == -1) {
                rangeSegments.First().AddToSummedRange(0);
                for (var i = 1; i < rangeSegments.Count; i++) {
                    rangeSegments[i].AddToSummedRange(rangeSegments[i - 1].summedRangeSegmentLength);
                }
            }

            if (convertedMaxRange == -1) {
                convertedMaxRange = Mathf.RoundToInt(GridCoords.Origin.DistanceTo(maxCoords) * maxRange);
            }

            var totalRange = rangeSegments.Last().summedRangeSegmentLength;
            var clampedDistance = Mathf.Clamp(distance, 0, convertedMaxRange);
            return rangeSegments.First(it =>
                       1f * clampedDistance / convertedMaxRange <= 1f * it.summedRangeSegmentLength / totalRange) ??
                   rangeSegments.Last();
        }
    }
}
