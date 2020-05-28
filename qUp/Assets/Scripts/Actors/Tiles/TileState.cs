using Base.Interfaces;
using UnityEngine;

namespace Actors.Tiles {
    
    public interface ITileState : IState { }
    
    public abstract class TileState<TState> : State<TState>, ITileState where TState : class, new() { }

    public class HighlightActivated : TileState<HighlightActivated> {
        public Color HighlightColor { get; private set; }

        public static HighlightActivated With(Color highlightColor) {
            Cache.HighlightColor = highlightColor;
            return Cache;
        }
    }

    public class OwnershipChanged : TileState<OwnershipChanged> {
        public Color OwnerColor { get; private set; }

        public static OwnershipChanged With(Color ownerColor) {
            Cache.OwnerColor = ownerColor;
            return Cache;
        }
    }

    public class Idle : TileState<Idle> {
        public static Idle With() {
            return Cache;
        }
    }
}