using Base.Singletons;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Handlers {
    public class CameraHandler : SingletonMonoBehaviour<CameraHandler> {

        [FormerlySerializedAs("camera")]
        [SerializeField]
        private Camera mainCamera;

        [SerializeField]
        private AudioSource source;

        public static Camera MainCamera => Instance.mainCamera;

        private GameObject cameraGameObject;

        [Header("Rotation")]
        [Range(-89, 89)]
        public float minXRotate = 60;

        [Range(-89, 89)]
        public float maxXRotate = 89;

        public float rotationSpeed = 0.01f;

        [Header("Pan")]
        public float panSpeed = 0.01f;

        private void Awake() {
            cameraGameObject = mainCamera.gameObject;
        }

        public static void EnableCamera(bool isEnabled) {
            Instance.gameObject.SetActive(isEnabled);
            Instance.source.mute = !isEnabled;
        }

        public static void Pan(Vector2 panDirection) {
            var transform1 = Instance.transform;
            var newPosition = transform1.position +
                transform1.rotation * panDirection.ToVector3XZ() * Instance.panSpeed * Time.deltaTime;
            Instance.transform.position = LimitPosition(newPosition);
        }

        public static void Rotate(Vector2 direction) {
            var position = Instance.transform.position;
            Instance.transform.Rotate(Vector3.up, -direction.x * Instance.rotationSpeed * Time.deltaTime);
            Instance.cameraGameObject.transform.RotateAroundClamped(position,
                direction.y * Instance.rotationSpeed * Time.deltaTime,
                Instance.minXRotate,
                Instance.maxXRotate);
        }

        private static Vector3 LimitPosition(Vector3 position) {
            //TODO input real min max
            var xClampedPosition = position.ClampAxis(Vector3Extensions.Vector3Axis.X, 0, 300);
            var clampedPosition = xClampedPosition.ClampAxis(Vector3Extensions.Vector3Axis.Z, 0, 300);
            return clampedPosition;
        }
    }
}
