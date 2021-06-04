using System.Collections.Generic;
using Actors.Players;
using Base.Common;
using UnityEngine;

namespace Handlers.PlayerHandlers {
    [CreateAssetMenu(fileName = "PlayerManagerData", menuName = "PlayerManagerData")]
    public class PlayerHandlerData : BaseData {
        
        [SerializeField]
        private List<PlayerData> playerDatas;

        public List<PlayerData> PlayerDatas => playerDatas;
    }
}
