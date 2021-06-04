using System.Collections;
using Base.Singletons;

namespace Handlers {
    public class CoroutineHandler : SingletonMonoBehaviour<CoroutineHandler> {

        public static void DoStartCoroutine(IEnumerator enumerator) {
            Instance.StartCoroutine(enumerator);
        }

        public static void DoStopCoroutine(IEnumerator enumerator) => Instance.StopCoroutine(enumerator);
    }
}
