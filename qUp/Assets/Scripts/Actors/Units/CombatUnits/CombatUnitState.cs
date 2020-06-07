using Base.Interfaces;
using UnityEngine;

namespace Actors.Units.CombatUnits {
    public interface ICombatUnitState : IState { }

    public abstract class CombatUnitState<TState> : State<TState>, ICombatUnitState where TState : class, new() { }

    public class CombatUnitSelected : CombatUnitState<CombatUnitSelected> {
        public static CombatUnitSelected Where() => Cache;
    }

    public class CombatUnitOwnership : CombatUnitState<CombatUnitOwnership> {
        public Color Color { get; private set; }

        public static CombatUnitOwnership Where(Color color) {
            Cache.Color = color;
            return Cache;
        }
    }

    public class Highlight : CombatUnitState<Highlight> {
        public bool IsHighlightOn { get; private set; }

        public static Highlight Where(bool isHighlightOn) {
            Cache.IsHighlightOn = isHighlightOn;
            return Cache;
        } 
    }

    public class CombatUnitMovement : CombatUnitState<CombatUnitMovement> {
        
        public Vector3 Position { get; private set; }
        public bool Combat { get; private set; }
        
        public static CombatUnitMovement Where(Vector3 position, bool combat) {
            Cache.Position = position;
            Cache.Combat = combat;
            return Cache;
        }
    }
}
