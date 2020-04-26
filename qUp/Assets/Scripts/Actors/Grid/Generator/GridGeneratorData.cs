using System;
using System.Collections.Generic;
using Actors.Grid.GeneratorFunctions.Base;
using Actors.Grid.SymmetryFunctions.Base;
using Base;
using Base.Common;
using Common;
using Extensions;
using UnityEngine;

namespace Actors.Grid.Generator {
    [CreateAssetMenu(fileName = "MapGeneratorData", menuName = "Map/Generator")]
    public class GridGeneratorData : BaseData {
        [Serializable]
        public class PrediterminedFieldData {
            public string name;
            public List<GameObject> Prefabs;
            public GridCoords coords;
        }

        [SerializeField]
        private GeneratorFunction generatorFunction;
        [SerializeField]
        private SymmetryFunction symmetryFunction;

        public float fieldSize = 30;

        [SerializeField]
        private int mapHeight = 10; //-> 20

        [SerializeField]
        private int mapWidth; //-> 10

        [SerializeField]
        public List<PrediterminedFieldData> predeterminedFieldDatas;

        public int MapWidth => mapWidth.IfZero(MapHeight / 2);

        public int MapHeight => mapHeight * 2;

        public float YOffset => (fieldSize.Sqr() - (fieldSize / 2).Sqr()).Sqrt();

        public float XOffset => fieldSize * 1.5f;

        public GeneratorFunction GeneratorFunction => generatorFunction;

        public SymmetryFunction SymmetryFunction => symmetryFunction;
    }
}
