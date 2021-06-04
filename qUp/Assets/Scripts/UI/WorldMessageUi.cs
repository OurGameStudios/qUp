using Common;
using Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class WorldMessageUi : MonoBehaviour, IActivable {

        public enum WorldMessageDuration {
            Short = 3, Long = 7, Permanent = -1
        }

        [SerializeField]
        private TextMeshProUGUI text;

        [SerializeField]
        private Image background;

        private Transform cameraTransform;

        private Transform CameraTransform => cameraTransform ??= CameraHandler.MainCamera.transform;

        private float timer = (float) WorldMessageDuration.Short;

        private bool isPermanent;

        public void SetMessage(string message, 
                               Vector3 position,
                               Color backgroundColor,
                               Color textColor,
                               WorldMessageDuration duration) {
            timer = (float) duration;
            isPermanent = duration == WorldMessageDuration.Permanent;
            text.SetText(message);
            text.color = textColor;
            background.color = backgroundColor;
            transform.position = position;
        }

        private void Update() {
            if (timer > 0 && !isPermanent) {
                timer -= Time.deltaTime;
            } else if (!isPermanent) {
                Deactivate();
            }

            transform.rotation = CameraTransform.rotation;
            // TODO implement fade out for world messages
        }

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void Deactivate() => WorldUi.CloseMessage(this);
    }
}
