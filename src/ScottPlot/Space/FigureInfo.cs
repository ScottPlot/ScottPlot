using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// This class handles dimensions, axis limits, and pixel/unit conversion and manipulation.
    /// </summary>
    public class FigureInfo
    {
        public readonly List<Plane> Planes = new List<Plane>();

        public float GetPixelX(double unitX, int planeIndex = 0) => Planes[planeIndex].X.GetPixel(unitX, inverted: false);
        public float GetPixelY(double unitY, int planeIndex = 0) => Planes[planeIndex].Y.GetPixel(unitY, inverted: true);

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }

        public FigureInfo()
        {
            Planes.Add(new Plane()); // primary Y
            Planes.Add(new Plane()); // secondary Y
        }

        public void Resize(float width, float height, float dataWidth, float dataHeight, float dataOffsetX, float dataOffsetY)
        {
            Width = width;
            Height = height;
            DataWidth = dataWidth;
            DataHeight = dataHeight;
            DataOffsetX = dataOffsetX;
            DataOffsetY = dataOffsetY;

            foreach (var plane in Planes)
            {
                plane.X.Resize(Width, DataWidth, DataOffsetX);
                plane.Y.Resize(Height, DataHeight, DataOffsetY);
            }
        }

        public void SetLimits(double x1, double x2, double y1, double y2, int planeIndex = 0)
        {
            Planes[planeIndex].X.SetLimits(x1, x2);
            Planes[planeIndex].Y.SetLimits(y1, y2);
        }
    }
}
