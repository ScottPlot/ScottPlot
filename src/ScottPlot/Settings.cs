using ScottPlot.Drawing;
using ScottPlot.Plottable;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    /// <summary>
    /// This module holds state for figure dimensions, axis limits, plot contents, and styling options.
    /// A plot can be duplicated by copying the full state of this settings module.
    /// </summary>
    public class Settings
    {
        // plottables
        public readonly List<IPlottable> Plottables = new List<IPlottable>();
        public Color GetNextColor() => PlottablePalette.GetColor(Plottables.Count);

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
        public bool AxisEqualScale = false;

        // shortcuts to fixed axes indexes
        public Axis YAxis => Axes[0];
        public Axis YAxis2 => Axes[1];
        public Axis XAxis => Axes[2];
        public Axis XAxis2 => Axes[3];

        // public fields represent primary X and Y axes
        public int Width => (int)XAxis.Dims.FigureSizePx;
        public int Height => (int)YAxis.Dims.FigureSizePx;
        public float DataOffsetX => XAxis.Dims.DataOffsetPx;
        public float DataOffsetY => YAxis.Dims.DataOffsetPx;
        public float DataWidth => XAxis.Dims.DataSizePx;
        public float DataHeight => YAxis.Dims.DataSizePx;

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
        public void AxisSet(double? xMin, double? xMax, double? yMin, double? yMax, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            GetXAxis(xAxisIndex).Dims.SetAxis(xMin, xMax);
            GetYAxis(yAxisIndex).Dims.SetAxis(yMin, yMax);
        }

        /// <summary>
        /// Define axis limits for a particuar axis
        /// </summary>
        public void AxisSet(AxisLimits limits, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            GetXAxis(xAxisIndex).Dims.SetAxis(limits.XMin, limits.XMax);
            GetYAxis(yAxisIndex).Dims.SetAxis(limits.YMin, limits.YMax);
        }

        /// <summary>
        /// Return X and Y axis limits
        /// </summary>
        public AxisLimits AxisLimits(int xAxisIndex, int yAxisIndex)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);
            return new AxisLimits(xAxis.Dims.Min, xAxis.Dims.Max, yAxis.Dims.Min, yAxis.Dims.Max);
        }

        /// <summary>
        /// Pan all axes by the given pixel distance
        /// </summary>
        public void AxesPanPx(float dxPx, float dyPx)
        {
            List<int> modifiedXs = new List<int>();
            foreach (Axis axis in Axes.Where(x => x.IsHorizontal))
            {
                if (!modifiedXs.Contains(axis.AxisIndex))
                {
                    axis.Dims.PanPx(axis.IsHorizontal ? dxPx : dyPx);
                    modifiedXs.Add(axis.AxisIndex);
                }
            }

            List<int> modifiedYs = new List<int>();
            foreach (Axis axis in Axes.Where(x => x.IsVertical))
            {
                if (!modifiedYs.Contains(axis.AxisIndex))
                {
                    axis.Dims.PanPx(axis.IsHorizontal ? dxPx : dyPx);
                    modifiedYs.Add(axis.AxisIndex);
                }
            }
        }

        /// <summary>
        /// Zoom all axes by the given pixel distance
        /// </summary>
        public void AxesZoomPx(float xPx, float yPx, bool lockRatio = false)
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
        /// Zoom all axes by the given fraction
        /// </summary>
        public void AxesZoomTo(double xFrac, double yFrac, float xPixel, float yPixel)
        {
            foreach (Axis axis in Axes)
            {
                double frac = axis.IsHorizontal ? xFrac : yFrac;
                float centerPixel = axis.IsHorizontal ? xPixel : yPixel;
                double center = axis.Dims.GetUnit(centerPixel);
                axis.Dims.Zoom(frac, center);
            }
        }

        /// <summary>
        /// Automatically adjust X and Y axis limits of all axes to fit the data
        /// </summary>
        public void AxisAutoAll(double horizontalMargin = .1, double verticalMargin = .1)
        {
            AxisAutoAllX(horizontalMargin);
            AxisAutoAllY(verticalMargin);
        }

        /// <summary>
        /// Automatically adjust axis limits for all axes which have not yet been set
        /// </summary>
        public void AxisAutoUnsetAxes()
        {
            /* Extra logic here ensures axes index only get auto-set once 
             * in the case that multiple axes share the same axis index
             */

            var xAxes = Axes.Where(x => x.IsHorizontal);
            var yAxes = Axes.Where(x => x.IsVertical);

            var setIndexesX = xAxes.Where(x => x.Dims.HasBeenSet && !x.Dims.IsNan).Select(x => x.AxisIndex).Distinct().ToList();
            var setIndexesY = yAxes.Where(x => x.Dims.HasBeenSet && !x.Dims.IsNan).Select(x => x.AxisIndex).Distinct().ToList();

            foreach (Axis axis in xAxes)
            {
                if (axis.Dims.HasBeenSet && !axis.Dims.IsNan)
                    continue;
                if (setIndexesX.Contains(axis.AxisIndex))
                    continue;
                setIndexesX.Add(axis.AxisIndex);
                AxisAutoX(axis.AxisIndex);
            }

            foreach (Axis axis in yAxes)
            {
                if (axis.Dims.HasBeenSet && !axis.Dims.IsNan)
                    continue;
                if (setIndexesY.Contains(axis.AxisIndex))
                    continue;
                setIndexesY.Add(axis.AxisIndex);
                AxisAutoY(axis.AxisIndex);
            }
        }

        /// <summary>
        /// Ensure X and Y axes have the same scale (units per pixel) if AxisEqualScale is True
        /// </summary>
        public void EnforceEqualAxisScales()
        {
            if (AxisEqualScale == false)
                return;

            double unitsPerPixel = Math.Max(XAxis.Dims.UnitsPerPx, YAxis.Dims.UnitsPerPx);
            double xHalfSize = (XAxis.Dims.DataSizePx / 2) * unitsPerPixel;
            double yHalfSize = (YAxis.Dims.DataSizePx / 2) * unitsPerPixel;

            double xMin = XAxis.Dims.Center - xHalfSize;
            double xMax = XAxis.Dims.Center + xHalfSize;
            double yMin = YAxis.Dims.Center - yHalfSize;
            double yMax = YAxis.Dims.Center + yHalfSize;

            AxisSet(xMin, xMax, yMin, yMax);
        }

        /// <summary>
        /// Automatically adjust X axis limits to fit the data
        /// </summary>
        public void AxisAutoAllX(double margin = .1)
        {
            int[] xAxisIndexes = Axes.Where(x => x.IsHorizontal).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in xAxisIndexes)
                AxisAutoX(i, margin);
        }

        /// <summary>
        /// Automatically adjust Y axis limits to fit the data
        /// </summary>
        public void AxisAutoAllY(double margin = .1)
        {
            int[] yAxisIndexes = Axes.Where(x => x.IsVertical).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in yAxisIndexes)
                AxisAutoY(i, margin);
        }

        private void AxisAutoX(int xAxisIndex, double margin = .1)
        {
            double min = double.NaN;
            double max = double.NaN;
            double zoomFrac = 1 - margin;

            var plottableLimits = Plottables.Where(x => x is IPlottable)
                                               .Select(x => (IPlottable)x)
                                               .Where(x => x.IsVisible)
                                               .Where(x => x.XAxisIndex == xAxisIndex)
                                               .Select(x => x.GetAxisLimits())
                                               .ToArray();

            foreach (var limits in plottableLimits)
            {
                if (!double.IsNaN(limits.XMin))
                    min = double.IsNaN(min) ? limits.XMin : Math.Min(min, limits.XMin);
                if (!double.IsNaN(limits.XMax))
                    max = double.IsNaN(max) ? limits.XMax : Math.Max(max, limits.XMax);
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

            var plottableLimits = Plottables.Where(x => x is IPlottable)
                                               .Select(x => (IPlottable)x)
                                               .Where(x => x.IsVisible)
                                               .Where(x => x.YAxisIndex == yAxisIndex)
                                               .Select(x => x.GetAxisLimits())
                                               .ToArray();

            foreach (var limits in plottableLimits)
            {
                if (!double.IsNaN(limits.YMin))
                    min = double.IsNaN(min) ? limits.YMin : Math.Min(min, limits.YMin);
                if (!double.IsNaN(limits.YMax))
                    max = double.IsNaN(max) ? limits.YMax : Math.Max(max, limits.YMax);
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
            AxisAutoUnsetAxes();
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

        public float MouseDownX { get; private set; }
        public float MouseDownY { get; private set; }

        /// <summary>
        /// Remember mouse position (do this before calling MousePan or MouseZoom)
        /// </summary>
        public void MouseDown(float mouseDownX, float mouseDownY)
        {
            RememberAxisLimits();
            MouseDownX = mouseDownX;
            MouseDownY = mouseDownY;
        }

        public bool MouseHasMoved(float mouseNowX, float mouseNowY, float threshold = 2) =>
            Math.Abs(mouseNowX - MouseDownX) >= threshold &&
            Math.Abs(mouseNowY - MouseDownY) >= threshold;

        /// <summary>
        /// Pan all axes based on the mouse position now vs that last given to MouseDown()
        /// </summary>
        public void MousePan(float mouseNowX, float mouseNowY)
        {
            RecallAxisLimits();
            AxesPanPx(MouseDownX - mouseNowX, mouseNowY - MouseDownY);
        }

        /// <summary>
        /// Zoom all axes based on the mouse position now vs that last given to MouseDown()
        /// </summary>
        public void MouseZoom(float mouseNowX, float mouseNowY)
        {
            RecallAxisLimits();
            AxesZoomPx(mouseNowX - MouseDownX, MouseDownY - mouseNowY);
        }

        public void MouseZoomRect(float mouseNowX, float mouseNowY, bool finalize = false)
        {
            float left = Math.Min(MouseDownX, mouseNowX);
            float right = Math.Max(MouseDownX, mouseNowX);
            float top = Math.Min(MouseDownY, mouseNowY);
            float bottom = Math.Max(MouseDownY, mouseNowY);
            float width = right - left;
            float height = bottom - top;

            if (finalize)
            {
                double x1 = XAxis.Dims.GetUnit(left);
                double x2 = XAxis.Dims.GetUnit(right);
                double y1 = YAxis.Dims.GetUnit(bottom);
                double y2 = YAxis.Dims.GetUnit(top);
                ZoomRectangle.Clear();
                AxisSet(x1, x2, y1, y2);
            }
            else
            {
                // TODO: dont require data offset shifting prior to calling this
                ZoomRectangle.Set(left - DataOffsetX, top - DataOffsetY, width, height);
            }
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
            //
            // To solve this, start by assuming data area size == figure size and layout padding == 0,
            // then calculate ticks, then set padding based on the largest tick, then re-calculate ticks.

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
            RecalculateDataPadding();

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
            RecalculateDataPadding();
        }

        private void RecalculateDataPadding()
        {
            Edge[] edges = { Edge.Left, Edge.Right, Edge.Top, Edge.Bottom };
            foreach (var edge in edges)
            {
                float offset = 0;
                foreach (var axis in Axes.Where(x => x.Edge == edge))
                {
                    axis.SetOffset(offset);
                    offset += axis.GetSize();
                }
            }

            float padLeft = Axes.Where(x => x.Edge == Edge.Left).Select(x => x.GetSize()).Sum();
            float padRight = Axes.Where(x => x.Edge == Edge.Right).Select(x => x.GetSize()).Sum();
            float padBottom = Axes.Where(x => x.Edge == Edge.Bottom).Select(x => x.GetSize()).Sum();
            float padTop = Axes.Where(x => x.Edge == Edge.Top).Select(x => x.GetSize()).Sum();

            foreach (Axis axis in Axes)
            {
                if (axis.IsHorizontal)
                    axis.Dims.SetPadding(padLeft, padRight);
                else
                    axis.Dims.SetPadding(padTop, padBottom);
            }
        }
    }
}
