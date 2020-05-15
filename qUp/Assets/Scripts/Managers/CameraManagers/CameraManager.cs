using Base.Managers;
using UnityEngine;

namespace Managers.CameraManagers {
    public class CameraManager : BaseManager<ICameraManagerState> {

        public GameObject hoveredGameObject;

        private bool isCameraPanEnabled = true;

        public void PanCamera(Vector2 panDirection) {
            if (isCameraPanEnabled) {
                SetState(CameraPan.With(panDirection));
            }
        }

        public void RotateCamera(Vector2 offset) {
            SetState(CameraRotate.Where(offset));
        }

        public void StartCameraRotation() {
            SetState(CameraRotationStart.Where());
        }

        public void ZoomCamera(float direction, Vector3 mousePosition) {
            SetState(CameraZoom.Where(direction, mousePosition));
        }

        public void SetWorldSize(float minX, float minY, float maxX, float maxY) {
            SetState(WorldSize.Where(new Vector2(minX, minY), new Vector2(maxX, maxY)));
        }
    }
}
