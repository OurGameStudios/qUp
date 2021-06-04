using Base.Common;
using Base.Interfaces;
using Handlers;

namespace Base.Managers {
    public abstract class BaseClassManager<TData> : IManager where TData : BaseData {
        private TData data;

        protected TData Data => data;

        private void InitData() {
            if (typeof(TData) != typeof(NoData)) {
                data = DataHandler.ProvideData<TData>();
            }
        }

        protected BaseClassManager() {
            InitData();
        }
    }
}
