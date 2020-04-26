using System;
using System.Collections.Generic;
using Actors.Grid.SymmetryFunctions.Base;
using Common;
using UnityEngine;

namespace Actors.Grid.SymmetryFunctions {
    [CreateAssetMenu(fileName = "CenterMirrorFunction", menuName = "Grid/Symmetry Functions/Center Mirror")]
    public class CenterMirrorFunction : SymmetryFunction {

        [NonSerialized]
        private Dictionary<GridCoords, GameObject> generatedTiles = new Dictionary<GridCoords, GameObject>();
        
        public override GameObject ProvideTile(GridCoords coords, GridCoords maxCoords) {
            if (generatedTiles.ContainsKey(GridCoords.MirrorFromGrid(coords, maxCoords))) {
                return generatedTiles[GridCoords.MirrorFromGrid(coords, maxCoords)];
            }
            
            var prefab = GeneratorFunction.GeneratePrefab(coords,  maxCoords);
            generatedTiles.Add(coords, prefab);
            return prefab;
        }
    }
}
