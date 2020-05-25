using System;
using System.Collections;
using System.Collections.Generic;
using Base.Interfaces;
using Common;
using Managers.ApiManagers;
using Managers.CameraManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Managers.InputManagers {
    public class PointerInteractions {
        private readonly Lazy<CameraManager> cameraManagerLazy =
            new Lazy<CameraManager>(ApiManager.ProvideManager<CameraManager>);

        private CameraManager CameraManager => cameraManagerLazy.Value;

        private Camera camera;
        private readonly Mouse mouse;

        private Ray cameraPointRay;
        private RaycastHit rayCastHit;

        private bool isRotationInProgress;
        private Vector2 mouseRotationStartPosition;
        private Vector3? mouseWorldPosition;

        private float screenEdgePercentage;
        private Vector2 panDirection = new Vector2();
        private Vector2 newPanDirection;

        private Vector2 pointerPosition;
        private Vector2 mouseDelta;

        private IEnumerator rotationEnumerator;
        private IEnumerator panEnumerator;

        // private IEnumerator RotationEnumerator => rotationEnumerator ?? (rotationEnumerator = Rotation());
        private IEnumerator PanEnumerator => panEnumerator ?? (panEnumerator = Panning());

        private readonly Dictionary<GameObject, IClickable> clickables = new Dictionary<GameObject, IClickable>(200);
        private readonly Dictionary<GameObject, IHoverable> hoverables = new Dictionary<GameObject, IHoverable>(200);

        private GameObject currentHitGameObject;

        private Action<IEnumerator> coroutineStartHandler;
        private Action<IEnumerator> coroutineStopHandler;
        private Action<GameObject> hoverStart;
        private Action<GameObject> hoverEnd;
        private Action<IClickable> interact;

        private PointerInteractions(Inputs inputs) {
            mouse = InputSystem.GetDevice<Mouse>();
            SetupInputs(inputs);
        }

        #region Builder pattern

        public class PointerInteractionsBuilder {
            private readonly PointerInteractions pointerInteractions;

            public PointerInteractionsBuilder(Inputs inputs) {
                pointerInteractions = new PointerInteractions(inputs);
            }

            public PointerInteractionsBuilder SetCamera(Camera camera) {
                pointerInteractions.camera = camera;
                return this;
            }

            public PointerInteractionsBuilder SetCoroutineHandlers(Action<IEnumerator> coroutineStartHandler,
                                                                   Action<IEnumerator> coroutineStopHandler) {
                pointerInteractions.coroutineStartHandler = coroutineStartHandler;
                pointerInteractions.coroutineStopHandler = coroutineStopHandler;
                return this;
            }

            public PointerInteractionsBuilder SetScreenEdgePercentage(float screenEdgePercentage) {
                pointerInteractions.screenEdgePercentage = screenEdgePercentage;
                return this;
            }

            public PointerInteractionsBuilder SetInteractionListener(Action<IClickable> interaction) {
                pointerInteractions.interact = interaction;
                return this;
            }

            public PointerInteractions Build() {
                return pointerInteractions;
            }
        }

        #endregion


        public void AddClickable(IClickable clickable, GameObject gameObject) => clickables.Add(gameObject, clickable);
        public void RemoveClickable(GameObject gameObject) => clickables.Remove(gameObject);
        public void AddHoverable(IHoverable hoverable, GameObject gameObject) => hoverables.Add(gameObject, hoverable);
        public void RemoveHoverable(GameObject gameObject) => hoverables.Remove(gameObject);

        private void SetupInputs(Inputs inputs) {
            inputs.CameraControls.CameraRotation.started += _ => StartRotation();
            inputs.CameraControls.CameraRotation.canceled += _ => EndRotation();
            inputs.CameraControls.CameraZoom.performed += context => OnZoom(context.ReadValue<float>());
            inputs.CameraControls.MouseDelta.performed += context => Rotation(context.ReadValue<Vector2>());

            inputs.UnitSelected.SelectPath.performed += _ => SelectPath();

            inputs.PlanningPhase.SelectUnit.performed += _ => SelectUnit();
            inputs.CameraControls.PointerPosition.performed += context => {
                pointerPosition = context.ReadValue<Vector2>();
                Pan();
                if (EventSystem.current.IsPointerOverGameObject()) {
                    HoverEnd();
                    currentHitGameObject = null;
                } else {
                    Hover();
                }
            };
        }

        private void Hover() {
            if (isRotationInProgress) {
                HoverEnd();
                currentHitGameObject = null;
                return;
            }

            cameraPointRay = camera.ScreenPointToRay(pointerPosition);
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

        #region Rotation

        private void StartRotation() {
            isRotationInProgress = true;
            // coroutineStartHandler?.Invoke(RotationEnumerator);
            CameraManager.StartCameraRotation();
            mouseRotationStartPosition = pointerPosition;
            Cursor.visible = false;
        }

        private void EndRotation() {
            isRotationInProgress = false;
            // coroutineStopHandler?.Invoke(RotationEnumerator);
            Cursor.visible = true;
            mouse.WarpCursorPosition(mouseRotationStartPosition);
        }

        private void Rotation(Vector2 mousePosition) {
            if (isRotationInProgress) {
                mouseDelta = mousePosition;
                CameraManager.RotateCamera(mouseDelta);
            }
        }

        // private IEnumerator Rotation() {
        //     while (true) {
        //         CameraManager.RotateCamera(mouseDelta);
        //         yield return this;
        //     }
        // }

        #endregion

        private void OnZoom(float scrollDelta) {
            if (isRotationInProgress) return;
            CameraManager.ZoomCamera(scrollDelta, GetMouseWorldPosition());
        }

        private void SelectUnit() {
            if (currentHitGameObject == null) return;
            if (InteractableTags.IsClickable(currentHitGameObject) && !EventSystem.current.IsPointerOverGameObject()) {
                clickables[currentHitGameObject].OnClick();
            }
        }

        private void SelectPath() {
            if (currentHitGameObject == null) return;
            if (currentHitGameObject.CompareTag(InteractableTags.TILE_TAG) && !EventSystem.current.IsPointerOverGameObject()) {
                clickables[currentHitGameObject].OnSecondaryClick();
            }
        }

        private void Pan() {
            if (isRotationInProgress) {
                coroutineStopHandler?.Invoke(PanEnumerator);
                return;
            }

            newPanDirection.Set(pointerPosition.x, pointerPosition.y);
            NormalizeScreenPosition(ref newPanDirection);

            newPanDirection.Set(SignIfDeltaOver(newPanDirection.x, screenEdgePercentage),
                SignIfDeltaOver(newPanDirection.y, screenEdgePercentage));

            if (newPanDirection != Vector2.zero) {
                if (panDirection == Vector2.zero) {
                    coroutineStartHandler?.Invoke(PanEnumerator);
                }

                panDirection.Set(newPanDirection.x, newPanDirection.y);
            } else if (panDirection != Vector2.zero) {
                coroutineStopHandler?.Invoke(PanEnumerator);
                panDirection.Set(0, 0);
            }
        }

        private IEnumerator Panning() {
            while (true) {
                CameraManager.PanCamera(panDirection * Time.deltaTime);
                yield return this;
            }
        }

        private float SignIfDeltaOver(float value, float edge) =>
            1f - Mathf.Abs(value) <= edge ? Mathf.Sign(value) : 0;

        private void NormalizeScreenPosition(ref Vector2 positionOnScreen) =>
            positionOnScreen.Set(2 * (positionOnScreen.x / Screen.width - 0.5f),
                (positionOnScreen.y / Screen.height - 0.5f) * 2);

        private Vector3 GetMouseWorldPosition() {
            return camera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, camera.farClipPlane));
        }

        public void CleanPlanningPhase() {
            HoverEnd();
        }
    }
}
