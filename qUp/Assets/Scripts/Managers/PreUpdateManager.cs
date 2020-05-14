using System;
using Base.Interfaces;
using Managers.ApiManagers;
using UnityEngine;

namespace Managers {
    public class PreUpdateManager : MonoBehaviour, IManager {

        private event Action Preupdate;
        private void Awake() {
            // GlobalManager.ExposeManager(this);
            ApiManager.ExposeManager(this);
        }

        private void Update() { Preupdate?.Invoke(); }
        
        public void SubscribeToPreUpdate(Action onPreUpdate) => Preupdate += onPreUpdate;
    }
}
