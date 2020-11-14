using ScottPlot.Renderable;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;
using System.Globalization;
using System.Diagnostics;

namespace ScottPlot
{
    /// <summary>
    /// This module holds state for figure dimensions, axis limits, plot contents, and styling options.
    /// A plot can be duplicated by copying the full stae of this settings module.
    /// </summary>
    public class Settings
    {
        // plottables
        public readonly List<IRenderable> Plottables = new List<IRenderable>();
        public Color GetNextColor() { return PlottablePalette.GetColor(Plottables.Count); }

        // renderable objects the user can customize
        public readonly FigureBackground FigureBackground = new FigureBackground();
        public readonly DataBackground DataBackground = new DataBackground();
        public readonly BenchmarkMessage BenchmarkMessage = new BenchmarkMessage();
        public readonly ErrorMessage ErrorMessage = new ErrorMessage();
        public readonly Legend CornerLegend = new Legend();
        public readonly ZoomRectangle ZoomRectangle = new ZoomRectangle();
        public Palette PlottablePalette = Palette.Category10;

        // the Axes list stores styling info for each axis and its limits
        public List<Axis> Axes = new List<Axis>() {
            new DefaultLeftAxis(),
            new DefaultRightAxis(),
            new DefaultBottomAxis(),
            new DefaultTopAxis()
        };

        public int[] XAxisIndexes => Axes.Where(x => x.IsHorizontal).Select(x => x.AxisIndex).Distinct().ToArray();
        public int[] YAxisIndexes => Axes.Where(x => x.IsVertical).Select(x => x.AxisIndex).Distinct().ToArray();
        public Axis GetXAxis(int xAxisIndex) => Axes.Where(x => x.IsHorizontal && x.AxisIndex == xAxisIndex).First();
        public Axis GetYAxis(int yAxisIndex) => Axes.Where(x => x.IsVertical && x.AxisIndex == yAxisIndex).First();

        public void RememberAxisLimits()
        {
            foreach (Axis axis in Axes)
                axis.Dims.Remember();
        }

        public void RecallAxisLimits()
        {
            foreach (Axis axis in Axes)
                axis.Dims.Recall();
        }

        // shortcuts to fixed axes indexes
        public Axis YAxis => Axes[0];
        public Axis YAxis2 => Axes[1];
        public Axis XAxis => Axes[2];
        public Axis XAxis2 => Axes[3];
        public Axis[] PrimaryAxes => Axes.Take(4).ToArray();

        /// <summary>
        /// Resize the layout by padding the data area based on the size of all axes
        /// </summary>
        public void TightenLayout()
        {
            Edge[] edges = { Edge.Left, Edge.Right, Edge.Top, Edge.Bottom };
            foreach (var edge in edges)
            {
                float offset = 0;
                foreach (var axis in Axes.Where(x => x.Edge == edge))
                {
                    axis.PixelOffset = offset;
                    offset += axis.PixelSize;
                }
            }

            float padLeft = Axes.Where(x => x.Edge == Edge.Left).Select(x => x.PixelSize).Sum();
            float padRight = Axes.Where(x => x.Edge == Edge.Right).Select(x => x.PixelSize).Sum();
            float padBottom = Axes.Where(x => x.Edge == Edge.Bottom).Select(x => x.PixelSize).Sum();
            float padTop = Axes.Where(x => x.Edge == Edge.Top).Select(x => x.PixelSize).Sum();

            foreach (Axis axis in Axes)
            {
                if (axis.IsHorizontal)
                    axis.Dims.SetPadding(padLeft, padRight);
                else
                    axis.Dims.SetPadding(padTop, padBottom);
            }
        }

        public bool AllAxesHaveBeenSet => Axes.All(x => x.Dims.HasBeenSet);

        // TODO: This should be readonly and a Resize() method updates sizes for all Axes (while retaining data size and offset)
        public int Width => (int)XAxis.Dims.FigureSizePx;
        public int Height => (int)YAxis.Dims.FigureSizePx;
        public float DataOffsetX => XAxis.Dims.DataOffsetPx;
        public float DataOffsetY => YAxis.Dims.DataOffsetPx;
        public float DataWidth => XAxis.Dims.DataSizePx;
        public float DataHeight => YAxis.Dims.DataSizePx;

        /*
         * ##################################################################################
         * # OLD SETTINGS WHICH I AM WORKING TO STRANGLE
         * 
         */

        public PlotDimensions GetPlotDimensions(int xAxisIndex, int yAxisIndex)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);

            // determine figure dimensions based on primary X and Y axis
            var figureSize = new SizeF(XAxis.Dims.FigureSizePx, YAxis.Dims.FigureSizePx);
            var dataSize = new SizeF(XAxis.Dims.DataSizePx, YAxis.Dims.DataSizePx);
            var dataOffset = new PointF(XAxis.Dims.DataOffsetPx, YAxis.Dims.DataOffsetPx);

            // determine figure dimensions based on specific X and Y axes
            //var figureSize = new SizeF(xAxis.Dims.FigureSizePx, yAxis.Dims.FigureSizePx);
            //var dataSize = new SizeF(xAxis.Dims.DataSizePx, yAxis.Dims.DataSizePx);
            //var dataOffset = new PointF(xAxis.Dims.DataOffsetPx, yAxis.Dims.DataOffsetPx);

            // determine axis limits based on specific X and Y axes
            (double xMin, double xMax) = xAxis.Dims.RationalLimits();
            (double yMin, double yMax) = yAxis.Dims.RationalLimits();
            var limits = (xMin, xMax, yMin, yMax);

            return new PlotDimensions(figureSize, dataSize, dataOffset, limits);
        }

        public void Resize(int width, int height)
        {
            foreach (Axis axis in Axes)
                axis.Dims.Resize(axis.IsHorizontal ? width : height);
        }

        public void ResetAxes()
        {
            foreach (Axis axis in Axes)
                axis.Dims.ResetLimits();
        }

        public void AxisSet(double? xMin, double? xMax, double? yMin, double? yMax, int xAxisIndex, int yAxisIndex)
        {
            foreach (Axis axis in Axes)
            {
                if (axis.IsHorizontal && axis.AxisIndex == xAxisIndex)
                    axis.Dims.SetAxis(xMin, xMax);
                if (axis.IsVertical && axis.AxisIndex == yAxisIndex)
                    axis.Dims.SetAxis(yMin, yMax);
            }
        }

        public double[] AxisLimitsArray(int xAxisIndex, int yAxisIndex)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);
            return new double[] { xAxis.Dims.Min, xAxis.Dims.Max, yAxis.Dims.Min, yAxis.Dims.Max };
        }

        public void AxesPanPx(int dxPx, int dyPx)
        {
            foreach (Axis axis in Axes)
                axis.Dims.PanPx(axis.IsHorizontal ? dxPx : dyPx);
        }

        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            // TODO: equal axes
            foreach (Axis axis in Axes)
            {
                double deltaPx = axis.IsHorizontal ? xPx : yPx;
                double delta = deltaPx * axis.Dims.UnitsPerPx;
                double deltaFrac = delta / (Math.Abs(delta) + axis.Dims.Span);
                axis.Dims.Zoom(Math.Pow(10, deltaFrac));
            }
        }

        public void AxisAuto(Axis axis)
        {
            if (axis.IsHorizontal)
                AxisAutoX(axis.AxisIndex);
            else
                AxisAutoY(axis.AxisIndex);
        }

        public void AxisAutoX(int xAxisIndex, double margin = .1, bool expandOnly = false)
        {
            double min = double.NaN;
            double max = double.NaN;
            double zoomFrac = 1 - margin;

            var plottableLimits = Plottables.Where(x => x is IUsesAxes)
                                               .Select(x => (IUsesAxes)x)
                                               .Where(x => x.HorizontalAxisIndex == xAxisIndex)
                                               .Select(x => x.GetAxisLimits())
                                               .ToArray();

            foreach (var limits in plottableLimits)
            {
                (double xMin, double xMax, _, _) = limits;
                if (!double.IsNaN(xMin))
                    min = double.IsNaN(min) ? xMin : Math.Min(min, xMin);
                if (!double.IsNaN(xMax))
                    max = double.IsNaN(max) ? xMax : Math.Max(max, xMax);
            }

            if (double.IsNaN(min) && double.IsNaN(max))
                return;

            var xAxis = GetXAxis(xAxisIndex);
            xAxis.Dims.SetAxis(min, max);
            xAxis.Dims.Zoom(zoomFrac);
        }

        public void AxisAutoY(int yAxisIndex, double margin = .1)
        {
            double min = double.NaN;
            double max = double.NaN;
            double zoomFrac = 1 - margin;

            var plottableLimits = Plottables.Where(x => x is IUsesAxes)
                                               .Select(x => (IUsesAxes)x)
                                               .Where(x => x.VerticalAxisIndex == yAxisIndex)
                                               .Select(x => x.GetAxisLimits())
                                               .ToArray();

            foreach (var limits in plottableLimits)
            {
                (_, _, double yMin, double yMax) = limits;
                if (!double.IsNaN(yMin))
                    min = double.IsNaN(min) ? yMin : Math.Min(min, yMin);
                if (!double.IsNaN(yMax))
                    max = double.IsNaN(max) ? yMax : Math.Max(max, yMax);
            }

            if (double.IsNaN(min) && double.IsNaN(max))
                return;

            var yAxis = GetYAxis(yAxisIndex);
            yAxis.Dims.SetAxis(min, max);
            yAxis.Dims.Zoom(zoomFrac);
        }

        public void AxisAuto(
            double horizontalMargin = .1,
            double verticalMargin = .1,
            bool autoX = true,
            bool autoY = true
            )
        {
            // TODO: equal axis

            int[] xAxisIndexes = Axes.Where(x => x.IsHorizontal).Select(x => x.AxisIndex).Distinct().ToArray();
            int[] yAxisIndexes = Axes.Where(x => x.IsVertical).Select(x => x.AxisIndex).Distinct().ToArray();

            if (autoX)
                foreach (int i in xAxisIndexes)
                    AxisAutoX(i, horizontalMargin);

            if (autoY)
                foreach (int i in yAxisIndexes)
                    AxisAutoY(i, verticalMargin);
        }
    }
}
