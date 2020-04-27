using Base.Interfaces;
using UnityEngine;

namespace UI.HqUis {
    public abstract class HqUiState : IState { }

    public class UnitInfo : HqUiState{
        public int MenuPosition { get; }
        public GameObject UnitPrefab { get; }
        public string UnitName { get; }
        public string UnitCost { get; }

        public UnitInfo(int menuPosition, GameObject unitPrefab, string unitName, string unitCost) {
            UnitPrefab = unitPrefab;
            MenuPosition = menuPosition;
            UnitName = unitName;
            UnitCost = unitCost;
        }
    }
}
