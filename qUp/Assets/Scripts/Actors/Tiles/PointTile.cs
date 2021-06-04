using Handlers;

namespace Actors.Tiles {
    public class PointTile : Tile {

        private readonly int pointIncrease = Configuration.GetPointTileIncrease();

        protected override void OnPreppingPhase() {
            if (Owner != null) {
                Owner.Points += pointIncrease;
            }
            base.OnPreppingPhase();
        }
    }
}
