using System;
using Base;
using Base.Common;
using Common;
using UnityEngine;

namespace Actors.Players {
    [CreateAssetMenu(fileName = "Player/Player", menuName = "PlayerData")]
    public class PlayerData : BaseData {
        [SerializeField]
        private string playerName;
        
        [SerializeField]
        private Color playerColor;

        [Serializable]
        private class PlayerBaseInfo {
            public GameObject prefab;
            public int x;
            public int y;
        }

        [SerializeField]
        private PlayerBaseInfo baseInfo;

        public GridCoords BaseCoordinates => new GridCoords(baseInfo.x, baseInfo.y);

        public Color PlayerColor => playerColor;

        public GameObject BasePrefab => baseInfo.prefab;

        public string PlayerName => playerName;
    }
}
