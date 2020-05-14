using Base.Interfaces;

namespace Actors.Players {
    public interface IPlayerState : IState { }
    public abstract class PlayerState<TState> : State<TState>, IPlayerState where TState : class, new() { }
}
