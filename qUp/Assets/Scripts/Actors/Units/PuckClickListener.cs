using UnityEngine;
using UnityEngine.EventSystems;

namespace Actors.Units {
    public class PuckClickListener : MonoBehaviour, IPointerClickHandler {

        [SerializeField]
        private Unit unit;

        public void OnPointerClick(PointerEventData eventData) {
            unit.OnPointerClick(eventData);
        }
    }
}
