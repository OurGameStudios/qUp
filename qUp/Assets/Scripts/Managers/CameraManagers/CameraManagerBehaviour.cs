using Base.Managers;
using Extensions;
using UnityEngine;
using static Extensions.Vector3Extensions;

namespace Managers.CameraManagers {
    public class CameraManagerBehaviour : BaseManagerMonoBehaviour<CameraManager, CameraManagerState> {

        [Header("Zoom")]
        public float minHeight = 100;

        public float maxHeight = 500;
        public float zoomStepDistance = 50;
        public float zoomAnimationSpeed = 25;

        [Header("Rotation")]
        [Range(90, -90)]
        public float minXRotate = 60;

        [Range(90, -90)]
        public float maxXRotate = 90;

        public float rotationSpeed = 1f;

        [Header("Pan")]
        [Range(0, 1f)]
        public float edgePercentage = 0.1f;

        public float panSpeed;


        private Vector3 zoomPosition;
        private bool isZooming;

        private Vector3 panPoint;

        private Vector2 minWorldPosition;
        private Vector2 maxWorldPosition;

        private Camera mainCamera;

        protected override void OnStateHandler(CameraManagerState inState) {
            if (inState is CameraMove moveState) {
                Move(moveState.Direction);
            } else if (inState is CameraPan panState) {
                Rotate(panState.Direction);
            } else if (inState is CameraZoom zoomState) {
                Zoom(zoomState.Direction, zoomState.MouseWorldPosition);
            } else if (inState is CameraRotationStart) {
                SetRotationPoint();
            } else if (inState is WorldSize worldSizeState) {
                minWorldPosition = worldSizeState.MinWorldPosition;
                maxWorldPosition = worldSizeState.MaxWorldPosition;
            }
        }

        protected override void OnAwake() {
            mainCamera = Camera.main;
        }

        private void Update() {
            ZoomAnimation();
        }

        private void Move(Vector2 mousePosition) {
            var cameraTransform = mainCamera.transform;
            var rotY = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
            if (1f - Mathf.Abs(mousePosition.x) < edgePercentage) {
                cameraTransform.Translate(Mathf.Sign(mousePosition.x) * panSpeed, 0, 0, Space.Self);
            }

            if (1f - Mathf.Abs(mousePosition.y) < edgePercentage) {
                cameraTransform.Translate(rotY * Vector3.forward * (Mathf.Sign(mousePosition.y) * panSpeed),
                    Space.World);
            }


            var position = cameraTransform.position;
            var maxDistanceByAngle =
                position.y /
                Mathf.Tan((cameraTransform.rotation.eulerAngles.x) * (Mathf.PI / 180f));
            var maxDistanceByAngleVector = rotY * -Vector3.forward * maxDistanceByAngle;
            cameraTransform.position = position.ClampAxis(Vector3Axis.X,
                                                   minWorldPosition.x + maxDistanceByAngleVector.x,
                                                   maxWorldPosition.x + maxDistanceByAngleVector.x)
                                               .ClampAxis(Vector3Axis.Z,
                                                   minWorldPosition.y + maxDistanceByAngleVector.z,
                                                   maxWorldPosition.y + maxDistanceByAngleVector.z);
        }

        private void Rotate(Vector2 direction) {
            var cameraTransform = mainCamera.transform;
            cameraTransform.RotateAround(panPoint, Vector3.up, -direction.x * rotationSpeed);
            cameraTransform.RotateAroundClamped(panPoint, direction.y * rotationSpeed, minXRotate, maxXRotate);
        }

        private void SetRotationPoint() {
            var cameraTransform = mainCamera.transform;
            var cameraPosition = cameraTransform.position;
            var cameraRotation = cameraTransform.rotation;
            panPoint = cameraPosition + cameraTransform.forward *
                (cameraPosition.y / Mathf.Cos((cameraRotation.eulerAngles.x - 90) * (Mathf.PI / 180f)));
        }

        private void Zoom(float direction, Vector3 mousePosition) {
            var cameraTransform = mainCamera.transform;
            if (cameraTransform.position.y > minHeight && direction > 0f ||
                cameraTransform.position.y < maxHeight && direction < 0f) {
                zoomPosition = MoveTowards(cameraTransform.position,
                    mousePosition,
                    Mathf.Sign(direction) * zoomStepDistance);
                isZooming = true;
            }
        }

        private void ZoomAnimation() {
            if ((mainCamera.transform.position - zoomPosition).magnitude >= 10 && isZooming) {
                var newPosition =
                    Vector3.Lerp(mainCamera.transform.position, zoomPosition, Time.deltaTime * zoomAnimationSpeed);
                mainCamera.transform.position = newPosition.ClampAxis(Vector3Axis.Y, minHeight, maxHeight);
                if (!mainCamera.transform.position.y.IsBetween(minHeight + 1, maxHeight - 1)) {
                    isZooming = false;
                }
            } else {
                isZooming = false;
            }
        }

        private Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta) {
            var a = target - current;
            var magnitude = a.magnitude;
            return current + a / magnitude * maxDistanceDelta;
        }
    }
}
