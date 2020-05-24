using System.Collections.Generic;
using Actors.Grid.SymmetryFunctions.Base;
using Actors.Players;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridGenerator : BaseController<IGridGeneratorState> {
        protected override bool Expose => true;

        private GridGeneratorData data;
        private SymmetryFunction symmetryFunction;
        private readonly PlayerInteractor playerInteractor = ApiManager.ProvideInteractor<PlayerInteractor>();

        private readonly List<GridCoords> preInstantiatedFields = new List<GridCoords>();

        public void Init(GridGeneratorData inData) {
            data = inData;
            symmetryFunction = inData.SymmetryFunction;
            symmetryFunction.SupplyGeneratorFunction(inData.GeneratorFunction);
        }

        public float SampleTerrain(Vector2 position) => data.TerrainGeneratorFunction.SampleTerrain(position);
        public float SampleTerrain(float x, float y) => data.TerrainGeneratorFunction.SampleTerrain(x, y);

        public void GenerateGrid() {
            CreatePlayerHqs();
            SetState(GridWorldSize.With(data.XOffset,
                data.YOffset,
                data.MapWidth * data.XOffset,
                data.MapHeight * data.YOffset));
            for (var i = 0; i <= data.MapWidth; i++) {
                for (var j = i % 2; j <= data.MapHeight; j += 2) {
                    if (preInstantiatedFields.Contains((i, j))) continue;
                    var prefab = symmetryFunction.ProvideTile(new GridCoords(i, j),
                        new GridCoords(data.MapWidth, data.MapHeight));
                    SetState(FieldGenerated.With(prefab, new Vector3(i * data.XOffset, 0, j * data.YOffset), (i, j)));
                }
            }
        }

        private void CreatePlayerHqs() {
            playerInteractor
                .GetPlayerHqs()
                .ForEach(baseDetails => {
                    var x = baseDetails.coords.x;
                    var y = baseDetails.coords.y;
                    if (x > data.MapWidth - 1) x = data.MapWidth;
                    if (y > data.MapHeight - 1) y = data.MapHeight;
                    preInstantiatedFields.Add((x, y));
                    var offset = new Vector3(x * data.XOffset, 0, y * data.YOffset);
                    SetState(BaseGenerated.With(offset, (x, y), baseDetails.owner));
                });
        }
    }
}
