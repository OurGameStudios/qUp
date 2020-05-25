using System;
using Base.Interfaces;
using Managers.ApiManagers;
using Managers.GridManagers;
using Managers.InputManagers;
using Managers.PlayerManagers;
using UnityEngine;

namespace Managers.PlayManagers {
    public class PlayManager : MonoBehaviour, IManager {
        private enum Phase {
            PlanningPhase,
            ExecutionPhase,
            PreppingPhase
        }
        
        private readonly Lazy<GridManager> gridManagerLazy =
            new Lazy<GridManager>(ApiManager.ProvideManager<GridManager>);
        
        private GridManager GridManager => gridManagerLazy.Value;
        
        private readonly Lazy<PlayerManager> playerManagerLazy =
            new Lazy<PlayerManager>(ApiManager.ProvideManager<PlayerManager>);
        
        private PlayerManager PlayerManager => playerManagerLazy.Value;
        
        private readonly Lazy<InputManagerBehaviour> inputManagerLazy =
            new Lazy<InputManagerBehaviour>(ApiManager.ProvideManager<InputManagerBehaviour>);
        
        private InputManagerBehaviour InputManager => inputManagerLazy.Value;
        

        //TODO this should be changed to prepping phase if starting units are to be implemented
        private Phase phase = Phase.PlanningPhase;

        private int currentMaxTickCount;

        private void Awake() {
            ApiManager.ExposeManager(this);
        }

        public void NextPhase() {
            if (phase == Phase.PlanningPhase) {
                //TODO start planning phase
                if (PlayerManager.NextPlayer()) {
                    SwitchPlayer();
                } else {
                    StartExecutionPhase();
                }
            } else if (phase == Phase.ExecutionPhase) {
                StartPlanningPhase();
                //TODO start prepping phase
                // StartPreppingPhase();
            } else if (phase == Phase.PreppingPhase) {
                //TODO start prepping phase
                 //replace when implementing prepping phase
                 StartPlanningPhase();
            }
        }

        private void SwitchPlayer() {
            GridManager.SetupForNextPlayer();
        }

        private void StartExecutionPhase() {
            phase = Phase.ExecutionPhase;
            InputManager.OnExecutionPhase();
            GridManager.StartExecution();
        }

        private void StartPlanningPhase() {
            phase = Phase.PlanningPhase;
            InputManager.OnPlanningPhase();
        }

        private void StartPreppingPhase() {
            phase = Phase.PreppingPhase;
            InputManager.OnPreppingPhase();
        }
    }
}
