using Base.Common;
using UnityEngine;

namespace Base.MonoBehaviours {
    public class BaseMonoBehaviour<TData> : BaseListenerMonoBehaviour where TData : BaseData {

        [SerializeField]
        protected TData data;
    }
}
