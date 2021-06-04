using System;

namespace Handlers.PhaseHandlers {
    public interface IPhaseHandler : IDisposable {
        void StartGame();
    }
}
