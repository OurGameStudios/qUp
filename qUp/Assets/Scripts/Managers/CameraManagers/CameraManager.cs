using Base.Managers;
using UnityEngine;

namespace Managers.CameraManagers {
    public class CameraManager : BaseManager<CameraManagerState> {
        private bool isCameraPanEnabled = true;

        public void DisableCameraPan() => isCameraPanEnabled = false;
        public void EnableCameraPan() => isCameraPanEnabled = true;

        public void MoveCamera(Vector3 mousePosition) {
            if (isCameraPanEnabled) {
                SetState(new CameraMove(new Vector2((mousePosition.x / Screen.width - 0.5f) * 2,
                    (mousePosition.y / Screen.height - 0.5f) * 2)));
            }
        }

        public void PanCamera(float xAxis, float yAxis) {
            SetState(new CameraPan(new Vector2(xAxis, yAxis)));
        }

        public void StartCameraRotation() {
            SetState(new CameraRotationStart());
        }

        public void ZoomCamera(float direction, Vector3 mousePosition) {
            SetState(new CameraZoom(direction, mousePosition));
        }

        public void SetWorldSize(float minX, float minY, float maxX, float maxY) {
            SetState(new WorldSize(new Vector2(minX, minY), new Vector2(maxX, maxY)));
        }
    }
}
