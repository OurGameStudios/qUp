namespace Base.Interfaces {
    public abstract class State<TState> : IState where TState : class, new() {
        private static TState _cache;
        protected static TState Cache => _cache ?? (_cache = new TState());
    }

    public interface IState { }
}
