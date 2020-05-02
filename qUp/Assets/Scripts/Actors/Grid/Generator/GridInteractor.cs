using System;
using Base.Interfaces;
using Extensions;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridInteractor : IBaseInteractor {

        private WeakReference<GridGenerator> generator;
        
        public void AddExposed<TExposed>(TExposed exposed) {
            generator = new WeakReference<GridGenerator>(exposed as GridGenerator);
        }

        public float? SampleTerrain(Vector2 position) => generator.GetOrNull()?.SampleTerrain(position);
    }
}
