using Base.Interfaces;

namespace Managers.GridManagers {
    
    public interface IGridManagerState : IState { }
    public abstract class GridManagerState<TState> : State<TState>, IGridManagerState where TState : class, new() { }
}
