using System;
using System.Collections.Generic;
using Base.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Units {
    [CreateAssetMenu(fileName = "StandardUnit", menuName = "Units/StandardUnit")]
    public class UnitData : BaseData {
        public GameObject prefab;
        public GameObject ghostPrefab;

        public Sprite unitUiImage;
        
        [FormerlySerializedAs("name")]
        public string unitName;
        public int cost = 100;
        public int upkeep = 50;
        public int hp = 10;
        public int tickPoints = 3;

        [Serializable]
        public class Damage {
            public UnitData unitDatas;
            public int damage;
        }

        public List<Damage> damages;
    }
}
