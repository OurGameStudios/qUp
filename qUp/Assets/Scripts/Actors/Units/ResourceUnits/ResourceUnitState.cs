using Actors.Units.CombatUnits;
using Base.Interfaces;
using UnityEngine;

namespace Actors.Units.ResourceUnits {
    public interface IResourceUnitState : IState { }

    public abstract class ResourceUnitState<TState> : State<TState>, IResourceUnitState where TState : class, new() { }

    public class ResourceUnitSelected : ResourceUnitState<ResourceUnitSelected> {
        public static ResourceUnitSelected Where() => Cache;
    }

    public class ResourceUnitOwnership : ResourceUnitState<ResourceUnitOwnership> {
        public Color Color { get; private set; }

        public static ResourceUnitOwnership Where(Color color) {
            Cache.Color = color;
            return Cache;
        }
    }

    public class ResourceUnitHighlight : ResourceUnitState<ResourceUnitHighlight> {
        public bool IsHighlightOn { get; private set; }

        public static ResourceUnitHighlight Where(bool isHighlightOn) {
            Cache.IsHighlightOn = isHighlightOn;
            return Cache;
        } 
    }

    public class ResourceUnitMovement : ResourceUnitState<ResourceUnitMovement> {
        
        public Vector3 Position { get; private set; }
        public bool Combat { get; private set; }
        
        public static ResourceUnitMovement Where(Vector3 position, bool combat) {
            Cache.Position = position;
            Cache.Combat = combat;
            return Cache;
        }
    }

    public class DestroyUnit : ResourceUnitState<DestroyUnit> {

        public static DestroyUnit Where() {
            return Cache;
        }
    }
}
