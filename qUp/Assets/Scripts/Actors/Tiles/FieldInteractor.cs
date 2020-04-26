using Base.Interfaces;
using Common;

namespace Actors.Tiles {
    public class FieldInteractor : IBaseInteractor {
        private WeakList<Tile> fields = new WeakList<Tile>();

        public void AddExposed<TExposed>(TExposed exposed) {
            fields.Add(exposed as Tile);
        }
    }
}
