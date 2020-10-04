using Actors.Players;
using Actors.Units;
using Base.Interfaces;
using Common;
using UnityEngine;

namespace Managers.PlayerManagers {
    
    public interface IPlayerManagerState : IState { }
    public class PlayerManagerState<TState> : State<TState>, IPlayerManagerState where TState : class, new(){ }

    public class UnitSpawn : PlayerManagerState<UnitSpawn> {
        public Vector3 SpawnPosition { get; private set; }
        public UnitData UnitData { get; private set; }
        public GridCoords Coords { get; private set; }
        public Player Player { get; private set; }
        public static UnitSpawn Where(Vector3 spawnPosition, UnitData unitData, GridCoords coords, Player player) {
            Cache.SpawnPosition = spawnPosition;
            Cache.UnitData = unitData;
            Cache.Coords = coords;
            Cache.Player = player;
            return Cache;
        }
    }
    
    public class ResourceUnitSpawn : PlayerManagerState<ResourceUnitSpawn> {
        public Vector3 SpawnPosition { get; private set; }
        public UnitData UnitData { get; private set; }

        public GridCoords Coords { get; private set; }
        
        public Player Player { get; private set; }
        
        public static ResourceUnitSpawn Where(Vector3 spawnPosition, UnitData unitData, GridCoords coords,
                                              Player player) {
            Cache.SpawnPosition = spawnPosition;
            Cache.UnitData = unitData;
            Cache.Coords = coords;
            Cache.Player = player;
            return Cache;
        }
    }
}
