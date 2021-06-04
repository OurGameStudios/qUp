using Actors.Players;
using Base.Singletons;
using Handlers;
using Handlers.PhaseHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public class PlayerChangeUi : SingletonMonoBehaviour<PlayerChangeUi>, IPointerClickHandler {

        [SerializeField]
        private GameObject playerChangeUi;

        [SerializeField]
        private TextMeshProUGUI playerChangeDescription;

        public static void ListenToPhaseManager() {
            Instance.AddToDispose(PhaseHandler.PlayerChange.Subscribe(Instance.OnPlayerChange));
        }

        public void OnPointerClick(PointerEventData eventData) {
            PhaseHandler.ContinuePhase();
            playerChangeUi.SetActive(false);
        }

        public void OnPlayerChange(IPlayer player) {
            Instance.playerChangeUi.SetActive(true);
            playerChangeDescription.text = string.Format(Localization.NEXT_PLAYER_IS, player.GetPlayerName());
        }
    }
}
