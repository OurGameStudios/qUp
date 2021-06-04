using System.Collections.Generic;
using System.Linq;
using Actors.Players;
using Actors.Units;

namespace Handlers {
    public static class CombatHandler {
        private static bool AreSameCombatants(List<IUnit> combatants) {
            var combatantData = combatants[0].Data;
            return combatants.TrueForAll(unit => unit.Data == combatantData);
        }

        public static IPlayer GetCombatWinner(List<IUnit> combatants) {
            var combatantHps = combatants.ToDictionary(unit => unit, unit => 
                unit.Hp);
            
            if (AreSameCombatants(combatants)) {
                return null;
            }
            while (!combatantHps.ContainsValue(0)) {
                foreach (var combatant in combatants) {
                    foreach (var otherCombatant in combatants.Where(unit => unit != combatant)) {
                        combatantHps[otherCombatant] -= combatant.GetDamageFor(otherCombatant);
                    }
                }
            }

            var winner = combatantHps.FirstOrDefault(pair => pair.Value != 0).Key;
            return winner.Owner;
        }
    }
}
