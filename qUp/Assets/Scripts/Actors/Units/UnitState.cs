using Base.Interfaces;

namespace Actors.Units {
    public interface IUnitState : IState { }

    public abstract class UnitState<TState> : State<TState>, IUnitState where TState : class, new() { }

    public class UnitSelected : UnitState<UnitSelected> {
        public static UnitSelected Where() => Cache;
    }
}
