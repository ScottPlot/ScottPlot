using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public enum MarkerShape
    {
        Circle
    }

    public struct Marker
    {
        public Color Color { get; set; } = Colors.Black;

        public float Size { get; set; } = 5;

        public MarkerShape Shape { get; set; } = MarkerShape.Circle;

        public Marker()
        {
        }

        public Marker(Color color)
        {
            Color = color;
        }
        
        public Marker(MarkerShape shape, Color color, float size = 5)
        {
            Shape = shape;
            Color = color;
            Size = size;
        }
    }
}
