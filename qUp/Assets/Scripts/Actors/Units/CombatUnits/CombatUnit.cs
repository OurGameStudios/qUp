using Actors.Players;
using Actors.Units.Interface;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using Managers.GridManagers;
using Managers.InputManagers;
using UnityEngine;

namespace Actors.Units.CombatUnits {
    public class CombatUnit : BaseController<ICombatUnitState>, IUnit {
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
            SetState(CombatUnitSelected.Where());
        }

        public void OnSecondaryClick() { }

        public void SetCoords(GridCoords inCoords) {
            coords = inCoords;
            gridManager.RegisterUnit(this, inCoords);
        }

        //TODO decide if vector3 is best way to dictate move position
        public void MoveToNextTile(Vector3 position, bool combatTile) {
            SetState(CombatUnitMovement.Where(position, combatTile));
        }

        public void SetUnitColor(Color color) {
            SetState(CombatUnitOwnership.Where(color));
        }

        public void DeactivateHighlight() {
            SetState(Highlight.Where(false));
        }

        public Player GetOwner() => null;

        public int GetUpkeep() => data.upkeep;

        public int GetCost() => data.cost;

        public int GetTickPoints() => data.tickPoints;
        public void Destroy() {
            
        }
    }
}
