using Base.MonoBehaviours;

namespace Base.Singletons {
    public abstract class SingletonMonoBehaviour<T> : BaseListenerMonoBehaviour where T : SingletonMonoBehaviour<T> {
        
        private static SingletonMonoBehaviour<T> instance;
        public static T Instance => (T) instance;

        public SingletonMonoBehaviour() {
            instance = this;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            instance = null;
        }
    }
}
