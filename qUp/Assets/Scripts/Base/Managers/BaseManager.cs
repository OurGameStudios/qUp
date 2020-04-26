using System;
using Base.Interfaces;
using Managers;

namespace Base.Managers {
    public abstract class BaseManager<TState> : IManager  where TState : IState {

        private event Action<TState> State;
        
        public void InitBase(Action<TState> eventListener) {
            State = eventListener;
            GlobalManager.AddManager(this);
        }

        protected void SetState(TState state) => State?.Invoke(state);
    }
}
