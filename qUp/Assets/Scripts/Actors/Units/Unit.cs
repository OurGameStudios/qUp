using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Managers;
using Managers.GridManager;

namespace Actors.Units {
    public class Unit : BaseController<UnitState>, IClickable {
        public UnitData data;
        
        private int currentHealth;
        private int currentTicks;

        private GridCoords coords;

        public void Init(UnitData inData) {
            data = inData;
            currentHealth = inData.hp;
            currentTicks = inData.tickPoints;
        }
        
        public void OnClick() {
            GlobalManager.GetManager<GridManager>().SelectUnit(this);
            SetState(new UnitSelected());
        }

        public void SetCoords(GridCoords inCoords) {
            this.coords = inCoords;
            GlobalManager.GetManager<GridManager>().RegisterUnit(this, inCoords);
        }
    }
}
