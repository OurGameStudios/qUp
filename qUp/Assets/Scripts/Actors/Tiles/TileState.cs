using Base.Interfaces;
using UnityEngine;

namespace Actors.Tiles {
    public class TileState : IState { }

    public class Highlight : TileState{
        private Color HighlightColor { get; }

        public Highlight(Color highlightColor) {
            HighlightColor = highlightColor;
        }
    }
}
