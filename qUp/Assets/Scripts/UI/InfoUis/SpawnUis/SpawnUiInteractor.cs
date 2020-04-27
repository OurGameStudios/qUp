using System;
using Base.Interfaces;
using Extensions;
using UnityEngine;

namespace UI.InfoUis.SpawnUis {
    public class SpawnUiInteractor : IBaseInteractor {
        private WeakReference<SpawnUi> unitSpawnUi;

        public void AddExposed<TExposed>(TExposed exposed) {
            unitSpawnUi = new WeakReference<SpawnUi>(exposed as SpawnUi);
        }

        public void ShowMenu(Sprite sprite, string name, int cost, int hp, int att, int tp) =>
            unitSpawnUi.GetOrNull()?.ShowMenu(sprite, name, cost, hp, att, tp);

        public void HideMenu() => unitSpawnUi.GetOrNull()?.HideMenu();
    }
}
