using System;
using Base.Interfaces;
using UI.ResourceUis;

namespace Managers.UIManagers {
    
    public interface IUiManagerState : IState { }
    public class UiManagerState<TState> : State<TState>, IUiManagerState where TState : class, new() { }
    
    public class PhaseShow : UiManagerState<PhaseShow> {
        public string PhaseText { get; private set; }

        public static PhaseShow Where(String phaseText) {
            Cache.PhaseText = phaseText;
            return Cache;
        }
    }
    
    public class PhaseHide : UiManagerState<PhaseHide> {
        public static PhaseHide Where() => Cache;
    }
}
