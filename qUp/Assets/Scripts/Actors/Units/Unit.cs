using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using Managers.GridManagers;
using Managers.InputManagers;
using UnityEngine;

namespace Actors.Units {
    public class Unit : BaseController<IUnitState>, IClickable {
        private readonly InputManagerBehaviour inputManager = ApiManager.ProvideManager<InputManagerBehaviour>();
        private readonly GridManager gridManager = ApiManager.ProvideManager<GridManager>();

        public UnitData data;

        private int currentHealth;
        private int currentTicks;

        private GridCoords coords;

        public void Init(UnitData inData, GameObject gameObject) {
            data = inData;
            currentHealth = inData.hp;
            currentTicks = inData.tickPoints;
            inputManager.RegisterClickable(this, gameObject);
        }

        public void OnClick() {
            gridManager.SelectUnit(this);
            SetState(UnitSelected.Where());
        }

        public void SetCoords(GridCoords inCoords) {
            coords = inCoords;
            gridManager.RegisterUnit(this, inCoords);
        }
    }
}
