using System;
using System.Collections.Generic;
using Actors.Units;
using Base.Common;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Players {
    [CreateAssetMenu(fileName = "Player/Player", menuName = "PlayerData")]
    public class PlayerData : BaseData {
        [SerializeField]
        private string playerName;

        [SerializeField]
        private Color playerColor;

        [Serializable]
        private class PlayerHqInfo {
            public GameObject prefab;
            public int x;
            public int y;
        }

        [FormerlySerializedAs("baseInfo")]
        [SerializeField]
        private PlayerHqInfo hqInfo;

        [SerializeField]
        private List<UnitData> unitDatas;


        public GridCoords HqCoordinates => new GridCoords(hqInfo.x, hqInfo.y);

        public Color PlayerColor => playerColor;

        public GameObject HqPrefab => hqInfo.prefab;

        public string PlayerName => playerName;

        public List<UnitData> UnitDatas => unitDatas;
    }
}
