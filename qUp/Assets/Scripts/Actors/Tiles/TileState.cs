using Base.Interfaces;
using UnityEngine;

namespace Actors.Tiles {
    
    public interface ITileState : IState { }
    
    public abstract class TileState<TState> : State<TState>, ITileState where TState : class, new() { }

    public class MarkingsChange : TileState<MarkingsChange> {
        public Color MarkingColor { get; private set; }

        public static MarkingsChange With(Color markingColor) {
            Cache.MarkingColor = markingColor;
            return Cache;
        }
    }

    public class HighlightActivated : TileState<HighlightActivated> {
        public Color HighlightColor { get; private set; }
        
        public static HighlightActivated With(Color highlightColor) {
            Cache.HighlightColor = highlightColor;
            return Cache;
        }
    }

    public class Idle : TileState<Idle> {
        public static Idle With() => Cache;
    }
}