using System.Collections.Generic;
using System.Linq;
using Actors.Tiles;
using Actors.Units;
using Base.Singletons;
using Handlers.PhaseHandlers;

namespace Handlers {
    public class ResourceUnitsHandler : SingletonClass<ResourceUnitsHandler> {

        private readonly Dictionary<ITile, List<ResourceUnit>> resourceUnits = new Dictionary<ITile, List<ResourceUnit>>();

        private static Dictionary<ITile, List<ResourceUnit>> ResourceUnits => Instance.resourceUnits;

        public static void SubscribeToPhaseManager() {
            Instance.AddToDispose(PhaseHandler.PreExecutionPhase.Subscribe(OnPreExecutionPhase));
        }

        public static void AddToActiveUnits(ResourceUnit resourceUnit) {
            if (!ResourceUnits.ContainsKey(resourceUnit.GetOriginTile())) {
                ResourceUnits.Add(resourceUnit.GetOriginTile(), new List<ResourceUnit>());
            }

            if (!ResourceUnits[resourceUnit.GetOriginTile()].Contains(resourceUnit)) {
                ResourceUnits[resourceUnit.GetOriginTile()].Add(resourceUnit);
            }
        }

        public static void RemoveFromActiveUnits(ResourceUnit resourceUnit) {
            if (ResourceUnits[resourceUnit.GetOriginTile()].Contains(resourceUnit)) {
                ResourceUnits[resourceUnit.GetOriginTile()].Remove(resourceUnit);
            }
        }

        private static void OnPreExecutionPhase() {
            foreach (var resourceUnit in ResourceUnits.Values.SelectMany(resourceUnits => resourceUnits)) {
                resourceUnit.PlanPath();
            }
            PhaseHandler.ContinuePhase();
        }
    }
}
