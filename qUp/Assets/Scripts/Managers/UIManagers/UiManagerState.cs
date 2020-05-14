using Base.Interfaces;

namespace Managers.UIManagers {
    
    public interface IUiManagerState : IState { }
    public class UiManagerState<TState> : State<TState>, IUiManagerState where TState : class, new() { }
}
