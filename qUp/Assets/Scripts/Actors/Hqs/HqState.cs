using Base.Interfaces;

namespace Actors.Hqs {
    public interface IHqState : IState { }

    public abstract class HqState<TState> : State<TState>, IHqState where TState : class, new() { }

    public class HqSelection : HqState<HqSelection> {
        public bool IsSelected { get; private set; }

        public static HqSelection With(bool isSelected) {
            Cache.IsSelected = isSelected;
            return Cache;
        }
    }
}
