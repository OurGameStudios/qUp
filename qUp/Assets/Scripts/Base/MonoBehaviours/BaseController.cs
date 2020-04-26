using System;
using Base.Interfaces;
using Managers.ApiManagers;

namespace Base.MonoBehaviours {
    public abstract class BaseController<TState>  where TState : IState {

        protected virtual bool Expose => false;
        
        private event Action<TState> State;

        public void InitBase(Action<TState> eventListener) {
            State = eventListener;
            if (Expose) {
                ApiManager.Expose(this);
            }
        }

        protected void SetState(TState state) => State?.Invoke(state);
    }
}
