using Base.Interfaces;
using Base.MonoBehaviours;

namespace Actors.Units {
    public class Unit : BaseController<UnitState>, IClickable {
        public UnitData data;
        
        private int currentHealth;
        private int currentTicks;

        public void Init(UnitData inData) {
            data = inData;
            currentHealth = inData.hp;
            currentTicks = inData.tickPoints;
        }
        
        public void OnClick() {
            SetState(new UnitSelected());
        }
    }
}
