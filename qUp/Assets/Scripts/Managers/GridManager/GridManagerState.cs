using Base.Interfaces;

namespace Managers.GridManager {
    public abstract class GridManagerState : IState { }

    public class UnitSelected : GridManagerState {
        public object Unit { get; }

        public UnitSelected(object unit) {
            Unit = unit;
        }
    }

    public class GroupSelected : GridManagerState { }

    public class BaseSelected : GridManagerState { }
}
