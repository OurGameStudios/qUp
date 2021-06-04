using System;

namespace Common {
    public class EventAction {
        private event Action Action;

        /// <summary>
        /// Subscribe to event
        /// </summary>
        /// <param name="onAction"></param>
        /// <returns>Delegate for unsubscribing</returns>
        public Action Subscribe(Action onAction) {
            Action += onAction;
            return () => Unsubscribe(onAction);
        }

        public void Unsubscribe(Action onAction) {
            Action -= onAction;
        }

        public void Invoke() => Action?.Invoke();
    }

    public class EventAction<T> {
        private event Action<T> Action;

        /// <summary>
        /// Subscribe to event
        /// </summary>
        /// <param name="onAction"></param>
        /// <returns>Delegate for unsubscribing</returns>
        public Action Subscribe(Action<T> onAction) {
            Action += onAction;
            return () =>  Unsubscribe(onAction);
        }

        public void Unsubscribe(Action<T> onAction) {
            Action -= onAction;
        }

        public void Invoke(T param) => Action?.Invoke(param);
    }
}
