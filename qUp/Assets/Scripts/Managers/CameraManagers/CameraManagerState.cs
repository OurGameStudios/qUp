using Base.Interfaces;
using UnityEngine;

namespace Managers.CameraManagers {
    public abstract class CameraManagerState : IState { }

    public class CameraMove : CameraManagerState {
        public Vector2 Direction { get; }
        public CameraMove(Vector2 direction) { Direction = direction; }
    }
    
    public class CameraPan : CameraManagerState {
        public Vector2 Direction { get; }
        public CameraPan(Vector2 direction) { Direction = direction; }
    }

    public class CameraRotationStart : CameraManagerState { }

    public class CameraZoom : CameraManagerState {
        public float Direction { get; }
        public Vector3 MouseWorldPosition { get; }

        public CameraZoom(float direction, Vector3 mouseWorldPosition) {
            Direction = direction;
            MouseWorldPosition = mouseWorldPosition;
        }
    }

    public class WorldSize : CameraManagerState {
        public Vector2 MinWorldPosition { get; }
        public Vector2 MaxWorldPosition { get; }
        public WorldSize(Vector2 minWorldPosition, Vector2 maxWorldPosition) {
            MinWorldPosition = minWorldPosition;
            MaxWorldPosition = maxWorldPosition;
        }
    }
}
