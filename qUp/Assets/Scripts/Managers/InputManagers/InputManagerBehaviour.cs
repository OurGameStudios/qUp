using Base.Interfaces;
using Managers.ApiManagers;
using Managers.PlayerManagers;
using Managers.PlayManagers;
using UnityEngine;
using static Managers.InputManagers.PointerInteractions;

namespace Managers.InputManagers {
    public class InputManagerBehaviour : MonoBehaviour, IManager {
        public Camera mainCamera;

        [Range(0, 1f)]
        public float cameraPanEdgePercentage;

        private Inputs inputs;

        public void RegisterClickable(IClickable clickable, GameObject gameObject) {
            pointerInteractions.AddClickable(clickable, gameObject);
        }

        public void RegisterHoverable(IHoverable hoverable, GameObject gameObject) {
            pointerInteractions.AddHoverable(hoverable, gameObject);
        }

        private void Awake() {
            ApiManager.ExposeManager(this);

            mainCamera = Camera.main;

            inputs = new Inputs();
            inputs.Enable();
            inputs.NoUnitSelected.Enable();
            inputs.General.Enable();
            inputs.General.Next.performed += _ => ApiManager.ProvideManager<PlayManager>().NextPhase();
            SetupPointer();
        }

        private PointerInteractions pointerInteractions;

        private void SetupPointer() {
            pointerInteractions =
                new PointerInteractionsBuilder(inputs)
                    .SetCamera(mainCamera)
                    .SetCoroutineHandlers(enumerator => StartCoroutine(enumerator), enumerator => StopCoroutine(enumerator))
                    .SetScreenEdgePercentage(cameraPanEdgePercentage)
                    .SetInteractionListener(interacted => OnClick(interacted))
                    .Build();
        }

        private void OnClick(IClickable currentHitGameObject) {
            currentHitGameObject.OnClick();
        }
    }
}
