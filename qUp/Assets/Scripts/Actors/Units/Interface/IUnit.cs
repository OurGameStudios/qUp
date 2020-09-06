using Base.Interfaces;
using UnityEngine;

namespace Actors.Units.Interface {
    public interface IUnit : IClickable {
        void MoveToNextTile(Vector3 position, bool combatTile);

        void SetUnitColor(Color color);

        void DeactivateHighlight();

        int GetUpkeep();

        int GetCost();

        int GetTickPoints();

        void Destroy();
    }
}
