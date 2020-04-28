using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Managers;
using Managers.GridManager;
using UnityEngine;

namespace Actors.Tiles {
    public class Tile : BaseController<TileState>, IClickable {
        public GridCoords Coords { get; private set; }

        public void Init(GridCoords coords) {
            Coords = coords;
            GlobalManager.GetManager<GridManager>().RegisterTile(this);
        }

        public void OnClick() {
            GlobalManager.GetManager<GridManager>().SelectTile(Coords);
        }

        public void SetMarkings(Color color) {
            SetState(new MarkingsChange(color));
        }

        public void ActivateHighlight(Color color) {
            SetState(new HighlightActivated(color.IsNull() ? Color.white : color));
        }

        public void DeactivateHighlight() {
            SetState(new Idle());
        }
    }
}
