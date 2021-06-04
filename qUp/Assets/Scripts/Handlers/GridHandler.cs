using System.Collections.Generic;
using Actors.Hqs;
using Actors.Tiles;
using Actors.Units;
using Base.Singletons;
using Common;
using Handlers.PlayerHandlers;

namespace Handlers {
    public class GridHandler : SingletonClass<GridHandler> {
        private readonly Dictionary<GridCoords, ITile> tiles = new Dictionary<GridCoords, ITile>();

        private Dictionary<ITile, ITile> pathsInRange = new Dictionary<ITile, ITile>();
        private static ref Dictionary<ITile, ITile> PathsInRangeRef => ref Instance.pathsInRange;
        private static Dictionary<ITile, ITile> PathsInRange => Instance.pathsInRange;

        private readonly List<ITile> path = new List<ITile>();

        /// <summary>
        /// Path is in correct order. First tile is the origin and the last one is the destination.
        /// </summary>
        private static List<ITile> Path => Instance.path;

        private IUnit selectedUnit;
        private static IUnit SelectedUnit => Instance.selectedUnit;

        private GridCoords maxCoords;

        public static GridCoords MaxCoords => Instance.maxCoords;

        public static void AddTile(ITile tile) {
            Instance.tiles.Add(tile.GetCoords(), tile);
            if (tile.GetCoords() > MaxCoords) {
                Instance.maxCoords = tile.GetCoords();
            }
        }

        public static void AddHq(IHq hq) {
            if (hq.GetCoords() > MaxCoords) {
                Instance.maxCoords = hq.GetCoords();
            }
        }

        public static bool TryGetTile(GridCoords coords, out ITile tile) =>
            Instance.tiles.TryGetValue(coords, out tile);

        public static void OnUnitSelected(IUnit unit, ITile tile) {
            if (unit != SelectedUnit) {
                OnUnitDeselected();
            }

            Pathfinder.GetPaths(PlayerHandler.GetCurrentPlayer(), tile, 0, unit, ref PathsInRangeRef);
            Instance.selectedUnit = unit;
            ShowUnitRange();
            ShowUnitPath();
        }

        public static void OnUnitDeselected() {
            foreach (var tile in PathsInRange.Keys) {
                tile.ResetPlanningPhaseState(PlayerHandler.GetCurrentPlayer());
            }

            PathsInRange.Clear();
            Path.Clear();
            Instance.selectedUnit = null;
            // TODO optimization point: You could store pathfinders range for a unit and use it next time instead of calculating
        }

        private static void ShowUnitRange() {
            foreach (var tile in PathsInRange.Keys) {
                tile.OnPathRange();
            }
        }

        private static void ShowUnitPath() {
            if (SelectedUnit.Path == null) return;
            Path.Clear();
            Path.AddRange(SelectedUnit.Path);
            foreach (var tile in SelectedUnit.Path) {
                tile.ShowSetPath();
            }
        }

        /// <summary>
        /// Setting a path should clear units previous path and clear the unit from the tiles on its respectable
        /// ticks. It should set a path to the unit and set its respectable clicks to tiles.
        /// </summary>
        /// <param name="destination"></param>
        public static void SetPath(ITile destination) {
            // Remove all but 0th tick from path before setting a new path
            for (var i = 0; i < Path.Count; i++) {
                var tile = Path[i];
                tile.RemoveUnitFromTick(PlayerHandler.GetCurrentPlayer(), i, i == Path.Count - 1);
                if (i != 0) {
                    tile.OnPathRange();
                }
            }

            Path.Clear();
            var next = destination;

            do {
                // We need to reverse here because key is destination and value is origin
                Path.Insert(0, next);
            } while ((next = PathsInRange[next]) != null);

            for (var i = 0; i < Path.Count; i++) {
                Path[i].OnPathSet(PlayerHandler.GetCurrentPlayer(), i, SelectedUnit, false, i == Path.Count - 1);
            }

            SelectedUnit.SetPath(Path);
        }
    }
}
