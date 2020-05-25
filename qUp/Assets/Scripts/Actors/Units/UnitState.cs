using Base.Interfaces;
using UnityEngine;

namespace Actors.Units {
    public interface IUnitState : IState { }

    public abstract class UnitState<TState> : State<TState>, IUnitState where TState : class, new() { }

    public class UnitSelected : UnitState<UnitSelected> {
        public static UnitSelected Where() => Cache;
    }

    public class UnitOwnership : UnitState<UnitOwnership> {
        public Color Color { get; private set; }

        public static UnitOwnership Where(Color color) {
            Cache.Color = color;
            return Cache;
        }
    }

    public class Highlight : UnitState<Highlight> {
        public bool IsHighlightOn { get; private set; }

        public static Highlight Where(bool isHighlightOn) {
            Cache.IsHighlightOn = isHighlightOn;
            return Cache;
        } 
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
