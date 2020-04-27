using Base.Interfaces;
using UnityEngine;

namespace UI.InfoUis.SpawnUis {
    public abstract class SpawnUiState : IState { }

    public class SetUI : SpawnUiState{
        public Sprite UnitSprite { get; }
        public string UnitName { get; }
        public string UnitCost { get; }
        public string UnitHp { get; }
        public string UnitAtt { get; }
        public string UnitTp { get; }

        public SetUI(Sprite sprite, string unitName, string unitCost, string unitHp, string unitAtt, string unitTp) {
            UnitSprite = sprite;
            UnitName = unitName;
            UnitCost = unitCost;
            UnitHp = unitHp;
            UnitAtt = unitAtt;
            UnitTp = unitTp;
        }
    }

    public class ShowUI : SpawnUiState { }

    public class HideUI : SpawnUiState { }
}
