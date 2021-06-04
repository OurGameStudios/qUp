using Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Common.Constants;

namespace UI {
    public class MainMenu : MonoBehaviour {
        
        private const string MUSIC_VOLUME = "MusicVolume";

        [SerializeField]
        private Button startGameButton;

        [SerializeField]
        private TextMeshProUGUI startGameText;

        [SerializeField]
        private Button quitGameButton;

        [SerializeField]
        private Slider musicVolumeSlider;

        [SerializeField]
        private AudioMixer volumeMixer;

        [SerializeField]
        private LoaderUi loaderUi;

        private bool isPauseMenu;

        private void Awake() {
            isPauseMenu = SceneManager.sceneCount > 1;
            startGameButton.onClick.AddListener(StartGame);
            quitGameButton.onClick.AddListener(QuitGame);
            volumeMixer.GetFloat(MUSIC_VOLUME, out var musicVolume);
            musicVolumeSlider.value = musicVolume;
            musicVolumeSlider.onValueChanged.AddListener(volume => volumeMixer.SetFloat(MUSIC_VOLUME, volume));
            if (isPauseMenu) {
                CameraHandler.EnableCamera(false);
                startGameText.text = Localization.CONTINUE_GAME;
            }
        }

        private void StartGame() {
            if (isPauseMenu) {
                InputHandler.EnableControls();
                CameraHandler.EnableCamera(true);
                Time.timeScale = 1;
                SceneManager.UnloadSceneAsync(MAIN_MENU_INDEX);
            } else {
                loaderUi.StartLoading(SceneManager.LoadSceneAsync(MATCH_SCENE_INDEX));
                gameObject.SetActive(false);
            }
        }

        private void QuitGame() {
            Application.Quit();
        }
    }
}
