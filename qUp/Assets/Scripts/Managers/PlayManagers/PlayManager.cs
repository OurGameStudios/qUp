using System;
using Base.Interfaces;
using Managers.ApiManagers;
using Managers.GridManagers;
using UnityEngine;

namespace Managers.PlayManagers {
    public class PlayManager : MonoBehaviour, IManager {
        private const int PHASE_COUNT = 3;
        private enum Phase {
            PlanningPhase = 0,
            ExecutionPhase = 1,
            PreppingPhase = 2
        }
        
        private readonly Lazy<GridManager> gridManagerLazy =
            new Lazy<GridManager>(ApiManager.ProvideManager<GridManager>);
        
        private GridManager GridManager => gridManagerLazy.Value;

        //TODO this should be changed to prepping phase if starting units are to be implemented
        private Phase phase = Phase.PlanningPhase;

        private int currentMaxTickCount;

        private void Awake() {
            ApiManager.ExposeManager(this);
        }

        public void NextPhase() {
            if ((int)phase != PHASE_COUNT - 1) {
                phase += 1;
            } else {
                phase = 0;
            }

            if (phase == Phase.PlanningPhase) {
                //TODO start planning phase
            } else if (phase == Phase.ExecutionPhase) {
                StartExecutionPhase();
            } else if (phase == Phase.PreppingPhase) {
                //TODO start prepping phase
                NextPhase(); //replace when implementing prepping phase
            }
        }

        private void StartExecutionPhase() {
            //TODO Change controls for input manager
            GridManager.StartExecution();
        }
    }
}
