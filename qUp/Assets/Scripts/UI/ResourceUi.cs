using Actors.Players;
using Base.Singletons;
using Extensions;
using Handlers;
using Handlers.PhaseHandlers;
using TMPro;
using UnityEngine;

namespace UI {
    public class ResourceUi : SingletonMonoBehaviour<ResourceUi> {

        [SerializeField]
        private TextMeshProUGUI totalResource;
        
        [SerializeField]
        private TextMeshProUGUI availableResource;

        public static void ListenToPhaseManager() {
            Instance.AddToDispose(PhaseHandler.PlanningPhase.Subscribe(Instance.ShowPlayerResource));
            Instance.AddToDispose(PhaseHandler.PlayerChange.Subscribe(HideResourceUi));
            Instance.AddToDispose(PhaseHandler.ExecutionPhase.Subscribe(HideResourceUi));
        }

        private void ShowPlayerResource(IPlayer player) {
            gameObject.SetActive(true);
            totalResource.text = Localization.TOTAL_RESOURCE.Format(player.TotalIncome);
            availableResource.text = Localization.AVAILABLE_RESOURCE.Format(player.TotalIncome);
        }

        private static void HideResourceUi(IPlayer _) => HideResourceUi();

        private static void HideResourceUi() {
            Instance.gameObject.SetActive(false);
        }

        public static void UpdatePlayerResource(IPlayer player) {
            Instance.totalResource.text = Localization.TOTAL_RESOURCE.Format(player.TotalIncome);
            Instance.availableResource.text = Localization.AVAILABLE_RESOURCE.Format(player.AvailableIncome);
        }
    }
}
