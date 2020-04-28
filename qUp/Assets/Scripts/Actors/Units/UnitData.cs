using Base.Common;
using UnityEngine;

namespace Actors.Units {
    [CreateAssetMenu(fileName = "StandardUnit", menuName = "Units/StandardUnit")]
    public class UnitData : BaseData {
        public GameObject prefab;

        public Sprite unitUiImage;
        
        public string name;
        public int cost = 200;
        public int hp = 10;
        public int attack = 5;
        public int tickPoints = 3;
        public int overpowers;
    }
}
