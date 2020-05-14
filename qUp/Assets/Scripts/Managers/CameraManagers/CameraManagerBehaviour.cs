using System.Collections;
using Base.Managers;
using Extensions;
using UnityEngine;
using static Extensions.Vector3Extensions;

namespace Managers.CameraManagers {
    public class CameraManagerBehaviour : BaseManagerMonoBehaviour<CameraManager, ICameraManagerState> {
        public Camera camera;

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

        private Vector3 maxOffsetFromWorld;

        protected override void OnStateHandler(ICameraManagerState inState) {
            if (inState is CameraPan moveState) {
                Pan(moveState.Direction);
            } else if (inState is CameraZoom zoomState) {
                Zoom(zoomState.Direction, zoomState.MouseWorldPosition);
            } else if (inState is CameraRotationStart) {
                SetRotationPoint();
            } else if (inState is CameraRotate rotateState) {
                Rotate(rotateState.Offset);
            } else if (inState is WorldSize worldSizeState) {
                minWorldPosition = worldSizeState.MinWorldPosition;
                maxWorldPosition = worldSizeState.MaxWorldPosition;
            }
        }

        private void Update() {
            ZoomAnimation();
        }

        private void Pan(Vector2 panDirection) {
            var cameraTransform = camera.transform;
            var rotY = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);
            cameraTransform.Translate(panDirection.x * panSpeed, 0, 0, Space.Self);
            cameraTransform.Translate(rotY * Vector3.forward * (panDirection.y * panSpeed), Space.World);


            var position = cameraTransform.position;
            cameraTransform.position = position.ClampAxis(Vector3Axis.X,
                                                   minWorldPosition.x + maxOffsetFromWorld.x,
                                                   maxWorldPosition.x + maxOffsetFromWorld.x)
                                               .ClampAxis(Vector3Axis.Z,
                                                   minWorldPosition.y + maxOffsetFromWorld.z,
                                                   maxWorldPosition.y + maxOffsetFromWorld.z);
        }

        private void Rotate(Vector2 direction) {
            var cameraTransform = camera.transform;
            cameraTransform.RotateAround(panPoint, Vector3.up, -direction.x * rotationSpeed);
            cameraTransform.RotateAroundClamped(panPoint, direction.y * rotationSpeed, minXRotate, maxXRotate);
            CalculateMaxOffsetFromWorld(cameraTransform);
        }

        private void CalculateMaxOffsetFromWorld(Transform cameraTransform) {
            var position = cameraTransform.position;
            var rotation = cameraTransform.rotation;
            var rotY = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
            var maxDistanceByAngle = position.y / Mathf.Tan(rotation.eulerAngles.x * (Mathf.PI / 180f));
            maxOffsetFromWorld = rotY * -Vector3.forward * maxDistanceByAngle;
        }

        private void SetRotationPoint() {
            var cameraTransform = camera.transform;
            var cameraPosition = cameraTransform.position;
            var cameraRotation = cameraTransform.rotation;
            panPoint = cameraPosition + cameraTransform.forward *
                (cameraPosition.y / Mathf.Cos((cameraRotation.eulerAngles.x - 90) * (Mathf.PI / 180f)));
        }

        private void Zoom(float direction, Vector3 mousePosition) {
            var cameraTransform = camera.transform;
            if (cameraTransform.position.y > minHeight && direction > 0f ||
                cameraTransform.position.y < maxHeight && direction < 0f) {
                zoomPosition = MoveTowards(cameraTransform.position,
                    mousePosition,
                    Mathf.Sign(direction) * zoomStepDistance);
                isZooming = true;
            }
        }

        private void ZoomAnimation() {
            if ((camera.transform.position - zoomPosition).magnitude >= 10 && isZooming) {
                var newPosition =
                    Vector3.Lerp(camera.transform.position, zoomPosition, Time.deltaTime * zoomAnimationSpeed);
                camera.transform.position = newPosition.ClampAxis(Vector3Axis.Y, minHeight, maxHeight);
                if (!camera.transform.position.y.IsBetween(minHeight + 1, maxHeight - 1)) {
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
