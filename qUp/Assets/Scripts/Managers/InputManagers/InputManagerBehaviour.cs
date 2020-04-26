using Base.Interfaces;
using Extensions;
using Managers.CameraManagers;
using UnityEngine;

namespace Managers.InputManagers {
    public class InputManagerBehaviour : MonoBehaviour, IManager {
        private Camera mainCamera;
        private Vector3? mouseWorldPosition;

        private bool cursorLockBugOffset;

        private void Awake() {
            GlobalManager.GetManager<PreUpdateManager>().SubscribeToPreUpdate(PreUpdate);
            mainCamera = Camera.main;
        }

        private void Update() {
            if (Inputs.IsCameraPanControl) {
                if (Cursor.lockState != CursorLockMode.Locked) {
                    cursorLockBugOffset = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = true;
                }

                if ((Inputs.MouseX > 0 || Inputs.MouseY > 0) && cursorLockBugOffset) {
                    GlobalManager.GetManager<CameraManager>().StartCameraRotation();
                    cursorLockBugOffset = false;
                } else if (!cursorLockBugOffset) {
                    GlobalManager.GetManager<CameraManager>()
                                 .PanCamera(Inputs.MouseX, Inputs.MouseY);
                }
                
            } else {
                Cursor.lockState = CursorLockMode.None;
                cursorLockBugOffset = false;
                GlobalManager.GetManager<CameraManager>().MoveCamera(Input.mousePosition);
            }

            if (Inputs.IsMouseScroll) {
                GlobalManager.GetManager<CameraManager>().ZoomCamera(Inputs.MouseScroll, GetMouseWorldPosition());
            }
        }

        private void PreUpdate() {
            mouseWorldPosition = null;
        }

        private Vector3 GetMouseWorldPosition() {
            if (mouseWorldPosition == null) {
                var mousePosition = Input.mousePosition;
                mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition.AddZ(mainCamera.nearClipPlane));
            }

            return (Vector3) mouseWorldPosition;
        }
    }
}
