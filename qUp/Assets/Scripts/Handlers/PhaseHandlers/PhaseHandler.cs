using System.Collections;
using Actors.Players;
using Base.Singletons;
using Common;
using Handlers.PlayerHandlers;
using UI;
using UnityEngine;

namespace Handlers.PhaseHandlers {
    public class PhaseHandler : SingletonClass<PhaseHandler>, IPhaseHandler {

        private enum Phase {
            Initial, PlanningPhase, PlayerChange, PreExecutionPhase, ExecutionPhase, PreppingPhase, End
        }

        private readonly EventAction<IPlayer> planningPhase = new EventAction<IPlayer>();
        public static EventAction<IPlayer> PlanningPhase => Instance.planningPhase;
        
        /// <summary>
        /// IPlayer is the player that is next (the player that should confirm the start of the turn)
        /// </summary>
        private readonly EventAction<IPlayer> playerChange = new EventAction<IPlayer>();
        public static EventAction<IPlayer> PlayerChange => Instance.playerChange;
        
        private readonly EventAction preExecutionPhase = new EventAction();
        public static EventAction PreExecutionPhase => Instance.preExecutionPhase;
        
        private readonly EventAction executionPhase = new EventAction();
        public static EventAction ExecutionPhase => Instance.executionPhase;
        
        private readonly EventAction preppingPhase = new EventAction();
        public static EventAction PreppingPhase => Instance.preppingPhase;

        private Phase phase = Phase.Initial;

        private bool shouldDelayNewRound;

        /// <summary>
        /// Phase logic is as follows:
        /// If it is plannig phase
        ///     Check if there are more players to player
        ///         If there are more players to play show Next player prompt and increment the player
        ///         If there are no more players to play start execution phase and increment the player (incrementing
        ///         player while it is the last player resets it to first player)
        /// If it is PlayerChange phase continue to planning phase
        /// If it is Execution phase continue to prepping phase
        /// If it is Prepping phase continue to Planning phase
        /// </summary>
        public static void ContinuePhase() {
            if (Instance.phase == Phase.End) {
                return;
            } 
            if (Instance.phase == Phase.Initial) {
                Instance.ContinueFromInitial();
            } else if (Instance.phase == Phase.PlanningPhase) {
                // Show Next player prompt if its planning phase and there are more players to play
                // Else continue to pre-execution phase
                Instance.ContinueFromPlanningPhase();
            } else if (Instance.phase == Phase.PlayerChange) {
                // Continue to next player
                Instance.ContinueFromPlayerChange();
            } else if (Instance.phase == Phase.PreExecutionPhase) {
                // Continue to execution phase
                Instance.ContinueFromPreExecutionPhase();
            } else if (Instance.phase == Phase.ExecutionPhase) {
                // Continue to prepping phase
                Instance.ContinueFromExecutionPhase();
            } else if (Instance.phase == Phase.PreppingPhase) {
                // Show Next player prompt
                Instance.ContinueFromPreppingPhase();
            }
        }

        public static void EndGame(IPlayer winner) {
            Instance.phase = Phase.End;
            EndGameUi.ShowEndGameUi(winner);
        }

        public void StartGame() {
            StartGameUi.StartListeningForContinue();
            InputHandler.SetCameraControlsEnabled(false);
        }

        private void ContinueFromInitial() {
            Instance.phase = Phase.PlayerChange;
            PlayerChange.Invoke(PlayerHandler.GetCurrentPlayer());
        }

        private void ContinueFromPlayerChange() {
            Instance.phase = Phase.PlanningPhase;
            InputHandler.SetCameraControlsEnabled(true);
            PlanningPhase.Invoke(PlayerHandler.GetCurrentPlayer());
        }

        private void ContinueFromPlanningPhase() {
            if (!PlayerHandler.IsLastPlayer()) {
                Instance.phase = Phase.PlayerChange;
                PlayerHandler.NextPlayer();
                InputHandler.SetCameraControlsEnabled(false);
                PlayerChange?.Invoke(PlayerHandler.GetCurrentPlayer());
            } else {
                Instance.phase = Phase.PreExecutionPhase;
                PreExecutionPhase?.Invoke();
            }
        }

        public static void DelayNewRound() {
            Instance.shouldDelayNewRound = true;
        }

        private void ContinueFromPreExecutionPhase() {
            Instance.phase = Phase.ExecutionPhase;
            ExecutionPhase.Invoke();
        }

        private void ContinueFromExecutionPhase() {
            Instance.phase = Phase.PreppingPhase;
            PreppingPhase.Invoke();
            if (shouldDelayNewRound) {
                shouldDelayNewRound = false;
                CoroutineHandler.DoStartCoroutine(DelayedNewRound());
            } else {
                ContinuePhase();
            }
        }

        private IEnumerator DelayedNewRound() {
            yield return new WaitForSecondsRealtime(3);
            ContinuePhase();
        }

        private void ContinueFromPreppingPhase() {
            Instance.phase = Phase.PlayerChange;
            WorldUi.ClearAllMessages();
            PlayerHandler.NextPlayer();
            PlayerChange?.Invoke(PlayerHandler.GetCurrentPlayer());
        }
    }
}
