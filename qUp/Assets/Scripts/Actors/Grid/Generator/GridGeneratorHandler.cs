using System.Collections.Generic;
using System.Linq;
using Actors.Hqs;
using Actors.Players;
using Actors.Tiles;
using Base.Managers;
using Base.Singletons;
using Common;
using Extensions;
using Handlers;
using Handlers.PlayerHandlers;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridGeneratorHandler : SingletonDataClass<GridGeneratorHandler, GridGeneratorData> {

        private const string GRID_GO_NAME = "Grid";

        private readonly List<GridCoords> preInstantiatedFields = new List<GridCoords>();

        private GameObject fieldsParent;

        public GridGeneratorHandler() {
            Data.SymmetryFunction.SupplyGeneratorFunction(Data.GeneratorFunction);
            preInstantiatedFields.AddRange(Data.predeterminedFieldDatas.Select(it => it.coords));
        }

        public static float SampleTerrain(Vector2 position) => Instance.Data.TerrainGeneratorFunction.SampleTerrain(position);

        public static float SampleTerrain(float x, float y) => Instance.Data.TerrainGeneratorFunction.SampleTerrain(x, y);

        public GridCoords GetMaxGridCoords() => (Data.MapWidth, Data.MapHeight);

        public static GameObject GetResourceDecorator() =>
            Instance.Data.ResourceDecorators[Random.Range(0, Instance.Data.ResourceDecorators.Count - 1)];

        private Dictionary<GridCoords, IPlayer> hqs = new Dictionary<GridCoords, IPlayer>(2);

        public static void CreateGrid() => Instance.GenerateGrid();

        private void GenerateGrid() {
            fieldsParent = new GameObject(GRID_GO_NAME);
            GeneratePlayerHqs();
            // TODO GridWorld size set for the camera
            foreach (var field in Data.predeterminedFieldDatas.Where(field => !field.prefabs.IsEmpty())) {
                GenerateField(field.prefabs.GetRandom(),
                    new Vector3(field.coords.x * Data.XOffset, 0, field.coords.y * Data.YOffset),
                    field.coords);
            }

            for (var i = 0; i <= Data.MapWidth; i++) {
                for (var j = i % 2; j <= Data.MapHeight; j += 2) {
                    if (preInstantiatedFields.Contains((i, j))) continue;
                    var prefab =
                        Data.SymmetryFunction.ProvideTile((i, j), (Data.MapWidth, Data.MapHeight));
                    GenerateField(prefab, new Vector3(i * Data.XOffset, 0, j * Data.YOffset), (i, j));
                }
            }
        }

        private void GeneratePlayerHqs() {
            foreach (var basePlayerPair in PlayerHandler.GetPlayerHqs()) {
                var hqCoords = GetPlayersRealHqCoords(basePlayerPair.coords);
                var x = hqCoords.x;
                var y = hqCoords.y;
                preInstantiatedFields.Add((x, y));
                var offset = new Vector3(x * Data.XOffset, 0, y * Data.YOffset);
                hqs.Add(basePlayerPair.coords, basePlayerPair.owner);
                GenerateBase(offset, (x, y), basePlayerPair.owner);
            }
        }

        /// <summary>
        /// Generates a tile. If tile is a neighbour of an hq (found in hqs list) then it sets it as a spawn tile
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="offset"></param>
        /// <param name="mapCoordinates"></param>
        private void GenerateField(GameObject prefab, Vector3 offset, GridCoords mapCoordinates) {
            var tile = Tile.Instantiate(prefab,
                fieldsParent.transform,
                offset,
                mapCoordinates,
                Data.TerrainGeneratorFunction.SampleTerrain);
            foreach (var coordsPlayerPair in hqs) {
                coordsPlayerPair.Value.SetRealHqCoords(GetPlayersRealHqCoords(coordsPlayerPair.Key));
            }

            var spawnTilePlayer = hqs.FirstOrDefault(it => GetPlayersRealHqCoords(it.Key).IsNeighbourOf(mapCoordinates))
                                     .Value;
            if (spawnTilePlayer != default) {
                tile.SetAsSpawnTile(spawnTilePlayer);
            }

            GridHandler.AddTile(tile);
        }

        private void GenerateBase(Vector3 offset, GridCoords mapCoordinates, IPlayer owner) {
            var hq = Hq.Instantiate(fieldsParent.transform,
                offset,
                mapCoordinates,
                Data.TerrainGeneratorFunction.SampleTerrain,
                owner);
            GridHandler.AddHq(hq);
        }

        /// <summary>
        /// This is for getting max coords if player hq coords are out of range
        /// </summary>
        /// <param name="hqCoords">Player hq coords</param>
        /// <returns>Player hq coords or max coords</returns>
        private GridCoords GetPlayersRealHqCoords(GridCoords hqCoords) {
            var x = hqCoords.x;
            var y = hqCoords.y;
            if (x > Data.MapWidth - 1) x = Data.MapWidth;
            if (y > Data.MapHeight - 1) y = Data.MapHeight;
            return (x, y);
        }
    }
}
