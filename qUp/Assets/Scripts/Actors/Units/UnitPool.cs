using System.Collections.Generic;
using System.Linq;
using Base.Singletons;
using Handlers;
using UnityEngine;

namespace Actors.Units {
    public class UnitPool : SingletonClass<UnitPool> {
        
        private UnitData resourceUnitData;
        
        private readonly Dictionary<UnitData, List<GameObject>> ghostsPool = new Dictionary<UnitData, List<GameObject>>();
        private readonly Dictionary<UnitData, List<GameObject>> usedGhostsPool = new Dictionary<UnitData, List<GameObject>>();

        private readonly Dictionary<UnitData, List<IUnit>> pool = new Dictionary<UnitData, List<IUnit>>();
        private readonly Dictionary<UnitData, List<IUnit>> usedPool = new Dictionary<UnitData, List<IUnit>>();

        private readonly List<ResourceUnit> resourceUnitPool = new List<ResourceUnit>();

        public UnitPool() {
            resourceUnitData = DataHandler.ProvideData<UnitData>();
        }

        public static void RegisterUnitData(UnitData unitData) {
            // There is no need to check multiple pools since we add the key to all pools
            if (!Instance.ghostsPool.ContainsKey(unitData)) {
                Instance.ghostsPool.Add(unitData, new List<GameObject>());
                Instance.usedGhostsPool.Add(unitData, new List<GameObject>());
                Instance.pool.Add(unitData, new List<IUnit>());
                Instance.usedPool.Add(unitData, new List<IUnit>());
            }
        }

        public static GameObject TakeGhost(UnitData unitData) {
            var ghosts = Instance.ghostsPool[unitData];
            var ghost = ghosts.FirstOrDefault();
            if (ghost == default) {
                ghost = Object.Instantiate(unitData.ghostPrefab);
            } else {
                ghosts.Remove(ghost);
            }
            Instance.usedGhostsPool[unitData].Add(ghost);
            ghost.SetActive(true);
            return ghost;
        }

        /// <summary>
        /// Deactivates ghost gameObject and returns it to unitDatas pool.
        /// </summary>
        /// <param name="unitData"></param>
        /// <param name="ghost"></param>
        public static void ReturnGhost(UnitData unitData, GameObject ghost) {
            ghost.SetActive(false);
            Instance.usedGhostsPool[unitData].Remove(ghost);
            Instance.ghostsPool[unitData].Add(ghost);
        }

        /// <summary>
        /// Takes gameObject of unitData from the pool instantiating it if necessary.
        /// </summary>
        /// <param name="unitData"></param>
        /// <returns></returns>
        public static IUnit TakeUnit(UnitData unitData) {
            var units = Instance.pool[unitData];
            var unit = units.FirstOrDefault();
            if (unit == default) {
                unit = Object.Instantiate(unitData.prefab).GetComponent<Unit>();
            } else {
                units.Remove(unit);
            }
            Instance.usedPool[unitData].Add(unit);
            unit.SetActive(true);
            return unit;
        }

        public static void ReturnUnit(IUnit unit) {
            Instance.usedPool[unit.Data].Remove(unit);
            Instance.pool[unit.Data].Add(unit);
            unit.SetActive(false);
        }

        public static IUnit TakeResourceUnit() {
            var units = Instance.resourceUnitPool;
            var unit = units.FirstOrDefault();
            if (unit == default) {
                unit = Object.Instantiate(Instance.resourceUnitData.prefab).GetComponent<ResourceUnit>();
            } else {
                units.Remove(unit);
            }
            unit.SetActive(true);
            return unit;
        }

        public static void ReturnResourceUnit(ResourceUnit unit) {
            Instance.resourceUnitPool.Add(unit);
            unit.SetActive(false);
        }
    }
}
