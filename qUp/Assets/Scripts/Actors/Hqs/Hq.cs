using Actors.Players;
using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Managers;
using Managers.GridManager;

namespace Actors.Hqs {
    public class Hq : BaseController<HqState> {
        public GridCoords Coords { get; private set; }
        
        public Player Owner { get; private set; }

        public void Init(GridCoords coords, Player owner) {
            Coords = coords;
            Owner = owner;
        }

        public void SetSelected(bool isSelected) {
            SetState(new HqSelection(isSelected));
        }
    }
}
