using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ScottPlot
{
    /// <summary>
    /// This class handles dimensions, axis limits, and pixel/position conversion and manipulation.
    /// </summary>
    public class PlotInfo
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

        public float DataW => DataOffsetX;
        public float DataE => DataOffsetX + DataWidth;
        public float DataN => DataOffsetY;
        public float DataS => DataOffsetY + DataHeight;
        public Point DataNW => new Point(DataOffsetX, DataOffsetY);
        public Point DataNC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY);
        public Point DataNE => new Point(DataOffsetX + DataWidth, DataOffsetY);
        public Point DataWC => new Point(DataOffsetX, DataOffsetY + DataHeight / 2);
        public Point DataCC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY + DataHeight / 2);
        public Point DataEC => new Point(DataOffsetX + DataWidth, DataOffsetY + DataHeight / 2);
        public Point DataSW => new Point(DataOffsetX, DataOffsetY + DataHeight);
        public Point DataSC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY + DataHeight);
        public Point DataSE => new Point(DataOffsetX + DataWidth, DataOffsetY + DataHeight);

        public bool LimitsHaveBeenSet { get; private set; }

        public PlotInfo()
        {
            var xPrimary = new LinearAxis(inverted: false);
            var yPrimary = new LinearAxis(inverted: true);
            var ySecondary = new LinearAxis(inverted: true);

            Planes.Add(new Plane(xPrimary, yPrimary));
            Planes.Add(new Plane(xPrimary, ySecondary));
        }

        public override string ToString()
        {
            return $"Figure [{Width}, {Height}]; Data [{DataWidth}, {DataHeight}]; " +
                   $"Offset ({DataOffsetX}, {DataOffsetY}); Plane: {Planes[0]}";
        }

        private Padding CurrentPadding()
        {
            return new Padding
            {
                Left = DataOffsetX,
                Right = Width - DataOffsetX - DataWidth,
                Above = DataOffsetY,
                Below = Height - DataOffsetY - DataHeight
            };
        }

        /// <summary>
        /// Resize while preserving padding
        /// </summary>
        public void UpdateSize(float width, float height)
        {
            var oldPad = CurrentPadding();
            Width = width;
            Height = height;
            DataWidth = width - oldPad.TotalHorizontal;
            DataHeight = height - oldPad.TotalVertical;
            DataOffsetX = oldPad.Left;
            DataOffsetY = oldPad.Above;

            foreach (var plane in Planes)
            {
                plane.X.Resize(Width, DataWidth, DataOffsetX);
                plane.Y.Resize(Height, DataHeight, DataOffsetY);
            }
        }

        /// <summary>
        /// Change padding while preserving size
        /// </summary>
        public void UpdatePadding(float? left, float? right, float? above, float? below)
        {
            var pad = CurrentPadding();
            pad.Left = left ?? pad.Left;
            pad.Right = right ?? pad.Right;
            pad.Above = above ?? pad.Above;
            pad.Below = below ?? pad.Below;

            DataWidth = Width - pad.TotalHorizontal;
            DataHeight = Height - pad.TotalVertical;
            DataOffsetX = pad.Left;
            DataOffsetY = pad.Above;

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
            LimitsHaveBeenSet = true;
        }

        public void SetLimits(double x1, double x2, double y1, double y2, int planeIndex = 0)
        {
            Planes[planeIndex].X.SetLimits(x1, x2);
            Planes[planeIndex].Y.SetLimits(y1, y2);
            LimitsHaveBeenSet = true;
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
