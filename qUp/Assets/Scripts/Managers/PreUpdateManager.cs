using System;
using Base.Interfaces;
using UnityEngine;

namespace Managers {
    public class PreUpdateManager : MonoBehaviour, IManager {

        private event Action Preupdate;
        private void Awake() {
            GlobalManager.AddManager(this);
        }

        private void Update() { Preupdate?.Invoke(); }

        public void SubscribeToPreUpdate(Action onPreUpdate) => Preupdate += onPreUpdate;
    }
}
