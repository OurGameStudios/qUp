using System;
using System.Collections.Generic;
using System.Linq;
using Actors.Grid.Generator;
using Actors.Players;
using Actors.Tiles;
using Base.Interfaces;

namespace Managers.ApiManagers {
    public class ApiManager {

        private static ApiManager instance;

        private static ApiManager Instance => instance ?? (instance = new ApiManager());

        private readonly Dictionary<Type, IBaseInteractor> interactors =
            new Dictionary<Type, IBaseInteractor> {
                                                  {typeof(Player), new PlayerInteractor()},
                                                  {typeof(GridGenerator), new GridInteractor()},
                                                  {typeof(Tile), new FieldInteractor()},
                                              };

        public static void Expose<TExposed>(TExposed exposed) {
            Instance.interactors[exposed.GetType()].AddExposed(exposed);
        }

        public static TInteractor ProvideInteractor<TInteractor>() where TInteractor : IBaseInteractor {
            return (TInteractor)  Instance.interactors.Values.FirstOrDefault(it => it is TInteractor);
        }

        public static void Clear() {
            instance = null;
        }
    }
}
