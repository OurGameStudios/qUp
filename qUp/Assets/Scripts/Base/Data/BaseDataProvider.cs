using System;
using Base.Interfaces;
using Handlers;
using UnityEngine;

namespace Base.Data {
    [DefaultExecutionOrder(-1001)]
    public abstract class BaseDataProvider<Data> : MonoBehaviour, IDataProvider where Data : ScriptableObject {
        [SerializeField]
        private Data data;

        public Type GetDataType() => typeof(Data);

        public object GetData() => data;

        private void Awake() {
            DataHandler.ExposeData(this);
        }
    }
}
