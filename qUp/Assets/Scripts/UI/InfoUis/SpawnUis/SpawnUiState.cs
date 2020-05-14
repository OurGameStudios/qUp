using Base.Interfaces;
using UnityEngine;

namespace UI.InfoUis.SpawnUis {
    
    public interface ISpawnUiState : IState { }
    public abstract class SpawnUiState<TState> : State<TState>, ISpawnUiState where TState : class, new() { }

    public class SetUI : SpawnUiState<SetUI> {
        public Sprite UnitSprite { get; private set; }
        public string UnitName { get; private set; }
        public string UnitCost { get; private set; }
        public string UnitHp { get; private set; }
        public string UnitAtt { get; private set; }
        public string UnitTp { get; private set; }

        public static SetUI Where(Sprite sprite, string unitName, string unitCost, string unitHp, string unitAtt, string unitTp) {
            Cache.UnitSprite = sprite;
            Cache.UnitName = unitName;
            Cache.UnitCost = unitCost;
            Cache.UnitHp = unitHp;
            Cache.UnitAtt = unitAtt;
            Cache.UnitTp = unitTp;
            return Cache;
        }
    }

    public class ShowUI : SpawnUiState<ShowUI> {
        public static ShowUI Where() => Cache;
    }

    public class HideUI : SpawnUiState<HideUI> {
        public static HideUI Where() => Cache;
    }
}
