using System;
using System.Collections.Generic;
using Actors.Grid.Generator;
using Actors.Units;
using Base.Singletons;
using Common;
using Handlers.PhaseHandlers;
using Handlers.PlayerHandlers;
using UI;
using UnityEngine.SceneManagement;
using static Common.Constants;

namespace Handlers {
    public class Loader : SingletonMonoBehaviour<Loader> {

        private IPlayerHandler playerHandler;
        private IPhaseHandler phaseHandler;

        private readonly List<IDisposable> disposables = new List<IDisposable>();

        private void Awake() {
            // Pool is a singleton but uses SingletonClass so it needs to be initialized
            disposables.Add(new UnitPool());

            InstantiateManagers();
            playerHandler.RegisterAllUnits();
            GridGeneratorHandler.CreateGrid();
            InputHandler.EnableControls();
        }

        private void Start() {
            InitializeUi();
            InitializeHandlers();
            phaseHandler.StartGame();
        }

        private void InstantiateManagers() {
            disposables.Add(new Configuration());
            disposables.Add(playerHandler = new PlayerHandler());
            disposables.Add(new GridHandler());
            disposables.Add(new GridGeneratorHandler());
            disposables.Add(new InteractionHandler());
            disposables.Add(new InputHandler());
            disposables.Add(phaseHandler = new PhaseHandler());
            disposables.Add(new Pathfinder());
            disposables.Add(new ExecutionHandler());
            disposables.Add(new ResourceUnitsHandler());
        }

        private void InitializeUi() {
            SpawnUi.ListenToPhaseManager();
            PlayerChangeUi.ListenToPhaseManager();
            ResourceUi.ListenToPhaseManager();
            PlayerPointsUi.SetupUi();
        }

        private void InitializeHandlers() {
            InteractionHandler.ListenToUi();
            ExecutionHandler.ListenToPhaseManager();
            PlayerHandler.SubscribePlayersToPhaseManager();
            ResourceUnitsHandler.SubscribeToPhaseManager();
        }

        public static void EndGame() {
            foreach (var disposable in Instance.disposables) {
                disposable.Dispose();
            }
            Instance.Dispose();
            SceneManager.LoadScene(MAIN_MENU_INDEX);
        }
    }
}
