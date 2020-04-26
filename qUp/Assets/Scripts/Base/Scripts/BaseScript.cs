using Base.Interfaces;
using Base.MonoBehaviours;
using Extensions;

namespace Base.Scripts {
    public abstract class BaseScript<TController, TState>
        where TController : BaseController<TState>, new()
        where TState : IState {
        protected BaseScript() => Controller = new TController().Also(it => it.InitBase(OnStateHandler));
        protected TController Controller { get; }

        protected abstract void OnStateHandler(TState inState);
    }
}
