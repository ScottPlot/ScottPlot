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
    }
}
