using Actors.Grid.Generator;
using Base.Interfaces;
using Base.MonoBehaviours;
using Common;
using Extensions;
using JetBrains.Annotations;
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
        private Color baseColor;
        private Color highlightColor;
        private Color currentBaseColor;

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
            baseColor = color;
            highlightColor = color;
            currentBaseColor = color;
        }

        public void OnClick() {
            gridManager.SelectTile(Coords);
        }

        public void OnHoverStart() {
            SetState(HighlightActivated.With(currentBaseColor, highlightColor));
        }

        public void OnHoverEnd() {
            SetState(Idle.With(currentBaseColor));
        }

        public void ActivateHighlight(Color? baseHighlightColor = null, Color? color = null) {
            baseHighlightColor?.Let(it => currentBaseColor = it);
            SetState(HighlightActivated.With(baseHighlightColor ?? currentBaseColor, color ?? highlightColor));
        }

        public void DeactivateHighlight() {
            currentBaseColor = baseColor;
            SetState(Idle.With(currentBaseColor));
        }

        public Vector3 ProvideTilePosition() {
            var positionHeight = gridInteractor.SampleTerrain(new Vector2(tilePosition.x, tilePosition.z)) ?? 0f;
            return new Vector3(tilePosition.x, positionHeight, tilePosition.z);
        }
    }
}
