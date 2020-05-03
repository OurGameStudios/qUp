using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Common {
    public static class Pathfinder {

        public static Dictionary<GridCoords, GridCoords?> FindRangeWeighted(GridCoords start, int movementRange) {
            var frontier = new PriorityQueue<int, GridCoords>();
            var cameFrom = new Dictionary<GridCoords, GridCoords?>();
            var costSoFar = new Dictionary<GridCoords, int>();
            
            frontier.Add(0, start);
            cameFrom.Add(start, null);
            costSoFar.Add(start, 0);

            var safetyBreak = 100;
            var safetyCount = 0;

            while (!frontier.IsEmpty() && safetyCount < safetyBreak) {
                var current = frontier.Pop();
                
                //If point found break

                //get neighbour cords should return coords with matching tick point// max should be max of grid
                var neighbours = current.GetNeighbourCoords();
                foreach (var next in neighbours) {
                    var newCost = costSoFar[current] + 1;

                    //newCost > movementRange to limit the range of search
                    if (costSoFar.ContainsKey(next)) if(newCost >= costSoFar[next]) continue;
                    if (newCost > movementRange + 1) continue;
                    costSoFar.AddOrUpdate(next, newCost);
                    frontier.Add(newCost, next);
                    cameFrom.AddOrUpdate(next, current);
                     
                }

                safetyCount++;
            }

            return cameFrom;
        }

        public static void FindRange(GridCoords start, int movementRange) {
            var visited = new HashSet<GridCoords>();
            visited.Add(start);
            var fringes = new List<List<GridCoords>>();
            fringes.Add(new List<GridCoords>{start});

            for (int i = 1; i < movementRange; i++) {
                fringes.Add(new List<GridCoords>());

                foreach (var coord in fringes[i-1]) {
                    for (var j = 0; j < 6; j++) {
                        var neighbour = coord.GetNeighbourCoords()[j];

                        // Also needs if not blocked!
                        if (!visited.Contains(neighbour)) {
                            visited.Add(neighbour);
                            fringes[i].Add(neighbour);
                        }
                    }
                }
            }
        }
    }
}
