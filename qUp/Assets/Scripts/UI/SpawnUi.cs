using Actors.Players;
using Actors.Units;
using Base.Singletons;
using Common;
using Handlers.PhaseHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class SpawnUi : SingletonMonoBehaviour<SpawnUi> {
        
        [SerializeField]
        private Transform contentView;

        [SerializeField]
        private GameObject spawnUnitButtonPrefab;

        private GameObjectPool spawnUnitButtonPool;

        private readonly EventAction<UnitData> spawnUnitSelected = new EventAction<UnitData>();

        public static EventAction<UnitData> SpawnUnitSelected => Instance.spawnUnitSelected;

        private void Awake() {
            spawnUnitButtonPool = new GameObjectPool(spawnUnitButtonPrefab, transform);
        }

        public static void ListenToPhaseManager() {
            Instance.AddToDispose(PhaseHandler.PlanningPhase.Subscribe(ShowPlayerSpawnUnits));
            Instance.AddToDispose(PhaseHandler.PlayerChange.Subscribe(HidePlayerSpawnUnits));
            Instance.AddToDispose(PhaseHandler.ExecutionPhase.Subscribe(HidePlayerSpawnUnits));
        }

        /// <summary>
        /// Show units in Spawn Ui
        /// </summary>
        private static void ShowPlayerSpawnUnits(IPlayer player) {
            Instance.gameObject.SetActive(true);
            var units = player.GetSpawnableUnits();
            var buttonList = Instance.spawnUnitButtonPool.RetakeGameObjects(units.Count);
            for (var i = 0; i < units.Count; i++) {
                var button = buttonList[i];
                InitButtonData(buttonList[i], units[i]);
                button.transform.SetParent(Instance.contentView);
            }
        }

        private static void HidePlayerSpawnUnits(IPlayer _) => HidePlayerSpawnUnits();

        public static void HidePlayerSpawnUnits() {
            Instance.gameObject.SetActive(false);
        }

        /// <summary>
        /// Set unit data to button
        /// </summary>
        /// <param name="button">Button to set</param>
        /// <param name="unit">Unit data to take</param>
        private static void InitButtonData(GameObject button, UnitData unit) {
            button.GetComponentInChildren<TextMeshProUGUI>().text = unit.unitName;
            var buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() => SpawnUnitSelected?.Invoke(unit));
        }
    }
}
