using System;
using Base.Interfaces;
using Common;
using Extensions;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridInteractor : IBaseInteractor {

        private WeakReference<GridGenerator> generator;
        
        public void AddExposed<TExposed>(TExposed exposed) {
            generator = new WeakReference<GridGenerator>(exposed as GridGenerator);
        }

        public float? SampleTerrain(Vector2 position) => generator.GetOrNull()?.SampleTerrain(position);
        public float? SampleTerrain(float x, float y) => generator.GetOrNull()?.SampleTerrain(x, y);

        public GridCoords GetMaxCoords() => generator.GetOrNull().GetMaxGridCoords();

        public GameObject GetResourceDecorator() => generator.GetOrNull().GetResourceDecorator();
    }
}
