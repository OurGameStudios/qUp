using Actors.Players;
using Base.Interfaces;
using Common;
using UnityEngine;

namespace Actors.Grid.Generator {
    public abstract class GridGeneratorState : IState { }

    public class FieldGenerated : GridGeneratorState {
        public GameObject Prefab { get; }
        public Vector3 Offset { get; }
        public GridCoords Coords { get; }

        public FieldGenerated(GameObject prefab, Vector3 offset, GridCoords coords) {
            Prefab = prefab;
            Offset = offset;
            Coords = coords;
        }
    }

    public class BaseGenerated : GridGeneratorState {
        public Vector3 Offset { get; }
        public GridCoords Coords { get; }
        public Player Owner { get; }


        public BaseGenerated(Vector3 offset, GridCoords coords, Player owner) {
            Offset = offset;
            Coords = coords;
            Owner = owner;
        }
    }

    public class GridWorldSize : GridGeneratorState {
        public float XMinOffset { get; }
        public float YMinOffset { get; }
        public float XMaxOffset { get; }
        public float YMaxOffset { get; }

        public GridWorldSize(float xMinOffset, float yMinOffset, float xMaxOffset, float yMaxOffset) {
            XMinOffset = xMinOffset;
            YMinOffset = yMinOffset;
            XMaxOffset = xMaxOffset;
            YMaxOffset = yMaxOffset;
        }
    }
}
