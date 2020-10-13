using Actors.Units;
using Base.Interfaces;
using Managers.GridManagers.GridInfos;
using UnityEngine;

namespace Managers.GridManagers {
    
    public interface IGridManagerState : IState { }
    public abstract class GridManagerState<TState> : State<TState>, IGridManagerState where TState : class, new() { }
    
    public class UnitSpawn : GridManagerState<UnitSpawn> {
        public Vector3 SpawnPosition { get; private set; }
        public UnitSpawnInfo UnitSpawnInfo { get; private set; }
        
        public static UnitSpawn Where(Vector3 spawnPosition, UnitSpawnInfo unitSpawnInfo) {
            Cache.SpawnPosition = spawnPosition;
            Cache.UnitSpawnInfo = unitSpawnInfo;
            return Cache;
        }
    }
    
    public class UnitSpawned : GridManagerState<UnitSpawned> {
        public GameObject Ghost { get; private set; }
        
        public static UnitSpawned Where(GameObject ghost) {
            Cache.Ghost = ghost;
            return Cache;
        }
    }
}
