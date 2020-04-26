using Base.Interfaces;

namespace Managers.UIManagers {
    public class UiManagerState : IState { }

    public class BaseSelected : UiManagerState {
        public string Name { get; }

        public BaseSelected(string name) {
            Name = name;
        }
    }
}
