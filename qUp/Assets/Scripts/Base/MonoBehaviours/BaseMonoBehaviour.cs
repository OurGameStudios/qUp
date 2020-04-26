using Base.Interfaces;
using Extensions;
using UnityEngine;

namespace Base.MonoBehaviours {
    public abstract class BaseMonoBehaviour<TController, TState> : MonoBehaviour
        where TController : BaseController<TState>, new()
        where TState : IState {
        protected TController Controller { get; private set; }

        protected abstract void OnStateHandler(TState inState);

        private void Awake() {
            Controller = new TController().Also(it => it.InitBase(OnStateHandler));
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
