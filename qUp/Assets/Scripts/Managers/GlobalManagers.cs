using System;
using System.Collections.Generic;
using System.Linq;
using Base.Interfaces;

namespace Managers {
    public class GlobalManager {
        private static GlobalManager globalManager;
        private static GlobalManager Instance => globalManager ?? (globalManager = new GlobalManager());
        
        private readonly Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();
        
        public static void ExposeManager(IManager manager) => Instance.managers.Add(manager.GetType(), manager);

        public static TManager ProvideManager<TManager>() where TManager : IManager {
            return (TManager) Instance.managers.Values.FirstOrDefault(it => it is TManager);
        }
    }
}
