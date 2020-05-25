using System;
using Base.Interfaces;
using Managers.ApiManagers;
using Managers.GridManagers;
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
            //TODO Change controls for input manager
            phase = Phase.ExecutionPhase;
            GridManager.StartExecution();
        }

        private void StartPlanningPhase() {
            phase = Phase.PlanningPhase;
        }

        private void StartPreppingPhase() {
            phase = Phase.PreppingPhase;
        }
    }
}
