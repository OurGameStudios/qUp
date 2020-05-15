using Base.Interfaces;
using Common;

namespace Actors.Tiles {
    public class TileInteractor : IBaseInteractor {
        private readonly WeakList<Tile> tiles = new WeakList<Tile>();

        public void AddExposed<TExposed>(TExposed exposed) {
            tiles.Add(exposed as Tile);
        }
    }
}
