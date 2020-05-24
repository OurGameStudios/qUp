using System;
using System.Collections;
using Base.Interfaces;
using Managers.ApiManagers;
using UnityEngine;

namespace Managers {
    public class CoroutineHandler : MonoBehaviour, IManager {
        private void Awake() {
            ApiManager.ExposeManager(this);
            onNextFrame = OnNextFrame();
        }

        //TODO this logic should be developed and implemented
        private IEnumerator onNextFrame;
        private event Action OnNextFrameEvent;

        public void DoOnNextFrame(Action doOnNextFrame) {
            OnNextFrameEvent += doOnNextFrame;
            StartCoroutine(OnNextFrame());
        }

        private IEnumerator OnNextFrame() {
            yield return this;
            OnNextFrameEvent?.Invoke();
            OnNextFrameEvent = null;
            yield return this;
        }

        // public void StartCoroutine(Action<IEnumerator> enumerator) {
        //     StartCoroutine(enumerator);
        // }
        //
        // public void StopCoroutine(IEnumerator enumerator) {
        //     StopCoroutine(enumerator);
        // }
    }
}
