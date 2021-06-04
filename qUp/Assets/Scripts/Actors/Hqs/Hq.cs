using System;
using Actors.Players;
using Actors.Tiles;
using Actors.Units;
using Common;
using UnityEngine;
using Wrappers.Shaders;

namespace Actors.Hqs {
    public class Hq : Tile, IHq {

        [SerializeField]
        private GameObject hqDecorator;

        [SerializeField]
        private MeshRenderer hqDecoratorMeshRenderer;

        private IPlayer owner;

        public static Hq Instantiate(Transform parent, Vector3 position, GridCoords coords,
                                     Func<Vector2, float> sampleHeight, IPlayer owner) {
            var hq = Instantiate(owner.GetHqPrefab(), position, Quaternion.identity, parent).GetComponent<Hq>();
            hq.owner = owner;
            hq.Init(coords, sampleHeight);
            return hq;
        }

        protected override void Init(GridCoords coords, Func<Vector2, float> sampleHeight) {
            base.Init(coords, sampleHeight);
            hqDecorator.transform.position = GetTileCenter();
            hqDecoratorMeshRenderer.materials[1].color = owner.GetPlayerColor();
        }

        public IPlayer GetOwner() => owner;

        public override bool IsAvailableForTick(int tick, IPlayer player, IUnit unit) => false;
    }
}
