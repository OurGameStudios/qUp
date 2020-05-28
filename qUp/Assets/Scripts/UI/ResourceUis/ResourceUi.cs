using System;
using Actors.Players;
using Base.MonoBehaviours;
using Managers.ApiManagers;
using Managers.PlayerManagers;

namespace UI.ResourceUis {
    public class ResourceUi : BaseController<IResourceUiState> {
        protected override bool Expose => true;

        private readonly Lazy<PlayerManager> playerManagerLazy =
            new Lazy<PlayerManager>(ApiManager.ProvideManager<PlayerManager>);
        
        private PlayerManager PlayerManager => playerManagerLazy.Value;

        private Player currentPlayer;

        public void RefreshPlayerIncome() {
            currentPlayer = PlayerManager.GetCurrentPlayer();
            SetState(IncomeChanged.Where(currentPlayer.GetIncome().ToString(), $"{currentPlayer.GetAvailableIncome()}"));
        }

        public void RefreshPlayerUpkeep() {
            currentPlayer = PlayerManager.GetCurrentPlayer();
            SetState(UpkeepChanged.Where((-currentPlayer.GetUpkeep()).ToString(), $"{currentPlayer.GetAvailableIncome()}"));
        }

        public void RefreshAll() {
            currentPlayer = PlayerManager.GetCurrentPlayer();
            SetState(IncomeChanged.Where(currentPlayer.GetIncome().ToString(), $"{currentPlayer.GetAvailableIncome()}"));
            SetState(UpkeepChanged.Where(currentPlayer.GetUpkeep().ToString(), $"{currentPlayer.GetAvailableIncome()}"));
        }
    }
}
