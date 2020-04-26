using System.Collections.Generic;
using System.Linq;
using Base.Interfaces;

namespace Managers {
    public class GlobalManager {
        private static GlobalManager globalManager;
        private static GlobalManager Instance => globalManager ?? (globalManager = new GlobalManager());

        private readonly List<IManager> managers = new List<IManager>();
        
        public static void AddManager(IManager manager) => Instance.managers.Add(manager);

        public static TManager GetManager<TManager>() where TManager : IManager {
            return (TManager) Instance.managers.FirstOrDefault(it => it is TManager);
        }
    }
}
