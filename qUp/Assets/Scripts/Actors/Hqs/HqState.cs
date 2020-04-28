using Base.Interfaces;

namespace Actors.Hqs {
    public abstract class HqState : IState { }

    public class HqSelection : HqState {
        public bool IsSelected { get; }

        public HqSelection(bool isSelected) => IsSelected = isSelected;
    }
}
