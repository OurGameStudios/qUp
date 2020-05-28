using System;
using Actors.Players;
using Base.Interfaces;
using Common;
using Extensions;
using Managers.ApiManagers;

namespace UI.ResourceUis {
    public class ResourceUiInteractor : IBaseInteractor {
        private WeakReference<ResourceUi> resourceUi;
        
        private LazyWeakReference<PlayerInteractor> playerInteractor =
            new LazyWeakReference<PlayerInteractor>(ApiManager.ProvideInteractor<PlayerInteractor>);
        public void AddExposed<TExposed>(TExposed exposed) {
            resourceUi = new WeakReference<ResourceUi>(exposed as ResourceUi);
        }

        public void RefreshIncome() => resourceUi.GetOrNull()?.RefreshPlayerIncome();

        public void RefreshUpkeep() => resourceUi.GetOrNull()?.RefreshPlayerUpkeep();

        public void RefreshAll() => resourceUi.GetOrNull()?.RefreshAll();
    }
}
