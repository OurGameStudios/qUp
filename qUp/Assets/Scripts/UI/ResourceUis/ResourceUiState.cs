using System;
using Base.Interfaces;

namespace UI.ResourceUis {

    public interface IResourceUiState : IState { }
    
    public abstract class ResourceUiState<TState> : State<TState>, IResourceUiState where TState : class, new() { }

    public class UpkeepChanged : ResourceUiState<UpkeepChanged> {
        public String Upkeep { get; private set; }
        public String Total { get; private set; }

        public static UpkeepChanged Where(String upkeep, String total) {
            Cache.Upkeep = upkeep;
            Cache.Total = total;
            return Cache;
        }
    }
    
    public class IncomeChanged : ResourceUiState<IncomeChanged> {
        public String Income { get; private set; }
        public String Total { get; private set; }

        public static IncomeChanged Where(String income, String total) {
            Cache.Income = income;
            Cache.Total = total;
            return Cache;
        }
    }

    public class ResourceUnitsChanged : ResourceUiState<ResourceUnitsChanged> {
        public String ResourceUnitCount { get; private set; }

        public static ResourceUnitsChanged Where(String resourceUnitCount) {
            Cache.ResourceUnitCount = resourceUnitCount;
            return Cache;
        }
    }
}
