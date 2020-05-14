using System;
using Base.Interfaces;
using Managers;
using Managers.ApiManagers;

namespace Base.Managers {
    public abstract class BaseManager<TState> : IManager  where TState : IState {

        private event Action<TState> State;
        
        public void InitBase(Action<TState> eventListener) {
            State = eventListener;
            // GlobalManager.ExposeManager(this);
            ApiManager.ExposeManager(this);
        }

        protected void SetState(TState state) => State?.Invoke(state);
    }
}
