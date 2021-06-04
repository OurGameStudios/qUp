using System.Collections.Generic;
using Actors.Players;
using Actors.Tiles;
using UnityEngine;

namespace Actors.Units {
    public interface IUnit {
        UnitData Data { get; }
        string UnitName { get; }
        int TickPoints { get; }
        int Hp { get; }
        IPlayer Owner { get; }
        List<ITile> Path { get; }
        void SetActive(bool isActive);
        bool IsActive();
        void SetPosition(Vector3 position);
        void SetTile(ITile tile);
        void SetOwner(IPlayer owner);
        void SetOriginTile(ITile tile);
        ITile GetOriginTile();
        void SetPath(List<ITile> path);
        int GetDamageFor(IUnit unit);
        void LostPathAt(int tick);
    }
}
