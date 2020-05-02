using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actors.Hqs;
using Actors.Players;
using Actors.Tiles;
using Base.MonoBehaviours;
using Common;
using Extensions;
using Managers;
using Managers.CameraManagers;
using UnityEngine;
using UnityEngine.AI;

namespace Actors.Grid.Generator {
    public class GridGeneratorBehaviour : BaseMonoBehaviour<GridGenerator, GridGeneratorState> {
        public GridGeneratorData data;

        
        private List<FieldGenerated> generatedFields = new List<FieldGenerated>();

        protected override void OnStateHandler(GridGeneratorState inState) {
            if (inState is FieldGenerated fieldGeneratedState) {
                // generatedFields.Add(fieldGeneratedState);
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

        private void Start() {
            Controller.Init(data);
            Controller.GenerateGrid();
            // StartCoroutine(AsyncGridGenerator());
        }

        private IEnumerator AsyncGridGenerator() {
            yield return generatedFields.Select(field => AsyncGenerateField(field.Prefab, field.Offset, field.Coords))
                                        .GetEnumerator();
            yield return null;
        }

        private void OnGridWorldSize(float xMinOffset, float yMinOffset, float xMaxOffset, float yMaxOffset) {
            var position = transform.position;
            GlobalManager.GetManager<CameraManager>()
                         .SetWorldSize(position.x + xMinOffset,
                             position.z + yMinOffset,
                             position.x + xMaxOffset,
                             position.z + yMaxOffset);
        }

        private IEnumerator AsyncGenerateField(GameObject prefab, Vector3 offset, GridCoords mapCoordinates) {
            var thisTransform = transform;
            TileBehaviour.Instantiate(prefab,
                thisTransform,
                thisTransform.position + offset,
                mapCoordinates,
                data.TerrainGeneratorFunction.SampleTerrain);
            yield return null;
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
