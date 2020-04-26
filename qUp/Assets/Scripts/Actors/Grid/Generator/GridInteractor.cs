using System;
using Base;
using Base.Interfaces;

namespace Actors.Grid.Generator {
    public class GridInteractor : IBaseInteractor {

        private WeakReference<GridGenerator> generator;
        
        public void AddExposed<TExposed>(TExposed exposed) {
            generator = new WeakReference<GridGenerator>(exposed as GridGenerator);
        }
    }
}
