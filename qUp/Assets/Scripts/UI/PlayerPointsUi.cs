using Actors.Players;
using Base.Singletons;
using Extensions;
using Handlers;
using Handlers.PlayerHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class PlayerPointsUi : SingletonMonoBehaviour<PlayerPointsUi> {

        [Header("Player 1")]
        [SerializeField]
        private TextMeshProUGUI pointsTextP1;

        [SerializeField]
        private Image fillAreaP1;
        
        [SerializeField]
        private Image fillP1;
        
        [SerializeField]
        private TextMeshProUGUI fillPointsTextP1;
        
        [Header("Player 2")]
        [SerializeField]
        private TextMeshProUGUI pointsTextP2;

        [SerializeField]
        private Image fillAreaP2;
        
        [SerializeField]
        private Image fillP2;
        
        [SerializeField]
        private TextMeshProUGUI fillPointsTextP2;

        private (int points, int maxPoints) Player1Points {
            set {
                fillAreaP1.fillAmount = (float) value.points /value.maxPoints;
                pointsTextP1.text = Localization.POINTS_UI_TEXT.Format(value.points, value.maxPoints);
                fillPointsTextP1.text = Localization.POINTS_UI_TEXT.Format(value.points, value.maxPoints);
            }
        }
        
        private (int points, int maxPoints) Player2Points {
            set {
                fillAreaP2.fillAmount = (float) value.points / value.maxPoints;
                pointsTextP2.text = Localization.POINTS_UI_TEXT.Format(value.points, value.maxPoints);
                fillPointsTextP2.text = Localization.POINTS_UI_TEXT.Format(value.points, value.maxPoints);
            }
        }
        
        public static void SetupUi() {
            var playerColors = PlayerHandler.GetOrderedPlayerColors();
            Instance.fillP1.color = playerColors[0];
            Instance.fillP2.color = playerColors[1];
            Instance.Player1Points = (0, Configuration.GetMaxPoints());
            Instance.Player2Points = (0, Configuration.GetMaxPoints());
        }

        public static void UpdatePlayerPoints(IPlayer player, int points) {
            if (PlayerHandler.GetPlayerIndex(player) == 0) {
                Instance.Player1Points = (points, Configuration.GetMaxPoints());
            } else {
                Instance.Player2Points = (points, Configuration.GetMaxPoints());
            }
        }
    }
}
