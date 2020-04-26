using Actors.Players;
using Base;
using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Managers;
using Managers.GridManager;

namespace Actors.PlayerBases {
    public class PlayerBase : BaseController<PlayerBaseState>, IClickable {
        public GridCoords Coords { get; private set; }
        
        public Player Owner { get; private set; }

        public void Init(GridCoords coords, Player owner) {
            Coords = coords;
            Owner = owner;
        }

        public void OnClick() {
            GlobalManager.GetManager<GridManager>().SelectBase(this);
        }

        public void SetSelected(bool isSelected) {
            SetState(new BaseSelection(isSelected));
        }
    }
}
