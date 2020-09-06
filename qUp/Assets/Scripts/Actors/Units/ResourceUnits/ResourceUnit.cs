using Actors.Units.Interface;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using Managers.GridManagers;
using Managers.InputManagers;
using UnityEngine;

namespace Actors.Units.ResourceUnits {
    public class ResourceUnit : BaseController<IResourceUnitState>, IUnit {
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
            // gridManager.SelectUnit(this);
            SetState(ResourceUnitSelected.Where());
        }

        public void OnSecondaryClick() { }

        public void SetCoords(GridCoords inCoords) {
            coords = inCoords;
            gridManager.RegisterResourceUnit(this, inCoords);
        }

        //TODO decide if vector3 is best way to dictate move position
        public void MoveToNextTile(Vector3 position, bool ResourceTile) {
            SetState(ResourceUnitMovement.Where(position, ResourceTile));
        }

        public void SetUnitColor(Color color) {
            SetState(ResourceUnitOwnership.Where(color));
        }

        public void DeactivateHighlight() {
            SetState(ResourceUnitHighlight.Where(false));
        }

        public int GetUpkeep() => data.upkeep;

        public int GetCost() => data.cost;

        public int GetTickPoints() => data.tickPoints;
        public void Destroy() {
            SetState(DestroyUnit.Where());
        }
    }
}
