using System.Collections;
using Base.Singletons;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Common.Constants;

namespace Handlers {
    public class InputHandler : SingletonClass<InputHandler> {
        
        [Range(0, 1f)]
        private float cameraPanEdgePercentage = 0.1f;

        private readonly Inputs inputs;
        private readonly Mouse mouse;

        private Vector2 pointerPosition;
        private Vector2 mouseDelta;

        public InputHandler() {
            mouse = InputSystem.GetDevice<Mouse>();
            inputs = new Inputs();
            SetupCameraControls();
            SetupMenuControls();
        }

        public static void EnableControls() => Instance.inputs.Enable();

        public static void SetCameraControlsEnabled(bool isEnabled) {
            if (isEnabled) {
                Instance.inputs.CameraControls.Enable();
                // Instance.inputs.NextPlayer.Enable();
            } else {
                Instance.inputs.CameraControls.Disable();
                if (Instance.isPanning && Instance.panEnumerator != null) {
                    CoroutineHandler.DoStopCoroutine(Instance.panEnumerator);
                    Instance.isPanning = false;
                }
                // Instance.inputs.NextPlayer.Disable();
            }
        }

        private void SetupMenuControls() {
            inputs.Menu.OpenMenu.performed += _ => {
                Time.timeScale = 0;
                SceneManager.LoadScene(MAIN_MENU_INDEX, LoadSceneMode.Additive);
                inputs.Disable();
            };
        }

        private void SetupCameraControls() {
            inputs.CameraControls.PointerPosition.performed += context => {
                pointerPosition = context.ReadValue<Vector2>();
                CameraPan(pointerPosition);
            };
            inputs.CameraControls.MouseDelta.performed += context => mouseDelta = context.ReadValue<Vector2>();
            inputs.CameraControls.CameraRotation.started += _ => StartCameraRotation();
            inputs.CameraControls.CameraRotation.canceled += _ => StopCameraRotation();
            inputs.NextPlayer.Next.performed += _ => InteractionHandler.OnContinueToNextPhase();
        }

        #region Camera Pan

        private Vector2 panDirection;
        private IEnumerator panEnumerator;
        private IEnumerator PanEnumerator => panEnumerator ??= PanCoroutine();
        private bool isPanning;

        private void CameraPan(Vector2 pointerPosition) {
            if (isRotating) {
                if (isPanning && panEnumerator != null) {
                    CoroutineHandler.DoStopCoroutine(panEnumerator);
                    isPanning = false;
                }

                return;
            }

            panDirection = pointerPosition;
            NormalizeScreenPosition(ref panDirection);
            panDirection.Set(SignIfDeltaOver(panDirection.x, cameraPanEdgePercentage),
                SignIfDeltaOver(panDirection.y, cameraPanEdgePercentage));
            panDirection.Normalize();

            if (panDirection == Vector2.zero && isPanning && panEnumerator != null) {
                CoroutineHandler.DoStopCoroutine(panEnumerator);
                isPanning = false;
            } else if (isPanning == false || panEnumerator == null) {
                CoroutineHandler.DoStartCoroutine(PanEnumerator);
                isPanning = true;
            }
        }

        private IEnumerator PanCoroutine() {
            while (true) {
                CameraHandler.Pan(panDirection);
                yield return this;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #endregion

        #region Camera Rotation

        private Vector2 mouseRotationStartPosition;
        private IEnumerator rotateEnumerator;
        private IEnumerator RotateEnumerator => rotateEnumerator ??= RotateCoroutine();
        private bool isRotating;

        private void StartCameraRotation() {
            Cursor.visible = false;
            mouseRotationStartPosition = pointerPosition;

            if (isRotating == false || rotateEnumerator == null) {
                CoroutineHandler.DoStartCoroutine(RotateEnumerator);
                isRotating = true;
            }
        }

        private void StopCameraRotation() {
            Cursor.visible = true;
            mouse.WarpCursorPosition(mouseRotationStartPosition);

            if (isRotating && rotateEnumerator != null) {
                CoroutineHandler.DoStopCoroutine(rotateEnumerator);
                isRotating = false;
            }
        }

        private IEnumerator RotateCoroutine() {
            while (true) {
                CameraHandler.Rotate(mouseDelta);
                yield return this;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        #endregion

        private void NormalizeScreenPosition(ref Vector2 positionOnScreen) =>
            positionOnScreen.Set(2 * (positionOnScreen.x / Screen.width - 0.5f),
                (positionOnScreen.y / Screen.height - 0.5f) * 2);

        /// <summary>
        /// Returns a sign of a value if the value is inside the edge on negative or positive side.
        /// So for an edge of 0.1 true is returned if value is grate than 0.9 or less than -0.9. Returns a 0 if value is 0
        /// </summary>
        private float SignIfDeltaOver(float value, float edge) =>
            1f - Mathf.Abs(value) <= edge ? Mathf.Sign(value) : 0;
    }
}
