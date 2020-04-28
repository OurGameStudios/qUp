using Base.Interfaces;
using UnityEngine;

namespace Actors.Tiles {
    public class TileState : IState { }

    public class MarkingsChange : TileState {
        public Color MarkingColor { get; }

        public MarkingsChange(Color markingColor) {
            MarkingColor = markingColor;
        }
    }

    public class HighlightActivated : TileState {
        public Color HighlightColor { get; }

        public HighlightActivated(Color highlightColor) {
            HighlightColor = highlightColor;
        }
    }

    public class Idle : TileState { }
}
