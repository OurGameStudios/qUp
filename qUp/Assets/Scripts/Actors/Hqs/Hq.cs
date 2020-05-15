using Actors.Players;
using Base.MonoBehaviours;
using Common;

namespace Actors.Hqs {
    public class Hq : BaseController<IHqState> {
        public GridCoords Coords { get; private set; }
        
        public Player Owner { get; private set; }

        public void Init(GridCoords coords, Player owner) {
            Coords = coords;
            Owner = owner;
        }

        public void SetSelected(bool isSelected) {
            SetState(HqSelection.With(isSelected));
        }
    }
}
