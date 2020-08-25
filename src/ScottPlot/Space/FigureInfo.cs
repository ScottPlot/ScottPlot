using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Space
{
    /// <summary>
    /// This class handles dimensions, axis limits, and pixel/position conversion and manipulation.
    /// </summary>
    public class FigureInfo
    {
        public readonly List<Plane> Planes = new List<Plane>();

        public float GetPixelX(double xPosition, int planeIndex = 0) => Planes[planeIndex].X.GetPixel(xPosition);
        public float GetPixelY(double yPosition, int planeIndex = 0) => Planes[planeIndex].Y.GetPixel(yPosition);
        public double GetPositionX(float xPixel, int planeIndex = 0) => Planes[planeIndex].X.GetPosition(xPixel);
        public double GetPositionY(float yPixel, int planeIndex = 0) => Planes[planeIndex].Y.GetPosition(yPixel);

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }

        public FigureInfo()
        {
            var xPrimary = new LinearAxis(inverted: false);
            var yPrimary = new LinearAxis(inverted: true);
            var ySecondary = new LinearAxis(inverted: true);

            Planes.Add(new Plane(xPrimary, yPrimary));
            Planes.Add(new Plane(xPrimary, ySecondary));
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

        public AxisLimits GetLimits(int planeIndex = 0)
        {
            return new AxisLimits()
            {
                X1 = Planes[planeIndex].X.Min,
                X2 = Planes[planeIndex].X.Max,
                Y1 = Planes[planeIndex].Y.Min,
                Y2 = Planes[planeIndex].Y.Max,
            };
        }

        public void SetLimits(AxisLimits limits, int planeIndex = 0)
        {
            Planes[planeIndex].X.SetLimits(limits.X1, limits.X2);
            Planes[planeIndex].Y.SetLimits(limits.Y1, limits.Y2);
        }

        public void SetLimits(double x1, double x2, double y1, double y2, int planeIndex = 0)
        {
            Planes[planeIndex].X.SetLimits(x1, x2);
            Planes[planeIndex].Y.SetLimits(y1, y2);
        }

        public void MousePan(float dX, float dY, int planeIndex = 0)
        {
            Planes[planeIndex].X.PanPx(dX);
            Planes[planeIndex].Y.PanPx(dY);
        }

        public void MouseZoom(float dX, float dY, int planeIndex = 0)
        {
            Planes[planeIndex].X.ZoomPx(dX);
            Planes[planeIndex].Y.ZoomPx(dY);
        }
    }
}
