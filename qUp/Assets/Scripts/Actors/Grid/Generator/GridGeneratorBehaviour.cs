using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Hqs;
using Actors.Players;
using Actors.Tiles;
using Base.MonoBehaviours;
using Common;
using Managers.ApiManagers;
using Managers.CameraManagers;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridGeneratorBehaviour : BaseMonoBehaviour<GridGenerator, IGridGeneratorState> {

        public GridGeneratorData data;

        protected override void OnStateHandler(IGridGeneratorState inState) {
            if (inState is FieldGenerated fieldGeneratedState) {
                GenerateField(fieldGeneratedState.Prefab,
                    fieldGeneratedState.Offset,
                    fieldGeneratedState.Coords);
            } else if (inState is GridWorldSize worldSizeState) {
                OnGridWorldSize(worldSizeState.XMinOffset,
                    worldSizeState.YMinOffset,
                    worldSizeState.XMaxOffset,
                    worldSizeState.YMaxOffset);
            } else if (inState is BaseGenerated baseGeneratedState) {
                OnBaseGenerate(baseGeneratedState.Offset, baseGeneratedState.Coords, baseGeneratedState.Owner);
            }
        }

        protected override void OnAwake() {
            Controller.Init(data);
            Controller.GenerateGrid();
        }

        private void OnGridWorldSize(float xMinOffset, float yMinOffset, float xMaxOffset, float yMaxOffset) {
            var position = transform.position;
            ApiManager.ProvideManager<CameraManager>()
                      .SetWorldSize(position.x + xMinOffset,
                          position.z + yMinOffset,
                          position.x + xMaxOffset,
                          position.z + yMaxOffset);
        }

        private void GenerateField(GameObject prefab, Vector3 offset, GridCoords mapCoordinates) {
            var thisTransform = transform;
            TileBehaviour.Instantiate(prefab,
                thisTransform,
                thisTransform.position + offset,
                mapCoordinates,
                data.TerrainGeneratorFunction.SampleTerrain);
        }

        private void OnBaseGenerate(Vector3 offset, GridCoords coords, Player owner) {
            var position = transform.position;
            HqBehaviour.Instantiate(position + offset, coords, owner, data.TerrainGeneratorFunction.SampleTerrain);
        }
    }
}
