using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common {
    [Serializable]
    public struct GridCoords : IEquatable<GridCoords> {
        /// <summary>
        /// Coordinates of (0, 0).
        /// </summary>
        public static GridCoords Origin { get; } = new GridCoords(0, 0);

        /// <summary>
        /// Coordinates of (0, 2) or 1 tile up from origin.
        /// </summary>
        public static GridCoords Up { get; } = new GridCoords(0, 2);

        /// <summary>
        /// Coordinates of (0, -2) or 1 down from origin.
        /// </summary>
        public static GridCoords Down { get; } = new GridCoords(0, -2);

        /// <summary>
        /// Coordinates of (1, 1) or upper right tile from origin.
        /// </summary>
        public static GridCoords UpRight { get; } = new GridCoords(1, 1);

        /// <summary>
        /// Coordinates of (-1, 1) or upper left tile from origin.
        /// </summary>
        public static GridCoords UpLeft { get; } = new GridCoords(-1, 1);

        /// <summary>
        /// Coordinates of (1, -1) or upper right tile from origin.
        /// </summary>
        public static GridCoords DownRight { get; } = new GridCoords(1, -1);

        /// <summary>
        /// Coordinates of (-1, -1) or lower left tile from origin.
        /// </summary>
        public static GridCoords DownLeft { get; } = new GridCoords(-1, -1);
        
        private static List<GridCoords> neighbourTransforms = new List<GridCoords> {Up, Down, UpRight, UpLeft, DownRight, DownLeft};

        /// <summary>
        /// Transform coordinates that are needed to find all neighbours; 
        /// </summary>
        /// <returns>Up, Down, UpRight, UpLeft, DownRight, DownLeft</returns>
        public static List<GridCoords> NeighbourTransforms => neighbourTransforms;

        /// <summary>
        /// GridCoords X coordinate on grid
        /// </summary>
        public int x;

        /// <summary>
        /// GridCoords Y coordinate on grid
        /// </summary>
        public int y;


        public GridCoords(int x, int y) {
            if (!AreValidGridCoords(x, y)) {
                throw new ArgumentException($"({x}, {y}) Coordinates do not conform to (x + y) % 2 == 0");
            }

            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Sets X and Y coordinates.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <exception cref="ArgumentException">Coordinates must conform to (x + y) % 2 == 0</exception>
        public void SetCoords(int x, int y) {
            if (!AreValidGridCoords(x, y)) {
                throw new ArgumentException($"({x}, {y}) Coordinates do not conform to (x + y) % 2 == 0");
            }

            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Checks if coordinates conform to doubled height system.
        /// Rule is (x + y) % 2 == 0
        /// </summary>
        /// <returns>True if coordinates are valid</returns>
        public static bool AreValidGridCoords(int x, int y) {
            return (x + y) % 2 == 0;
        }

        /// <summary>
        /// Checks if coordinates conform to doubled height system.
        /// Rule is (x + y) % 2 == 0
        /// </summary>
        /// <returns>True if coordinates are valid</returns>
        public static bool AreValidGridCoords((int x, int y) coordinates) =>
            AreValidGridCoords(coordinates.x, coordinates.y);

        /// <summary>
        ///  Checks if two are next to eachother on grid.
        /// </summary>
        /// <returns>Are neighbours</returns>
        public static bool AreNeighbours(GridCoords a, GridCoords b) => NeighbourTransforms.Contains(a - b);

        /// <summary>
        /// Returns all possible neighbour coordinates of coords.
        /// Doesn't take holes into account.
        /// </summary>
        /// <returns>Neighbour coordinates</returns>
        public static List<GridCoords> GetNeighbourCoords(GridCoords coords) {
            var neighbourCoords = new List<GridCoords>();
            for (var i = 0; i < 6; i++) {
                neighbourCoords.Add(NeighbourTransforms[i] + coords);
            }

            return neighbourCoords;
        }

        /// <summary>
        /// Returns all neighbours of coords in current grid.
        /// Doesn't take holes into account.
        /// </summary>
        /// <param name="maxGridCoords">Maximum coordinates in grid.</param>
        /// <returns>Neighbour coordinates</returns>
        public static List<GridCoords> GetNeighbourCoordsOfGrid(GridCoords coords, GridCoords maxGridCoords) {
            var neighbourCoords = new List<GridCoords>();
            for (var i = 0; i < 6; i++) {
                var possibleNeighbour = NeighbourTransforms[i] + coords;
                if ((possibleNeighbour.x >= Origin.x && possibleNeighbour.y >= Origin.y) && possibleNeighbour < maxGridCoords) {
                    neighbourCoords.Add(possibleNeighbour);
                }
            }

            return neighbourCoords;
        }

        /// <summary>
        /// Checks if this next to other on grid.
        /// </summary>
        /// <returns>Is a neighbour</returns>
        public bool IsNeighbourOf(GridCoords other) => NeighbourTransforms.Contains(this - other);

        /// <summary>
        /// Returns all possible neighbour coordinates.
        /// Doesn't take holes into account.
        /// </summary>
        /// <returns>Neighbour coordinates</returns>
        public List<GridCoords> GetNeighbourCoords() => GetNeighbourCoords(this);

        /// <summary>
        /// Returns all neighbours in current grid.
        /// Doesn't take holes into account.
        /// </summary>
        /// <param name="maxGridCoords">Maximum coordinates in grid.</param>
        /// <returns>Neighbour coordinates</returns>
        public List<GridCoords> GetNeighbourCoordsOfGrid(GridCoords maxGridCoords) =>
            GetNeighbourCoordsOfGrid(this, maxGridCoords);

        /// <summary>
        /// Calculates the number of hexagon tiles from a to b;
        /// </summary>
        /// <returns>Number of hexagon tiles</returns>
        public static int Distance(GridCoords a, GridCoords b) {
            var dx = Math.Abs(a.x - b.x);
            var dy = Math.Abs(a.y - b.y);
            return dx + Math.Max(0, (dy - dx) / 2);
        }

        /// <summary>
        /// Calculates the number of hexagon tiles from this to target;
        /// </summary>
        /// <returns>Number of hexagon tiles</returns>
        public int DistanceTo(GridCoords target) {
            var dx = Math.Abs(x - target.x);
            var dy = Math.Abs(y - target.y);
            return dx + Math.Max(0, (dy - dx) / 2);
        }

        /// <summary>
        /// Calculates coordinates mirrored in reference to another coordinates.
        /// </summary>
        /// <param name="coords">Coordinates to be mirrored.</param>
        /// <param name="mirrorCoords">Coordinates across which mirror will be calculated.</param>
        /// <returns>Mirrored grid coordinates</returns>
        public static GridCoords MirrorFromPoint(GridCoords coords, GridCoords mirrorCoords) =>
            mirrorCoords * 2 - coords;

        /// <summary>
        /// Calculates coordinates mirrored in grid of defined size.
        /// </summary>
        /// <param name="coords">Coordinates to be mirrored.</param>
        /// <param name="maxGridCoords">Maximum coordinates in grid.</param>
        /// <returns>Mirrored grid coordinates</returns>
        public static GridCoords MirrorFromGrid(GridCoords coords, GridCoords maxGridCoords) => maxGridCoords - coords;

        /// <summary>
        /// Calculates coordinates mirrored in reference to another coordinates.
        /// </summary>
        /// <param name="mirrorCoords">Coordinates across which mirror will be calculated.</param>
        /// <returns>Mirrored grid coordinates</returns>
        public GridCoords MirrorFromPoint(GridCoords mirrorCoords) => 2 * mirrorCoords - this;

        /// <summary>
        /// Calculates coordinates mirrored in grid of defined size.
        /// </summary>
        /// <param name="maxGridCoords">Maximum coordinates in grid.</param>
        /// <returns>Mirrored grid coordinates</returns>
        public GridCoords MirrorFromGrid(GridCoords maxGridCoords) => maxGridCoords - this;

        /// <summary>
        /// Calculates coordinates in path from source to target.
        /// </summary>
        /// <param name="source">Start coordinates.</param>
        /// <param name="target">End coordinates.</param>
        /// <param name="isYMax">Set to true if source or target have max Y coordinate in grid.</param>
        /// <returns>Array of tiles in path</returns>
        public static List<GridCoords> Path(GridCoords source, GridCoords target, bool isYMax = false) {
            var n = Distance(source, target);
            if (n == 0) {
                return new List<GridCoords> {source, target};
            }

            var path = new List<GridCoords>(n + 1);
            for (var i = 0; i < n + 1; i++) {
                path.Add(PushY(Lerp(source, target, i * (1f / n)), isYMax ? -1 : 1));
            }

            return path;
        }

        /// <summary>
        /// Calculates coordinates in path from source to target.
        /// </summary>
        /// <param name="source">Start coordinates.</param>
        /// <param name="target">End coordinates.</param>
        /// <param name="maxY">Set to max Y coordinate in grid.</param>
        /// <returns>Array of tiles in path</returns>
        public static List<GridCoords> Path(GridCoords source, GridCoords target, int maxY = int.MaxValue) =>
            Path(source, target, source.y >= maxY || target.y >= maxY);

        /// <summary>
        /// Calculates coordinates in path from this to target.
        /// </summary>
        /// <param name="target">End coordinates.</param>
        /// <param name="isYMax">Set to true if source or target have max Y coordinate in grid.</param>
        /// <returns>Array of tiles in path</returns>
        public List<GridCoords> PathTo(GridCoords target, bool isYMax) => Path(this, target, isYMax);

        /// <summary>
        /// Calculates coordinates in path from this to target.
        /// </summary>
        /// <param name="target">End coordinates.</param>
        /// <param name="maxY">Set to max Y coordinate in grid.</param>
        /// <returns>Array of tiles in path,</returns>
        public List<GridCoords> PathTo(GridCoords target, int maxY) => Path(this, target, maxY);

        /// <summary>
        /// Calculates coordinates in range of coords.
        /// </summary>
        /// <param name="coords">Coordinates in center of range.</param>
        /// <param name="range">Maximum distance from coords. Center of range.</param>
        /// <returns>List of coordinates in range with center coordinate.</returns>
        public static List<GridCoords> RangeOf(GridCoords coords, int range) {
            var coordsInRange = new List<GridCoords>();
            for (var rx = coords.x - range; rx <= coords.x + range; rx++) {
                for (var ry = coords.x - 2 * range + Math.Abs(coords.x - rx);
                     ry <= coords.x + 2 * range - Math.Abs(coords.x - rx);
                     ry += 2) {
                    coordsInRange.Add(new GridCoords(rx, ry));
                }
            }

            return coordsInRange;
        }

        public List<GridCoords> InRange(int range) => RangeOf(this, range);

        /// <summary>
        /// Count of Coordinates in range.
        /// </summary>
        /// <param name="range">range from origin.</param>
        public static int CountInRange(int range) => range * (range + 1) / 2 * 6;


        public static GridCoords SwapXY(GridCoords coords) => new GridCoords(coords.y, coords.x);

        /// <summary>
        /// Lerps between two coords and rounds its x and y to nearest int.
        /// If x or y are 0.5 then it rounds it to even int.
        /// Doesnt return GridCoords because return value could be invalid grid coordinate.
        /// Should be used with Push to get valid coords.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static (int x, int y) Lerp(GridCoords a, GridCoords b, float threshold) {
            return (Mathf.RoundToInt(Mathf.Lerp(a.x, b.x, threshold)),
                    Mathf.RoundToInt(Mathf.Lerp(a.y, b.y, threshold)));
        }

        /// <summary>
        /// If necessary pushes invalid coordinates to valid coordinates in sign of direction on Y axis.
        /// </summary>
        /// <param name="coords">Invalid coordinates.</param>
        /// <param name="direction">Up or down as sign of number.</param>
        /// <returns>Valid coordinates</returns>
        public static GridCoords PushY((int x, int y) coords, int direction) {
            if (!AreValidGridCoords(coords)) {
                coords.y += Math.Sign(direction);
            }

            return coords;
        }

        /// <summary>
        /// If necessary pushes invalid coordinates to valid coordinates in sign of direction on X axis.
        /// Returns coords if they are valid.
        /// </summary>
        /// <param name="coords">Invalid coordinates.</param>
        /// <param name="direction">Up or down as sign of number.</param>
        /// <returns>Valid coordinates</returns>
        public static GridCoords PushX((int x, int y) coords, int direction) {
            if (!AreValidGridCoords(coords)) {
                coords.x += Math.Sign(direction);
            }

            return coords;
        }

        // Addition
        public static GridCoords operator +(GridCoords a, GridCoords b) => new GridCoords(a.x + b.x, a.y + b.y);

        public static GridCoords operator +(GridCoords a, int b) => new GridCoords(a.x + b, a.y + b);

        // Subtraction
        public static GridCoords operator -(GridCoords a, GridCoords b) => new GridCoords(a.x - b.x, a.y - b.y);

        public static GridCoords operator -(GridCoords a, int b) => new GridCoords(a.x - b, a.y - b);

        // Multiplication
        public static GridCoords operator *(GridCoords a, GridCoords b) => new GridCoords(a.x * b.x, a.y * b.y);

        public static GridCoords operator *(GridCoords a, int b) => new GridCoords(a.x * b, a.y * b);

        public static GridCoords operator *(int a, GridCoords b) => new GridCoords(a * b.x, a * b.y);

        // Division
        public static GridCoords operator /(GridCoords a, GridCoords b) => new GridCoords(a.x / b.x, a.y / b.y);

        public static GridCoords operator /(GridCoords a, int b) => new GridCoords(a.x / b, a.y / b);

        // Modulo
        public static GridCoords operator %(GridCoords a, GridCoords b) => new GridCoords(a.x % b.x, a.y % b.y);

        public static GridCoords operator %(GridCoords a, int b) => new GridCoords(a.x % b, a.y % b);

        // Equals
        public static bool operator ==(GridCoords a, GridCoords b) => a.x == b.x && a.y == b.y;
        
        public static bool operator ==(GridCoords a, (int x, int y) b) => a.x == b.x && a.y == b.y;

        //UnEquals
        public static bool operator !=(GridCoords a, GridCoords b) => a.x != b.x && a.y != b.y;
        
        public static bool operator !=(GridCoords a, (int x, int y) b) => a.x != b.x && a.y != b.y;
        public static bool operator <(GridCoords a, GridCoords b) => a.x < b.x || a.y < b.y;

        public static bool operator >(GridCoords a, GridCoords b) => a.x > b.x || a.y > b.y;

        // Implicit asignment

        public static implicit operator GridCoords((int x, int y) gridCoordinates) {
            return new GridCoords(gridCoordinates.x, gridCoordinates.y);
        }

        public static implicit operator (int, int)(GridCoords gridCoords) {
            return (gridCoords.x, gridCoords.y);
        }

        public static implicit operator Vector2(GridCoords gridCoordinates) {
            return new Vector2(gridCoordinates.x, gridCoordinates.y);
        }

        public override string ToString() => $"({x,2},{y,2})";

        // Equals functions
        public override bool Equals(object obj) => obj is GridCoords other && Equals(other);

        public bool Equals(GridCoords other) => x == other.x && y == other.y;

        // Other
        public override int GetHashCode() {
            unchecked {
                return (x * 397) ^ y;
            }
        }
    }
}
