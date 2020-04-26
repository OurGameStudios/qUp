using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
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
            
        }

        public void SetHighlighted(Color color) {
            SetState(new Highlight(color));
        }
    }
}
