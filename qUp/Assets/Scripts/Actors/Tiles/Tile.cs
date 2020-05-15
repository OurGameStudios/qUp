using Actors.Grid.Generator;
using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Managers.ApiManagers;
using Managers.GridManagers;
using Managers.InputManagers;
using UnityEngine;

namespace Actors.Tiles {
    public class Tile : BaseController<ITileState>, IClickable, IHoverable {
        private readonly GridInteractor gridInteractor = ApiManager.ProvideInteractor<GridInteractor>();
        private readonly GridManager gridManager = ApiManager.ProvideManager<GridManager>();

        public GridCoords Coords { get; private set; }
        private Color markingsColor;
        private Vector3 tilePosition;

        public void Init(GridCoords coords, Vector3 position, GameObject gameObject) {
            Coords = coords;
            tilePosition = position;
            gridManager.RegisterTile(this);
            ApiManager.ProvideManager<InputManagerBehaviour>().Let(it => {
                    it.RegisterClickable(this, gameObject);
                    it.RegisterHoverable(this, gameObject);
                });
        }

        public void InitMarkings(Color color) {
            markingsColor = color;
        }

        public void OnClick() {
            gridManager.SelectTile(Coords);
        }

        public void OnHoverStart() {
            SetState(HighlightActivated.With(Color.white));
        }

        public void OnHoverEnd() {
            SetState(new Idle());
        }

        public void ResetMarkings() {
            SetState(MarkingsChange.With(markingsColor));
        }

        public void ApplyMarkings(Color color) {
            SetState(MarkingsChange.With(color));
        }

        public void SetMarkings(Color color) {
            markingsColor = color;
            SetState(MarkingsChange.With(color));
        }

        public void ActivateHighlight(Color? color) {
            SetState(HighlightActivated.With(color ?? Color.white));
        }

        public void DeactivateHighlight() {
            SetState(Idle.With());
        }

        public Vector3 ProvideTilePosition() {
            var positionHeight = gridInteractor.SampleTerrain(new Vector2(tilePosition.x, tilePosition.z)) ?? 0f;
            return new Vector3(tilePosition.x, positionHeight, tilePosition.z);
        }
    }
}
