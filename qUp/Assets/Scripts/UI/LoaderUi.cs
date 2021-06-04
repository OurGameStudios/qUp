using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LoaderUi : MonoBehaviour {

        [SerializeField]
        private Image fillArea;
        
        private bool isLoading;

        private AsyncOperation loading;

        public void StartLoading(AsyncOperation loading) {
            gameObject.SetActive(true);
            isLoading = true;
            this.loading = loading;
        }
        
        private void Update() {
            if (isLoading) {
                fillArea.fillAmount = loading.progress;
            }
        }
    }
}
