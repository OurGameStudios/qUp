using Actors.Players;
using Actors.Units.Interface;
using Base.MonoBehaviours;
using Common;
using Common.Interaction;
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
        public Player owner;

        public void Init(UnitData inData, GameObject gameObject) {
            data = inData;
            currentHealth = inData.hp;
            currentTicks = inData.tickPoints;
            inputManager.RegisterClickable(this, gameObject);
        }

        public void OnInteraction(ClickInteraction interaction) {
            if (interaction == ClickInteraction.Primary) {
                SetState(ResourceUnitSelected.Where());
            }
        }

        public void SetCoords(GridCoords inCoords) {
            coords = inCoords;
            gridManager.RegisterResourceUnit(this, inCoords);
        }

        //TODO decide if vector3 is best way to dictate move position
        public void MoveToNextTile(Vector3 position, bool resourceTile) {
            SetState(ResourceUnitMovement.Where(position, resourceTile));
        }

        public void SetUnitColor(Color color) {
            SetState(ResourceUnitOwnership.Where(color));
        }

        public void DeactivateHighlight() {
            SetState(ResourceUnitHighlight.Where(false));
        }

        public Player GetOwner() => owner;

        public int GetUpkeep() => data.upkeep;

        public int GetCost() => data.cost;

        public int GetTickPoints() => data.tickPoints;
        public void Destroy() {
            SetState(DestroyUnit.Where());
        }
    }
}
