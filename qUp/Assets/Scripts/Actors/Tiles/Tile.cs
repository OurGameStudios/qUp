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
        private Color markingsColor;
        private Vector3 tilePosition;

        public void Init(GridCoords coords, Vector3 position) {
            Coords = coords;
            tilePosition = position;
            GlobalManager.GetManager<GridManager>().RegisterTile(this);
        }

        public void InitMarkings(Color color) {
            markingsColor = color;
        }

        public void OnClick() {
            GlobalManager.GetManager<GridManager>().SelectTile(Coords);
        }

        public void ResetMarkings() {
            SetState(new MarkingsChange(markingsColor));
        }

        public void ApplyMarkings(Color color) {
            SetState(new MarkingsChange(color));
        }

        public void SetMarkings(Color color) {
            markingsColor = color;
            SetState(new MarkingsChange(color));
        }

        public void ActivateHighlight(Color color) {
            SetState(new HighlightActivated(color.IsNull() ? Color.white : color));
        }

        public void DeactivateHighlight() {
            SetState(new Idle());
        }

        public Vector3 ProvideSpawnPoint() {
            return tilePosition;
        }
    }
}
