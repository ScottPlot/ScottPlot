using System;
using System.Linq;

namespace ScottPlot
{
    public static class Marker
    {
        private static readonly Random Rand = new(0);

        public static MarkerShape Random()
        {
            return Random(Rand);
        }

        public static MarkerShape[] GetMarkers()
        {
            return Enum.GetValues(typeof(MarkerShape))
                .Cast<MarkerShape>()
                .Where(x => x is not MarkerShape.none)
                .ToArray();
        }

        public static MarkerShape Random(Random rand)
        {
            MarkerShape[] markers = GetMarkers();
            return markers[rand.Next(markers.Length)];
        }

        /// <summary>
        /// Create a new Marker (IMarker class) from an old marker (MarkerStyle enum)
        /// </summary>
        public static IMarker Create(MarkerShape shape)
        {
            return shape switch
            {
                MarkerShape.none => new MarkerShapes.None(),
                MarkerShape.filledCircle => new MarkerShapes.FilledCircle(),
                MarkerShape.filledSquare => new MarkerShapes.FilledSquare(),
                MarkerShape.openCircle => new MarkerShapes.OpenCircle(),
                MarkerShape.openSquare => new MarkerShapes.OpenSquare(),
                MarkerShape.filledDiamond => new MarkerShapes.FilledDiamond(),
                MarkerShape.openDiamond => new MarkerShapes.OpenDiamond(),
                MarkerShape.asterisk => new MarkerShapes.Asterisk(),
                MarkerShape.hashTag => new MarkerShapes.Hashtag(),
                MarkerShape.cross => new MarkerShapes.Cross(),
                MarkerShape.eks => new MarkerShapes.Eks(),
                MarkerShape.verticalBar => new MarkerShapes.VerticalBar(),
                MarkerShape.triUp => new MarkerShapes.TriStarUp(),
                MarkerShape.triDown => new MarkerShapes.TriStarDown(),
                MarkerShape.filledTriangleUp => new MarkerShapes.FilledTriangleUp(),
                MarkerShape.filledTriangleDown => new MarkerShapes.FilledTriangleDown(),
                MarkerShape.openTriangleUp => new MarkerShapes.OpenTriangleUp(),
                MarkerShape.openTriangleDown => new MarkerShapes.OpenTriangleDown(),
                _ => throw new NotImplementedException($"unsupported {shape.GetType()}: {shape}"),
            };
        }
    }
}
