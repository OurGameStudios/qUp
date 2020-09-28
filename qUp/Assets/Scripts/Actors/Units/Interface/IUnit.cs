﻿using Actors.Players;
using Base.Interfaces;
using Common.Interaction;
using UnityEngine;

namespace Actors.Units.Interface {
    public interface IUnit : IClickable {
        void MoveToNextTile(Vector3 position, bool combatTile);

        void SetUnitColor(Color color);

        void DeactivateHighlight();

        Player GetOwner();

        int GetUpkeep();

        int GetCost();

        int GetTickPoints();

        void Destroy();
    }
}
