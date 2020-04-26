using System.Collections.Generic;
using Actors.Players;
using Base.Common;
using UnityEngine;

namespace Managers.PlayerManagers {
    [CreateAssetMenu(fileName = "PlayerManagerData", menuName = "PlayerManagerData")]
    public class PlayerManagerData : BaseData {
        [SerializeField]
        private List<PlayerData> playerDatas;

        public List<PlayerData> PlayerDatas => playerDatas;
    }
}
