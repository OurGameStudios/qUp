using System.Collections.Generic;
using System.Linq;
using Actors.Units;
using Common;
using Extensions;
using Managers.GridManagers.GridInfos;

namespace Managers.GridManagers {
    public class GridUnitHandler {
        
        private readonly Dictionary<Unit, List<TileTickInfo>> unitPath = new Dictionary<Unit, List<TileTickInfo>>();
        
        private Dictionary<TileTickInfo, TileTickInfo> pathsInRange;

        private Pathfinder pathfinder;

        private bool isUnitSelected;
        private bool isPathAltered;
        
        //TODO selectedGroup which holds all units on selected tileTick
        private readonly List<Unit> selectedUnits = new List<Unit>(3);
        private int currentTick = 0;
        private List<TileTickInfo> currentSelectedPath = new List<TileTickInfo>(5);

        public void OnGroupSelected(Unit unit, int tick) {
            //TODO clear previous focus
            selectedUnits.Repopulate(unitPath[unit][tick].units);
            
        }

        
        public void SetRange() {
            // pathfinder.FindRange(selectedUnits.Count, selectedUnits.Min(x => x.data.tickPoints), grid, ref pathsInRange);
        }

        public void SavePath() {
            if (!isPathAltered) return;
            
        }

        public void ClearFocus() {
            isUnitSelected = false;
            
        }
    }
}
