using Actors.Players;
using Common;

namespace Actors.Hqs {
    public interface IHq {
        public IPlayer GetOwner();

        public GridCoords GetCoords();
    }
}
