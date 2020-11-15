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
        public bool AllAxesHaveBeenSet => Axes.All(x => x.Dims.HasBeenSet);

        // shortcuts to fixed axes indexes
        public Axis YAxis => Axes[0];
        public Axis YAxis2 => Axes[1];
        public Axis XAxis => Axes[2];
        public Axis XAxis2 => Axes[3];
        public Axis[] PrimaryAxes => Axes.Take(4).ToArray();

        // public fields represent primary X and Y axes
        public int Width => (int)XAxis.Dims.FigureSizePx;
        public int Height => (int)YAxis.Dims.FigureSizePx;
        public float DataOffsetX => XAxis.Dims.DataOffsetPx;
        public float DataOffsetY => YAxis.Dims.DataOffsetPx;
        public float DataWidth => XAxis.Dims.DataSizePx;
        public float DataHeight => YAxis.Dims.DataSizePx;

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

        /// <summary>
        /// Return figure dimensions for the specified X and Y axes
        /// </summary>
        public PlotDimensions GetPlotDimensions(int xAxisIndex, int yAxisIndex)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);

            // determine figure dimensions based on primary X and Y axis
            var figureSize = new SizeF(XAxis.Dims.FigureSizePx, YAxis.Dims.FigureSizePx);
            var dataSize = new SizeF(XAxis.Dims.DataSizePx, YAxis.Dims.DataSizePx);
            var dataOffset = new PointF(XAxis.Dims.DataOffsetPx, YAxis.Dims.DataOffsetPx);

            // determine axis limits based on specific X and Y axes
            (double xMin, double xMax) = xAxis.Dims.RationalLimits();
            (double yMin, double yMax) = yAxis.Dims.RationalLimits();
            var limits = (xMin, xMax, yMin, yMax);

            return new PlotDimensions(figureSize, dataSize, dataOffset, limits);
        }

        /// <summary>
        /// Set the default size for rendering images
        /// </summary>
        public void Resize(float width, float height)
        {
            foreach (Axis axis in Axes)
                axis.Dims.Resize(axis.IsHorizontal ? width : height);
        }

        /// <summary>
        /// Reset axis limits to their defauts
        /// </summary>
        public void ResetAxisLimits()
        {
            foreach (Axis axis in Axes)
                axis.Dims.ResetLimits();
        }

        /// <summary>
        /// Define axis limits for a particuar axis
        /// </summary>
        public void AxisSet(double? xMin, double? xMax, double? yMin, double? yMax, int xAxisIndex, int yAxisIndex)
        {
            GetXAxis(xAxisIndex).Dims.SetAxis(xMin, xMax);
            GetYAxis(yAxisIndex).Dims.SetAxis(yMin, yMax);
        }

        /// <summary>
        /// Return X and Y axis limits
        /// </summary>
        public double[] AxisLimitsArray(int xAxisIndex, int yAxisIndex)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);
            return new double[] { xAxis.Dims.Min, xAxis.Dims.Max, yAxis.Dims.Min, yAxis.Dims.Max };
        }

        /// <summary>
        /// Pan all axes by the given pixel distance
        /// </summary>
        public void AxesPanPx(int dxPx, int dyPx)
        {
            foreach (Axis axis in Axes)
                axis.Dims.PanPx(axis.IsHorizontal ? dxPx : dyPx);
        }

        /// <summary>
        /// Zoom all axes by the given pixel distance
        /// </summary>
        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            if (lockRatio)
                (xPx, yPx) = (Math.Max(xPx, yPx), Math.Max(xPx, yPx));

            foreach (Axis axis in Axes)
            {
                double deltaPx = axis.IsHorizontal ? xPx : yPx;
                double delta = deltaPx * axis.Dims.UnitsPerPx;
                double deltaFrac = delta / (Math.Abs(delta) + axis.Dims.Span);
                axis.Dims.Zoom(Math.Pow(10, deltaFrac));
            }
        }

        /// <summary>
        /// Automatically adjust X and Y axis limits to fit the data
        /// </summary>
        public void AxisAuto(double horizontalMargin = .1, double verticalMargin = .1, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            AxisAutoX(horizontalMargin, xAxisIndex);
            AxisAutoY(verticalMargin, yAxisIndex);
        }

        /// <summary>
        /// Automatically adjust axis limits for all axes which have not yet been set
        /// </summary>
        public void AxisAutoUnsetAxes()
        {
            foreach (Axis axis in Axes.Where(x => x.Dims.HasBeenSet == false))
            {
                if (axis.IsHorizontal)
                    AxisAutoX(axis.AxisIndex);
                else
                    AxisAutoY(axis.AxisIndex);
            }
        }

        /// <summary>
        /// Automatically adjust X axis limits to fit the data
        /// </summary>
        public void AxisAutoX(double margin = .1, int xAxisIndex = 0)
        {
            int[] xAxisIndexes = Axes.Where(x => x.IsHorizontal).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in xAxisIndexes)
                AxisAutoX(i, margin);
        }

        /// <summary>
        /// Automatically adjust Y axis limits to fit the data
        /// </summary>
        public void AxisAutoY(double margin = .1, int yAxisIndex = 0)
        {
            int[] yAxisIndexes = Axes.Where(x => x.IsVertical).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in yAxisIndexes)
                AxisAutoY(i, margin);
        }

        private void AxisAutoX(int xAxisIndex, double margin = .1, bool expandOnly = false)
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

        private void AxisAutoY(int yAxisIndex, double margin = .1)
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

        /// <summary>
        /// Store axis limits (useful for storing state upon a MouseDown event)
        /// </summary>
        public void RememberAxisLimits()
        {
            foreach (Axis axis in Axes)
                axis.Dims.Remember();
        }

        /// <summary>
        /// Recall axis limits (useful for recalling state from a previous MouseDown event)
        /// </summary>
        public void RecallAxisLimits()
        {
            foreach (Axis axis in Axes)
                axis.Dims.Recall();
        }

        /// <summary>
        /// Ensure all axes have the same size and offset as the primary X and Y axis
        /// </summary>
        public void CopyPrimaryLayoutToAllAxes()
        {
            foreach (Axis axis in Axes)
            {
                if (axis.IsHorizontal)
                    axis.Dims.Resize(Width, DataWidth, DataOffsetX);
                else
                    axis.Dims.Resize(Height, DataHeight, DataOffsetY);
            }
        }

        public void LayoutAuto()
        {
            foreach (int xAxisIndex in XAxisIndexes)
                LayoutAuto(xAxisIndex, 0);
            foreach (int yAxisIndex in YAxisIndexes)
                LayoutAuto(0, yAxisIndex);
        }

        private void LayoutAuto(int xAxisIndex, int yAxisIndex)
        {
            // TODO: separate this into distinct X and Y functions (requires refactoring plottable interface)
            bool atLeastOneAxisIsZero = xAxisIndex == 0 || yAxisIndex == 0;
            if (!atLeastOneAxisIsZero)
                throw new InvalidOperationException();

            // Adjust padding around the data area to accommodate title and tick labels.
            //
            // This is a chicken-and-egg problem:
            //   * TICK DENSITY depends on the DATA AREA SIZE
            //   * DATA AREA SIZE depends on LAYOUT PADDING
            //   * LAYOUT PADDING depends on MAXIMUM LABEL SIZE
            //   * MAXIMUM LABEL SIZE depends on TICK DENSITY
            // To solve this, start by assuming data area size == figure size, and layout padding == 0

            // axis limits shall not change
            var dims = GetPlotDimensions(xAxisIndex, yAxisIndex);
            var limits = (dims.XMin, dims.XMax, dims.YMin, dims.YMax);
            var figSize = new SizeF(Width, Height);

            // first-pass tick calculation based on full image size 
            var dimsFull = new PlotDimensions(figSize, figSize, new PointF(0, 0), limits);

            foreach (var axis in Axes)
            {
                bool isMatchingXAxis = axis.IsHorizontal && axis.AxisIndex == xAxisIndex;
                bool isMatchingYAxis = axis.IsVertical && axis.AxisIndex == yAxisIndex;
                if (isMatchingXAxis || isMatchingYAxis)
                {
                    axis.RecalculateTickPositions(dimsFull);
                    axis.RecalculateAxisSize();
                }
            }

            // now adjust our layout based on measured axis sizes
            TightenLayout();

            // now recalculate ticks based on new layout
            var dataSize = new SizeF(DataWidth, DataHeight);
            var dataOffset = new PointF(DataOffsetX, DataOffsetY);

            var dims3 = new PlotDimensions(figSize, dataSize, dataOffset, limits);
            foreach (var axis in Axes)
            {
                bool isMatchingXAxis = axis.IsHorizontal && axis.AxisIndex == xAxisIndex;
                bool isMatchingYAxis = axis.IsVertical && axis.AxisIndex == yAxisIndex;
                if (isMatchingXAxis || isMatchingYAxis)
                {
                    axis.RecalculateTickPositions(dims3);
                }
            }

            // adjust the layout based on measured tick label sizes
            TightenLayout();
        }
    }
}
