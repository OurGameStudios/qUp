using Actors.Units;
using Base.Interfaces;

namespace Managers.UIManagers {
    public class UiManagerState : IState { }

    public class BaseSelected : UiManagerState {
        public string Name { get; }

        public BaseSelected(string name) {
            Name = name;
        }
        
        public class UnitToSpawnSelected : UiManagerState {
            public UnitData UnitData;

            public UnitToSpawnSelected(UnitData unitData) {
                UnitData = unitData;
            }
        }
    }
}
