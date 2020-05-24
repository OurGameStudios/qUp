using Base.Interfaces;
using UnityEngine;

namespace Actors.Units {
    public interface IUnitState : IState { }

    public abstract class UnitState<TState> : State<TState>, IUnitState where TState : class, new() { }

    public class UnitSelected : UnitState<UnitSelected> {
        public static UnitSelected Where() => Cache;
    }

    public class UnitMovement : UnitState<UnitMovement> {
        
        public Vector3 Position { get; private set; }
        public bool Combat { get; private set; }
        
        public static UnitMovement Where(Vector3 position, bool combat) {
            Cache.Position = position;
            Cache.Combat = combat;
            return Cache;
        }
    }
}
