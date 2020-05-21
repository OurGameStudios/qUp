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
        public Color BaseColor { get; private set; }
        
        public static HighlightActivated With(Color BaseColor, Color highlightColor) {
            Cache.BaseColor = BaseColor;
            Cache.HighlightColor = highlightColor;
            return Cache;
        }
    }

    public class Idle : TileState<Idle> {
        public Color Color { get; private set; }
        
        public static Idle With(Color color) {
            Cache.Color = color;
            return Cache;
        }
    }
}