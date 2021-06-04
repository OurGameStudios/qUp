using Actors.Players;
using Base.Singletons;
using Common;
using UnityEngine;

namespace UI {
    public class WorldUi : SingletonMonoBehaviour<WorldUi> {

        [SerializeField]
        private GameObject worldMessageUiPrefab;

        private MonoBehaviourPool<WorldMessageUi> messagePool;

        private static int DefaultMessageAlpha => 200;
        private static Color DefaultMessageColor => new Color(255, 255, 255, DefaultMessageAlpha);


        private static Color DefaultTextColor => Color.white;

        private void Awake() {
            messagePool = new MonoBehaviourPool<WorldMessageUi>(worldMessageUiPrefab, transform);
        }

        public static void ShowMessage(string message, Vector3 position,
                                       WorldMessageUi.WorldMessageDuration duration =
                                           WorldMessageUi.WorldMessageDuration.Short) {
            
            Instance.messagePool.TakeObject()
                    .SetMessage(message, position, DefaultMessageColor, DefaultTextColor, duration);
        }

        public static void ShowMessageForPlayer(string message, Vector3 position, IPlayer player,
                                                WorldMessageUi.WorldMessageDuration duration =
                                                    WorldMessageUi.WorldMessageDuration.Short) {
            Color.RGBToHSV(player?.GetPlayerColor() ?? Color.white, out var h, out var s, out var v);
            var playerColor = Color.HSVToRGB(h, s / 2, v);
            
            Instance.messagePool.TakeObject()
                    .SetMessage(message, position, playerColor, player != null ? DefaultTextColor : Color.black, duration);
        }

        public static void CloseMessage(WorldMessageUi message) => Instance.messagePool.ReturnObject(message);
        
        public static void ClearAllMessages() {
            Instance.messagePool.ReturnAll();
        }
    }
}
