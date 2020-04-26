using System;
using System.Collections.Generic;
using Actors.Grid.GeneratorFunctions.Base;
using Common;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actors.Grid.GeneratorFunctions {
    [CreateAssetMenu(fileName = "RandomGeneratorFunction", menuName = "Grid/Generator Functions/Random")]
    public class RandomGeneratorFunction
        : GeneratorFunction {

        [Serializable]
        public class FieldType {
            public string name;
            public List<GameObject> prefabs;
        }

        [Tooltip("Leave seed at 0 for random seed")]
        public int seed;

        public List<FieldType> fieldTypes;

        [NonSerialized]
        private bool isRandomSet;

        public override GameObject GeneratePrefab(GridCoords coords, GridCoords maxCoords) {
            InitRandom();
            return fieldTypes.GetRandom().prefabs.GetRandom();
        }

        private void InitRandom() {
            if (!isRandomSet && seed != 0) {
                Random.InitState(seed);
                isRandomSet = true;
            } else if (!isRandomSet && seed == 0) {
                isRandomSet = true;
            }
        }
    }
}
