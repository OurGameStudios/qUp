using System;
using UnityEngine;

namespace Base.MonoBehaviours {
    public class BaseListenerMonoBehaviour : MonoBehaviour {
        private event Action Unsubscribe;

        protected void AddToDispose(Action disposeAction) {
            Unsubscribe += disposeAction;
        }

        protected virtual void OnDestroy() {
            Dispose();
        }

        protected void Dispose() {
            Unsubscribe?.Invoke();
            Unsubscribe = null;
        }
    }
}
