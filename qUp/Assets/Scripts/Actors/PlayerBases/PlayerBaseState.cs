using Base;
using Base.Interfaces;

namespace Actors.PlayerBases {
    public abstract class PlayerBaseState : IState { }

    public class BaseSelection : PlayerBaseState {
        public bool IsSelected { get; }

        public BaseSelection(bool isSelected) => IsSelected = isSelected;
    }
}
