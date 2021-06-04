using System;

namespace Handlers.PlayerHandlers {
    public interface IPlayerHandler : IDisposable {
        void RegisterAllUnits();
    }
}
