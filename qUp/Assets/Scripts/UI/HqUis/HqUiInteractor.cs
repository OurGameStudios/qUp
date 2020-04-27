using System;
using Actors.Players;
using Base.Interfaces;
using Common;
using Extensions;
using Managers.ApiManagers;

namespace UI.HqUis {
    public class HqUiInteractor : IBaseInteractor {
        private WeakReference<HqUi> hqUi;

        private LazyWeakReference<PlayerInteractor> playerInteractor =
            new LazyWeakReference<PlayerInteractor>(ApiManager.ProvideInteractor<PlayerInteractor>);

        public void AddExposed<TExposed>(TExposed exposed) {
            hqUi = new WeakReference<HqUi>(exposed as HqUi);
        }

        public void InitHqUi() => hqUi.GetOrNull()?.Init(playerInteractor.GetOrNull()?.GetPlayerUnits());
    }
}
