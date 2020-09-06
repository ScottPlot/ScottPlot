using ScottPlot.Plottable;
using ScottPlot.Renderable;
using ScottPlot.Renderer;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace ScottPlot
{
    /// <summary>
    /// This class handles dimensions, axis limits, and pixel/position conversion and manipulation.
    /// </summary>
    public class Dimensions
    {
        public List<IAxis1D> XAxes { get; private set; } = new List<IAxis1D>();
        public List<IAxis1D> YAxes { get; private set; } = new List<IAxis1D>();

        public float GetPixelX(double xPosition, int xAxisIndex) => XAxes[xAxisIndex].GetPixel(xPosition);
        public float GetPixelY(double yPosition, int yAxisIndex) => YAxes[yAxisIndex].GetPixel(yPosition);
        public double GetPositionX(float xPixel, int xAxisIndex) => XAxes[xAxisIndex].GetPosition(xPixel);
        public double GetPositionY(float yPixel, int yAxisIndex) => YAxes[yAxisIndex].GetPosition(yPixel);

        public float Width { get; private set; }
        public float Height { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }

        public float DataW => DataOffsetX;
        public float DataE => DataOffsetX + DataWidth - 1;
        public float DataN => DataOffsetY;
        public float DataS => DataOffsetY + DataHeight - 1;
        public Point DataNW => new Point(DataOffsetX, DataOffsetY);
        public Point DataNC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY);
        public Point DataNE => new Point(DataOffsetX + DataWidth, DataOffsetY);
        public Point DataWC => new Point(DataOffsetX, DataOffsetY + DataHeight / 2);
        public Point DataCC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY + DataHeight / 2);
        public Point DataEC => new Point(DataOffsetX + DataWidth, DataOffsetY + DataHeight / 2);
        public Point DataSW => new Point(DataOffsetX, DataOffsetY + DataHeight);
        public Point DataSC => new Point(DataOffsetX + DataWidth / 2, DataOffsetY + DataHeight);
        public Point DataSE => new Point(DataOffsetX + DataWidth, DataOffsetY + DataHeight);

        public Dimensions()
        {
            CreateAxes(1, 1);
        }

        public override string ToString()
        {
            return $"Figure [{Width}, {Height}]; Data [{DataWidth}, {DataHeight}]; " +
                   $"Offset ({DataOffsetX}, {DataOffsetY}); X={XAxes[0]}, Y={YAxes[0]}";
        }

        public void CreateAxes(List<IPlottable> plottables)
        {
            CreateAxes(
                totalX: plottables.Select(x => x.XAxisIndex).Concat(new int[] { 0 }).Max() + 1,
                totalY: plottables.Select(x => x.YAxisIndex).Concat(new int[] { 0 }).Max() + 1);
        }

        public void CreateAxes(int totalX, int totalY)
        {
            while (XAxes.Count < totalX)
                XAxes.Add(new LinearAxis(inverted: false));
            while (YAxes.Count < totalY)
                YAxes.Add(new LinearAxis(inverted: true));
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

        // call this after changing plot dimensions or padding
        private void ResizeAxes()
        {
            foreach (IAxis1D xAxis in XAxes)
                xAxis.Resize(Width, DataWidth, DataOffsetX);
            foreach (IAxis1D yAxis in YAxes)
                yAxis.Resize(Height, DataHeight, DataOffsetY);
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
            ResizeAxes();
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

            ResizeAxes();
        }

        public AxisLimits2D GetLimits(int xAxisIndex, int yAxisIndex)
        {
            return new AxisLimits2D()
            {
                X1 = XAxes.Count() > xAxisIndex ? XAxes[xAxisIndex].Min : double.NaN,
                X2 = XAxes.Count() > xAxisIndex ? XAxes[xAxisIndex].Max : double.NaN,
                Y1 = YAxes.Count() > yAxisIndex ? YAxes[yAxisIndex].Min : double.NaN,
                Y2 = YAxes.Count() > yAxisIndex ? YAxes[yAxisIndex].Max : double.NaN,
            };
        }

        public void SetLimits(AxisLimits2D limits, int xAxisIndex, int yAxisIndex)
        {
            XAxes[xAxisIndex].SetLimits(limits.X1, limits.X2);
            YAxes[yAxisIndex].SetLimits(limits.Y1, limits.Y2);
        }

        public void SetLimits(double x1, double x2, double y1, double y2, int xAxisIndex, int yAxisIndex)
        {
            XAxes[xAxisIndex].SetLimits(x1, x2);
            YAxes[yAxisIndex].SetLimits(y1, y2);
        }

        public void MousePan(float dX, float dY, int xAxisIndex, int yAxisIndex)
        {
            XAxes[xAxisIndex].PanPx(dX);
            YAxes[yAxisIndex].PanPx(dY);
        }

        public void MouseZoom(float dX, float dY, int xAxisIndex, int yAxisIndex)
        {
            XAxes[xAxisIndex].ZoomPx(dX);
            YAxes[yAxisIndex].ZoomPx(dY);
        }
    }
}
