using System.Collections.Generic;
using System.Linq;
using Base.Singletons;
using Common;
using Extensions;
using Handlers.PhaseHandlers;

namespace Handlers {
    public class ExecutionHandler : SingletonClass<ExecutionHandler> {
        private readonly EventAction<int> tickDispatch = new EventAction<int>();
        public static EventAction<int> TickDispatch => Instance.tickDispatch;

        private readonly HashSet<ITickWorker> tickWorkers = new HashSet<ITickWorker>();
        public static HashSet<ITickWorker> TickWorkers => Instance.tickWorkers;

        private readonly HashSet<ITickWorker> currentTickWorkers = new HashSet<ITickWorker>();
        public static HashSet<ITickWorker> CurrentTickWorkers => Instance.currentTickWorkers;

        private int currentTick = 1;
        private static int CurrentTick => Instance.currentTick;

        private bool hasDequeueContinuedPhase;

        public static void ListenToPhaseManager() {
            Instance.AddToDispose(PhaseHandler.ExecutionPhase.Subscribe(OnExecutionPhase));
        }

        private static void OnExecutionPhase() {
            // TODO should show some UI stating that execution is starting or that there is no execution at all.
            Instance.hasDequeueContinuedPhase = false;
            if (TickWorkers.IsEmpty()) {
                PhaseHandler.ContinuePhase();
            } else {
                TickDispatch.Invoke(CurrentTick);
            }
        }

        /// <summary>
        /// Adds worker to TickWorkers. This tells execution handler that it has to wait for worker before moving to
        /// next tick.
        /// </summary>
        /// <param name="tickWorker"></param>
        public static void TickWorkerQueued(ITickWorker tickWorker) => TickWorkers.Add(tickWorker);

        /// <summary>
        /// Removes worker from TickWorkers. If worker has no work it shouldn't be among TickWorkers.
        /// </summary>
        /// <param name="tickWorker"></param>
        public static void TickWorkerDequeued(ITickWorker tickWorker) {
            TickWorkers.Remove(tickWorker);
            if (CurrentTickWorkers.Contains(tickWorker)) {
                CurrentTickWorkers.Remove(tickWorker);
            }
            if (TickWorkers.IsEmpty() && !Instance.hasDequeueContinuedPhase) {
                CurrentTickWorkers.Clear();
                Instance.currentTick = 1;
                PhaseHandler.ContinuePhase();
                Instance.hasDequeueContinuedPhase = true;
            }
        }

        /// <summary>
        /// Notifies that worker has started his work.
        /// </summary>
        /// <param name="tickWorker"></param>
        public static void TickWorkerStarted(ITickWorker tickWorker) {
            CurrentTickWorkers.Add(tickWorker);
        }

        /// <summary>
        /// Sets tickWorker for this tick as completed. If tickWorker has no more work it is removed from TickWorkers and
        /// wont be awaited next ticks. If no worker needs to be awaited it signals PhaseManager that execution phase is ended.
        /// </summary>
        /// <param name="tickWorker"></param>
        public static void TickWorkerFinished(ITickWorker tickWorker) {
            if (!CurrentTickWorkers.Contains(tickWorker) && !TickWorkers.Contains(tickWorker)) {
                return;
            }

            if (CurrentTickWorkers.Contains(tickWorker)) {
                CurrentTickWorkers.Remove(tickWorker);
            }

            if (!tickWorker.HasMoreWork() && TickWorkers.Contains(tickWorker)) {
                TickWorkerDequeued(tickWorker);
            }

            ValidateTickWorkers();
            if (Instance.hasDequeueContinuedPhase) {
                return;
            }

            if (TickWorkers.IsEmpty()) {
                Instance.currentTick = 1;
                PhaseHandler.ContinuePhase();
            } else if (CurrentTickWorkers.IsEmpty()) {
                Instance.currentTick++;
                if (Instance.currentTick <= Configuration.GetMaxTick()) {
                    TickDispatch?.Invoke(CurrentTick);
                } else {
                    PhaseHandler.ContinuePhase();
                }
            }
        }

        private static void ValidateTickWorkers() {
            var invalidTicKWorkers = Instance.tickWorkers.Where(worker => !worker.HasMoreWork()).ToList();
            foreach (var tickWorker in invalidTicKWorkers) {
                TickWorkers.Remove(tickWorker);
                if (CurrentTickWorkers.Contains(tickWorker)) {
                    CurrentTickWorkers.Remove(tickWorker);
                }
            }
        }
    }
}
