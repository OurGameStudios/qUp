using Actors.Hqs;
using Actors.Players;
using Actors.Tiles;
using Base;
using Base.MonoBehaviours;
using Common;
using Managers;
using Managers.CameraManagers;
using UnityEngine;

namespace Actors.Grid.Generator {
    public class GridGeneratorBehaviour : BaseMonoBehaviour<GridGenerator, GridGeneratorState> {
        public GridGeneratorData data;

        protected override void OnStateHandler(GridGeneratorState inState) {
            if (inState is FieldGenerated fieldGeneratedState) {
                OnFieldGenerated(fieldGeneratedState.Prefab,
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
        }

        private void OnGridWorldSize(float xMinOffset, float yMinOffset, float xMaxOffset, float yMaxOffset) {
            var position = transform.position;
            GlobalManager.GetManager<CameraManager>()
                         .SetWorldSize(position.x + xMinOffset,
                             position.z + yMinOffset,
                             position.x + xMaxOffset,
                             position.z + yMaxOffset);
        }

        private void OnFieldGenerated(GameObject prefab, Vector3 offset, GridCoords mapCoordinates) {
            var thisTransform = transform;
            TileBehaviour.Instantiate(prefab, thisTransform, thisTransform.position + offset, mapCoordinates);
        }

        private void OnBaseGenerate(Vector3 offset, GridCoords coords, Player owner) {
            HqBehaviour.Instantiate(transform.position + offset, coords, owner);
        }
    }
}
