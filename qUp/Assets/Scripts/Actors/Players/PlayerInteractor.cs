using System.Collections.Generic;
using Base;
using Base.Interfaces;
using Common;

namespace Actors.Players {
    public class PlayerInteractor : IBaseInteractor {
        private readonly List<Player> players = new List<Player>();

        public void AddExposed<TExposed>(TExposed exposed) {
            players.Add(exposed as Player);
        }

        public List<(GridCoords coords, Player owner)> GetPlayerBases() => players.ConvertAll(it => it.GetBaseInfo());
    }
}
