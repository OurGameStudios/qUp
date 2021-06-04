using System;
using System.Collections.Generic;
using Base.Interfaces;
using Extensions;

namespace Handlers {
    public class DataHandler {
        
        private static DataHandler instance;

        private static DataHandler Instance => instance ??= new DataHandler();

        private readonly Dictionary<Type, IDataProvider> dataProviders = new Dictionary<Type, IDataProvider>();

        public static void ExposeData(IDataProvider data) =>
            Instance.dataProviders.Add(data.GetDataType(), data);

        public static TData ProvideData<TData>() where TData : IData {
            return (TData) Instance.dataProviders.GetOrNull(typeof(TData)).GetData();
        }
    }
}
