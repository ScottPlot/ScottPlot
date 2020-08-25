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

        public void SetLimits(double x1, double x2, double y1, double y2, int planeIndex = 0)
        {
            Planes[planeIndex].X.SetLimits(x1, x2);
            Planes[planeIndex].Y.SetLimits(y1, y2);
            RememberAxes();
        }

        public void RememberAxes()
        {
            foreach (var plane in Planes)
            {
                plane.X.Remember();
                plane.Y.Remember();
            }
        }

        public void RecallAxes()
        {
            foreach (var plane in Planes)
            {
                plane.X.Recall();
                plane.Y.Recall();
            }
        }

        public void MousePan(float dX, float dY, bool remember = true, int planeIndex = 0)
        {
            RecallAxes();
            Planes[planeIndex].X.PanPx(dX);
            Planes[planeIndex].Y.PanPx(dY);
            if (remember)
                RememberAxes();
        }

        public void MouseZoom(float dX, float dY, bool remember = true, int planeIndex = 0)
        {
            RecallAxes();
            Planes[planeIndex].X.ZoomPx(dX);
            Planes[planeIndex].Y.ZoomPx(dY);
            if (remember)
                RememberAxes();
        }
    }
}
