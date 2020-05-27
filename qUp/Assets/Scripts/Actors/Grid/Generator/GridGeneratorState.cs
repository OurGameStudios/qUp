using Actors.Players;
using Base.Interfaces;
using Common;
using UnityEngine;

namespace Actors.Grid.Generator {
    public interface IGridGeneratorState : IState { }
    public abstract class GridGeneratorState<TState> : State<TState>, IGridGeneratorState where TState : class, new() { }

    public class FieldGenerated : GridGeneratorState<FieldGenerated> {
        public GameObject Prefab { get; private set; }
        public Vector3 Offset { get; private set; }
        public GridCoords Coords { get; private set; }

        public static FieldGenerated With(GameObject prefab, Vector3 offset, GridCoords coords) {
            Cache.Prefab = prefab;
            Cache.Offset = offset;
            Cache.Coords = coords;
            return Cache;
        }
    }
    
    public class ResourceFieldGenerated : GridGeneratorState<ResourceFieldGenerated> {
        public GameObject Prefab { get; private set; }
        public Vector3 Offset { get; private set; }
        public GridCoords Coords { get; private set; }
        
        public GameObject DecoratorPrefab { get; private set; }

        public static ResourceFieldGenerated With(GameObject prefab, Vector3 offset, GridCoords coords, GameObject decorator) {
            Cache.Prefab = prefab;
            Cache.Offset = offset;
            Cache.Coords = coords;
            Cache.DecoratorPrefab = decorator;
            return Cache;
        }
    }

    public class BaseGenerated : GridGeneratorState<BaseGenerated> {
        public Vector3 Offset { get; private set; }
        public GridCoords Coords { get; private set; }
        public Player Owner { get; private set; }


        public static BaseGenerated With(Vector3 offset, GridCoords coords, Player owner) {
            Cache.Offset = offset;
            Cache.Coords = coords;
            Cache.Owner = owner;
            return Cache;
        }
    }

    public class GridWorldSize : GridGeneratorState<GridWorldSize> {
        public float XMinOffset { get; private set; }
        public float YMinOffset { get; private set; }
        public float XMaxOffset { get; private set; }
        public float YMaxOffset { get; private set; }

        public static GridWorldSize With(float xMinOffset, float yMinOffset, float xMaxOffset, float yMaxOffset) {
            Cache.XMinOffset = xMinOffset;
            Cache.YMinOffset = yMinOffset;
            Cache.XMaxOffset = xMaxOffset;
            Cache.YMaxOffset = yMaxOffset;
            return Cache;
        }
    }
}
