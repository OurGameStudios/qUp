using Base.Interfaces;
using UnityEngine;

namespace Managers.CameraManagers {
    public interface ICameraManagerState : IState { }

    public abstract class CameraManagerState<TState> : State<TState>, ICameraManagerState where TState : class, new() { }

    public class CameraPan : CameraManagerState<CameraPan> {
        private Vector2 direction = new Vector2();

        public Vector2 Direction => direction;

        public static CameraPan With(Vector2 direction) {
            Cache.direction.Set(direction.x, direction.y);
            return Cache;
        }
    }

    public class CameraRotationStart : CameraManagerState<CameraRotationStart> {
        public static CameraRotationStart Where() => Cache;
    }

    public class CameraRotate : CameraManagerState<CameraRotate> {
        public Vector2 Offset { get; private set; }

        public static CameraRotate Where(Vector2 offset) {
            Cache.Offset = offset;
            return Cache;
        }
    }

    public class CameraZoom : CameraManagerState<CameraZoom> {
        public float Direction { get; private set; }

        public Vector3 MouseWorldPosition { get; private set; }

        public static CameraZoom Where(float direction, Vector3 mouseWorldPosition) {
            Cache.Direction = direction;
            Cache.MouseWorldPosition = mouseWorldPosition;
            return Cache;
        }
    }

    public class WorldSize : CameraManagerState<WorldSize> {
        public Vector2 MinWorldPosition { get; private set; }
        public Vector2 MaxWorldPosition { get; private set; }

        public static WorldSize Where(Vector2 minWorldPosition, Vector2 maxWorldPosition) {
            Cache.MinWorldPosition = minWorldPosition;
            Cache.MaxWorldPosition = maxWorldPosition;
            return Cache;
        }
    }
}
