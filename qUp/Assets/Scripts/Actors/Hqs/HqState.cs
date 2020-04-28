using Base.Interfaces;

namespace Actors.Hqs {
    public abstract class HqState : IState { }

    public class BaseSelection : HqState {
        public bool IsSelected { get; }

        public BaseSelection(bool isSelected) => IsSelected = isSelected;
    }
}
