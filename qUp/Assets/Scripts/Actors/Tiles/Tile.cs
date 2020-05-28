using Actors.Grid.Generator;
using Actors.Players;
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
        private Vector3 tilePosition;

        private Color hoverHighlightColor = Color.white;
        private Color? highlightColor;

        private Player owner;

        public void Init(GridCoords coords, Vector3 position, GameObject gameObject) {
            Coords = coords;
            tilePosition = position;
            gridManager.RegisterTile(this);
            ApiManager.ProvideManager<InputManagerBehaviour>().Let(it => {
                    it.RegisterClickable(this, gameObject);
                    it.RegisterHoverable(this, gameObject);
                });
        }

        public void InitColors(Color color) {
            
        }

        public void OnClick() {
            gridManager.SelectTile(Coords);
        }

        public void OnSecondaryClick() {
            gridManager.SelectUnitPath(Coords);
        }

        public void OnHoverStart() {
            SetState(HighlightActivated.With(hoverHighlightColor));
        }

        public void OnHoverEnd() {
            if (highlightColor != null) {
                SetState(HighlightActivated.With(highlightColor ?? hoverHighlightColor));
            } else {
                SetState(Idle.With());
            }
            
        }

        public void ActivateHighlight(Color? color = null) {
            color?.Let(it => highlightColor = it);
            SetState(HighlightActivated.With(color ?? hoverHighlightColor));
        }

        public void DeactivateHighlight() {
            highlightColor = null;
            SetState(Idle.With());
        }

        public Vector3 ProvideTilePosition() {
            var positionHeight = gridInteractor.SampleTerrain(new Vector2(tilePosition.x, tilePosition.z)) ?? 0f;
            return new Vector3(tilePosition.x, positionHeight, tilePosition.z);
        }

        public void SetOwnership(Player owner) {
            this.owner = owner;
            SetState(OwnershipChanged.With(owner.PlayerColor));
        }

        public Player GetOwner() => owner;
    }
}
