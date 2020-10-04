using System;
using System.Threading;
using System.Threading.Tasks;
using Base.Interfaces;
using Common.Interaction;
using Managers.ApiManagers;
using Managers.PlayManagers;
using UnityEngine;
using static Managers.InputManagers.PointerInteractions;

namespace Managers.InputManagers {
    public class InputManagerBehaviour : MonoBehaviour, IManager {
        
        private Lazy<PlayManager> playManagerLazy = new Lazy<PlayManager>(ApiManager.ProvideManager<PlayManager>);
        private PlayManager PlayManager => playManagerLazy.Value;
        
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
            inputs.NextPlayer.Enable();
            inputs.NextPlayer.Next.performed += _ => PlayManager.NextPhase();
            SetupPointer();
        }

        private PointerInteractions pointerInteractions;

        private void SetupPointer() {
            pointerInteractions =
                new PointerInteractionsBuilder(inputs)
                    .SetCamera(mainCamera)
                    .SetCoroutineHandlers(enumerator => StartCoroutine(enumerator), enumerator => StopCoroutine(enumerator))
                    .SetScreenEdgePercentage(cameraPanEdgePercentage)
                    .Build();
        }

        private void DisablePlanningPhase() {
            inputs.PlanningPhase.Disable();
            inputs.NoUnitSelected.Disable();
            inputs.UnitSelected.Disable();
            pointerInteractions.CleanPlanningPhase();
        }

        private void DisableExecutionPhase() {
            inputs.ExecutionInteractions.Disable();
         }

        private void DisablePreppingPhase() { }

        public void OnPlanningPhase() {
            DisablePreppingPhase();
            // This needs to be delayed because Disabling is sometimes run right before and racing condition affetcs enabling
            Task.Delay(500).ContinueWith(task => {
                inputs.NextPlayer.Enable();
                task.Dispose();
            });
            
            inputs.PlanningPhase.Enable();
            inputs.NoUnitSelected.Enable();
        }

        public void OnExecutionPhase() {
            DisablePlanningPhase();
            inputs.ExecutionInteractions.Enable();
            inputs.NextPlayer.Disable();
        }

        public void OnPreppingPhase() {
            DisableExecutionPhase();
        }

        public void OnUnitSelected() {
            inputs.NoUnitSelected.Disable();
            inputs.UnitSelected.Enable();
        }

        public void OnUnitDeselected() {
            inputs.NoUnitSelected.Enable();
            inputs.UnitSelected.Disable();
        }
    }
}
