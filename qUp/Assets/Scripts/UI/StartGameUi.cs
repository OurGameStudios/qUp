using Base.Singletons;
using Handlers.PhaseHandlers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class StartGameUi : SingletonMonoBehaviour<StartGameUi>, IPointerClickHandler {
        [SerializeField]
        private GameObject startGameUi;

        private bool isListening;

        public static void StartListeningForContinue() {
            Instance.isListening = true;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (isListening) {
                PhaseHandler.ContinuePhase();
                Destroy(startGameUi);
            }
        }
    }
}
