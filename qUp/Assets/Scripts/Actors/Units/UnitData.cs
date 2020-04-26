using Base.Common;
using UnityEngine;

namespace Actors.Units {
    [CreateAssetMenu(fileName = "StandardUnit", menuName = "Units/StandardUnit")]
    public class UnitData : BaseData {
        public int hp = 10;
        public int attack = 5;
        public int tickPoints = 3;
        public int Overpowers;
    }
}
