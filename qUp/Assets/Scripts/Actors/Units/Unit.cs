using Base.Interfaces;
using Base.MonoBehaviours;

namespace Actors.Units {
    public class Unit : BaseController<UnitState>, IClickable {
        public UnitData data;
        
        private int currentHealth;
        private int currentTicks;

        public void Init(UnitData data) {
            this.data = data;
            currentHealth = data.hp;
            currentTicks = data.tickPoints;
        }
        
        public void OnClick() {
            SetState(new UnitSelected());
        }
    }
}
