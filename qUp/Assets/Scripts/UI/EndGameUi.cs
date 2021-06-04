using Actors.Players;
using Base.Singletons;
using Extensions;
using Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class EndGameUi : SingletonMonoBehaviour<EndGameUi>, IPointerClickHandler {
        
        [SerializeField]
        private TextMeshProUGUI endGameDescription;

        [SerializeField]
        private GameObject endGameUi;

        private bool isListening;

        public static void ShowEndGameUi(IPlayer winner) {
            Instance.endGameUi.gameObject.SetActive(true);
            Instance.endGameDescription.text = Localization.END_GAME_DESCRIPTION.Format(winner);
            Instance.isListening = true;
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (isListening) {
                Loader.EndGame();
            }
        }
    }
}
