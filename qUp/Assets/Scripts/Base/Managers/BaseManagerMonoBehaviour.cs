using Base.Interfaces;
using Extensions;
using UnityEngine;

namespace Base.Managers {
    public abstract class BaseManagerMonoBehaviour<TController, TState> : MonoBehaviour
        where TController : BaseManager<TState>, new()
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
