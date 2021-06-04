using System.Collections.Generic;
using Actors.Units;
using Handlers;
using UnityEngine;

namespace Debuggers {
    public class ExecutionDebugger : MonoBehaviour {


        public List<Unit> tickWorkers = new List<Unit>();
        
        public List<ResourceUnit> resTickWorkers = new List<ResourceUnit>();
        
        public List<Unit> currentTickWorkers = new List<Unit>();
        
        public List<ResourceUnit> currentResTickWorkers = new List<ResourceUnit>();


        private void Update() {
            tickWorkers.Clear();
            resTickWorkers.Clear();
            currentTickWorkers.Clear();
            currentResTickWorkers.Clear();
            foreach (var tickWorker in ExecutionHandler.TickWorkers) {
                if (tickWorker is Unit unit) {
                    tickWorkers.Add(unit);
                } else {
                    resTickWorkers.Add(tickWorker as ResourceUnit);
                }
            }
            
            foreach (var tickWorker in ExecutionHandler.CurrentTickWorkers) {
                if (tickWorker is Unit unit) {
                    currentTickWorkers.Add(unit);
                } else {
                    currentResTickWorkers.Add(tickWorker as ResourceUnit);
                }
            }
        }
    }
}
