using Base.Interfaces;

namespace Actors.Units {
    public abstract class UnitState : IState { }

    public class UnitSelected : UnitState { }
}
