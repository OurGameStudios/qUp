using System;
using System.Collections;
using System.Collections.Generic;
using Base.Interfaces;
using Common;
using InputControlls;
using Managers.ApiManagers;
using Managers.CameraManagers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers.InputManagers {
    public class InputManagerBehaviour : MonoBehaviour, IManager {

        private readonly Lazy<CameraManager> cameraManagerLazy = new Lazy<CameraManager>(ApiManager.ProvideManager<CameraManager>);
        private CameraManager CameraManager => cameraManagerLazy.Value;

        public Camera mainCamera;

        public float cameraPanEdgePercentage = 0f;
        private Vector2 panDirection;
        private Vector2 newPanDirection;

        private Vector3? mouseWorldPosition;

        private Ray cameraPointRay;
        private RaycastHit rayCastHit;
        private GameObject currentHitGameObject;

        private Inputs inputs;

        private Coroutine rotation;
        private Coroutine panning;

        private readonly Dictionary<GameObject, IClickable> clickables = new Dictionary<GameObject, IClickable>(200);
        private readonly Dictionary<GameObject, IHoverable> hoverables = new Dictionary<GameObject, IHoverable>(200);

        private bool isRotationInProgress;
        private Vector2 mouseRotationStartPosition;

        public void RegisterClickable(IClickable clickable, GameObject gameObject) {
            clickables.Add(gameObject, clickable);
        }

        public void RegisterHoverable(IHoverable hoverable, GameObject gameObject) {
            hoverables.Add(gameObject, hoverable);
        }

        private void Awake() {
            ApiManager.ExposeManager(this);

            mainCamera = Camera.main;

            SetupInputs();
            ApiManager.ProvideManager<PreUpdateManager>().SubscribeToPreUpdate(PreUpdate);
        }

        private void SetupInputs() {
            inputs = new Inputs();
            inputs.Enable();
            inputs.NoUnitSelected.CameraRotation.started += _ => StartRotation();
            inputs.NoUnitSelected.CameraRotation.canceled += _ => EndRotation();
            inputs.NoUnitSelected.SelectUnit.performed += _ => OnClick();
            inputs.NoUnitSelected.CameraZoom.performed += ctx => OnZoom(ctx.ReadValue<float>());
            inputs.NoUnitSelected.CameraPan.performed += ctx => Pan(ctx.ReadValue<Vector2>());
        }

        private void PreUpdate() {
            if (isRotationInProgress) {
                HoverEnd();
                currentHitGameObject = null;
                return;
            }

            cameraPointRay = mainCamera.ScreenPointToRay(InputSystem.GetDevice<Mouse>().position.ReadValue());
            Physics.Raycast(cameraPointRay, out rayCastHit, 700);
            var hitGameObject = rayCastHit.transform?.gameObject;
            if (currentHitGameObject != hitGameObject) {
                HoverEnd();
                currentHitGameObject = hitGameObject;
                HoverStart();
            } else {
                currentHitGameObject = hitGameObject;
            }
        }

        #region Hover

        private void HoverStart() {
            if (InteractableTags.IsHoverable(currentHitGameObject) == true) {
                hoverables[currentHitGameObject].OnHoverStart();
            }
        }

        private void HoverEnd() {
            if (InteractableTags.IsHoverable(currentHitGameObject) == true) {
                hoverables[currentHitGameObject].OnHoverEnd();
            }
        }

        #endregion

        #region Rotation

        private void StartRotation() {
            isRotationInProgress = true;
            rotation = StartCoroutine(Rotation());
            CameraManager.StartCameraRotation();
            mouseRotationStartPosition = InputSystem.GetDevice<Mouse>().position.ReadValue();
            Cursor.visible = false;
        }

        private IEnumerator Rotation() {
            while (true) {
                var offset = InputSystem.GetDevice<Mouse>().delta.ReadValue();
                CameraManager.RotateCamera(offset);
                yield return new WaitForEndOfFrame();
            }
        }

        private void EndRotation() {
            isRotationInProgress = false;
            StopCoroutine(rotation);
            Cursor.visible = true;
            InputSystem.GetDevice<Mouse>().WarpCursorPosition(mouseRotationStartPosition);
        }

        #endregion

        private void OnZoom(float scrollDelta) {
            CameraManager.ZoomCamera(scrollDelta, GetMouseWorldPosition());
        }

        private void OnClick() {
            if (InteractableTags.IsClickable(currentHitGameObject)) {
                clickables[currentHitGameObject].OnClick();
            }
        }


        private void Pan(Vector2 mousePosition) {
            if (isRotationInProgress) {
                if (panning != null) StopCoroutine(panning);
                return;
            }

            mousePosition.Set((mousePosition.x / Screen.width - 0.5f) * 2,
                (mousePosition.y / Screen.height - 0.5f) * 2);
            
            if (1f - Mathf.Abs(mousePosition.x) <= cameraPanEdgePercentage) {
                newPanDirection.x = Mathf.Sign(mousePosition.x);
            }

            if (1f - Mathf.Abs(mousePosition.y) <= cameraPanEdgePercentage) {
                newPanDirection.y = Mathf.Sign(mousePosition.y);
            }

            if (newPanDirection == Vector2.zero) {
                if (panning != null) StopCoroutine(panning);
                return;
            }
            

            if (newPanDirection == panDirection) return;
            if (panning != null) StopCoroutine(panning);
            panDirection = newPanDirection;
            panning = StartCoroutine(Panning());
        }

        private IEnumerator Panning() {
            while (true) {
                CameraManager.PanCamera(panDirection);
                yield return null;
            }
        }

        // private void Update() {
        //     if (InputsOld.IsCameraPanControl) {
        //         if (Cursor.lockState != CursorLockMode.Locked) {
        //             cursorLockBugOffset = true;
        //             Cursor.lockState = CursorLockMode.Locked;
        //             Cursor.visible = true;
        //         }
        //
        //         if ((InputsOld.MouseX > 0 || InputsOld.MouseY > 0) && cursorLockBugOffset) {
        //             GlobalManager.GetManager<CameraManager>().StartCameraRotation();
        //             cursorLockBugOffset = false;
        //         } else if (!cursorLockBugOffset) {
        //             GlobalManager.GetManager<CameraManager>()
        //                          .PanCamera(InputsOld.MouseX, InputsOld.MouseY);
        //         }
        //         
        //     } else {
        //         Cursor.lockState = CursorLockMode.None;
        //         cursorLockBugOffset = false;
        //         GlobalManager.GetManager<CameraManager>().MoveCamera(Input.mousePosition);
        //     }
        //
        //     if (InputsOld.IsMouseScroll) {
        //         GlobalManager.GetManager<CameraManager>().ZoomCamera(InputsOld.MouseScroll, GetMouseWorldPosition());
        //     }
        // }

        private Vector3 GetMouseWorldPosition() {
            if (mouseWorldPosition != null) return (Vector3) mouseWorldPosition;
            var mousePosition = InputSystem.GetDevice<Mouse>().position.ReadValue();
            mouseWorldPosition =
                mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));

            return (Vector3) mouseWorldPosition;
        }
    }
}
