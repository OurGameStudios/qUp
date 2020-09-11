using Base.Interfaces;
using UnityEngine;

namespace UI.HqUis {
    
    public interface IHqUiState : IState { }
    public abstract class HqUiState<TState> : State<TState>, IHqUiState where TState : class, new(){ }

    public class UnitInfo : HqUiState<UnitInfo> {
        public int MenuPosition { get; private set; }
        public GameObject UnitPrefab { get; private set; }
        public string UnitCost { get; private set; }
        
        public Sprite UiImage { get; private set; }

        public static UnitInfo Where(int menuPosition, GameObject unitPrefab, string unitCost,
                                     Sprite unitUiImage) {
            Cache.UnitPrefab = unitPrefab;
            Cache.MenuPosition = menuPosition;
            Cache.UnitCost = unitCost;
            Cache.UiImage = unitUiImage;
            return Cache;
        }
    }
}
