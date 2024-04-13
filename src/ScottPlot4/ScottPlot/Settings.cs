using ScottPlot.Drawing;
using ScottPlot.Plottable;
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        /// <summary>
        /// This List contains all plottables managed by this Plot.
        /// Render order is from lowest (first) to highest (last).
        /// </summary>
        public readonly ObservableCollection<IPlottable> Plottables = new();

        /// <summary>
        /// Unique value that changes any time the list of plottables is modified.
        /// </summary>
        public int PlottablesIdentifier { get; private set; } = 0;

        /// <summary>
        /// Return the next color from PlottablePalette based on the current number of plottables
        /// </summary>
        public Color GetNextColor() => PlottablePalette.GetColor(Plottables.Count);

        public readonly FigureBackground FigureBackground = new FigureBackground();
        public readonly DataBackground DataBackground = new DataBackground();
        public readonly BenchmarkMessage BenchmarkMessage = new BenchmarkMessage();
        public readonly ErrorMessage ErrorMessage = new ErrorMessage();
        public readonly Legend CornerLegend = new Legend();
        public readonly ZoomRectangle ZoomRectangle = new ZoomRectangle();
        public IPalette PlottablePalette = new Palettes.Category10();

        /// <summary>
        /// List of all axes used in this plot.
        /// Axes can be added, but existing ones should not be removed.
        /// </summary>
        public List<Axis> Axes = new()
        {
            new DefaultLeftAxis(),
            new DefaultRightAxis(),
            new DefaultBottomAxis(),
            new DefaultTopAxis()
        };

        /// <summary>
        /// Get an array containing just horizontal axes
        /// </summary>
        public Axis[] HorizontalAxes => Axes.Where(x => x.IsHorizontal).Distinct().ToArray();

        /// <summary>
        /// Get an array containing just vertical axes
        /// </summary>
        public Axis[] VerticalAxes => Axes.Where(x => x.IsVertical).Distinct().ToArray();

        /// <summary>
        /// Return the first horizontal axis with the given axis index
        /// </summary>
        public Axis GetXAxis(int xAxisIndex)
        {
            Axis[] axes = Axes.Where(x => x.IsHorizontal && x.AxisIndex == xAxisIndex).ToArray();
            if (axes.Length == 0)
                throw new InvalidOperationException($"There no X axes with an axis index of {xAxisIndex}");
            return axes[0];
        }

        /// <summary>
        /// Return the first vertical axis with the given axis index
        /// </summary>
        public Axis GetYAxis(int yAxisIndex)
        {
            Axis[] axes = Axes.Where(x => x.IsVertical && x.AxisIndex == yAxisIndex).ToArray();
            if (axes.Length == 0)
                throw new InvalidOperationException($"There no Y axes with an axis index of {yAxisIndex}");
            return axes[0];
        }

        /// <summary>
        /// Indicates whether unset axes are present.
        /// If true, the user may want to call AxisAuto() or SetAxisLimits().
        /// </summary>
        public bool AllAxesHaveBeenSet => Axes.All(x => x.Dims.HasBeenSet);

        /// <summary>
        /// Controls relationship between X and Y axis scales.
        /// See documentation for enumeration members.
        /// </summary>
        public EqualScaleMode EqualScaleMode { get; set; } = EqualScaleMode.Disabled;

        /// <summary>
        /// Primary vertical axis (on the left of the plot)
        /// </summary>
        public Axis YAxis => Axes[0];

        /// <summary>
        /// Secondary vertical axis (on the right of the plot)
        /// </summary>
        public Axis YAxis2 => Axes[1];

        /// <summary>
        /// Primary horizontal axis (on the bottom of the plot)
        /// </summary>
        public Axis XAxis => Axes[2];

        /// <summary>
        /// Secondary horizontal axis (on the top of the plot)
        /// </summary>
        public Axis XAxis2 => Axes[3];

        /// <summary>
        /// Width of the figure (in pixels)
        /// </summary>
        public int Width => (int)XAxis.Dims.FigureSizePx;

        /// <summary>
        /// Height of the figure (in pixels)
        /// </summary>
        public int Height => (int)YAxis.Dims.FigureSizePx;

        /// <summary>
        /// Default padding to use when AxisAuto() or Margins() is called without a specified margin
        /// </summary>
        public double MarginsX = .05;

        /// <summary>
        /// Default padding to use when AxisAuto() or Margins() is called without a specified margin
        /// </summary>
        public double MarginsY = .1;

        /// <summary>
        /// Controls whether OferflowException is ignored in the Render() method.
        /// This exception is commonly thrown by System.Drawing when drawing to extremely large pixel locations.
        /// </summary>
        public bool IgnoreOverflowExceptionsDuringRender = true;

        /// <summary>
        /// Determines whether the grid lines should be drawn above the plottables.
        /// </summary>
        public bool DrawGridAbovePlottables { get; set; } = false;

        /// <summary>
        /// If defined, the data area will use this rectangle and not be adjusted
        /// depending on axis labels or ticks.
        /// </summary>
        public PixelPadding? ManualDataPadding { get; set; } = null;

        public Settings()
        {
            Plottables.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs e) => PlottablesIdentifier++;
        }

        /// <summary>
        /// Return figure dimensions for the specified X and Y axes
        /// </summary>
        public PlotDimensions GetPlotDimensions(int xAxisIndex, int yAxisIndex, double scaleFactor)
        {
            var xAxis = GetXAxis(xAxisIndex);
            var yAxis = GetYAxis(yAxisIndex);

            // determine figure dimensions based on primary X and Y axis
            var figureSize = new SizeF(XAxis.Dims.FigureSizePx, YAxis.Dims.FigureSizePx);
            var dataSize = new SizeF(XAxis.Dims.DataSizePx, YAxis.Dims.DataSizePx);
            var dataOffset = new PointF(XAxis.Dims.DataOffsetPx, YAxis.Dims.DataOffsetPx);

            // manual override if manual padding is enabled
            if (ManualDataPadding is not null)
            {
                dataOffset = new PointF(
                    x: ManualDataPadding.Value.Left,
                    y: ManualDataPadding.Value.Top);

                dataSize = new SizeF(
                    width: figureSize.Width - ManualDataPadding.Value.Left - ManualDataPadding.Value.Right,
                    height: figureSize.Height - ManualDataPadding.Value.Top - ManualDataPadding.Value.Bottom);
            }

            // determine axis limits based on specific X and Y axes
            (double xMin, double xMax) = xAxis.Dims.RationalLimits();
            (double yMin, double yMax) = yAxis.Dims.RationalLimits();
            AxisLimits limits = new(xMin, xMax, yMin, yMax);

            return new PlotDimensions(figureSize, dataSize, dataOffset, limits, scaleFactor, xAxis.IsReverse, yAxis.IsReverse);
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
            foreach (Axis axis in Axes)
            {
                if (axis.IsHorizontal)
                {
                    axis.Dims.PanPx(dxPx);
                }
                else
                {
                    axis.Dims.PanPx(dyPx);
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
        /// Zoom all axes by the given pixel distance
        /// </summary>
        public void AxesZoomPxTo(float xPx, float yPx, float xPixel, float yPixel)
        {
            foreach (Axis axis in Axes)
            {
                double deltaPx = axis.IsHorizontal ? xPx : yPx;
                double delta = deltaPx * axis.Dims.UnitsPerPx;
                double deltaFrac = delta / (Math.Abs(delta) + axis.Dims.Span);
                double frac = Math.Pow(10, deltaFrac);
                float centerPixel = axis.IsHorizontal ? xPixel : yPixel;
                double center = axis.Dims.GetUnit(centerPixel);
                axis.Dims.Zoom(frac, center);
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
        public void AxisAutoAll(double? horizontalMargin = null, double? verticalMargin = null)
        {
            AxisAutoAllX(horizontalMargin);
            AxisAutoAllY(verticalMargin);
        }

        /// <summary>
        /// Automatically adjust axis limits for all axes which have not yet been set
        /// </summary>
        public void AxisAutoUnsetAxes(double? horizontalMargin = null, double? verticalMargin = null)
        {
            Axis[] unsetAxesX = HorizontalAxes
                .Where(x => !x.Dims.HasBeenSet && x.Dims.IsNan)
                .Select(x => x.AxisIndex)
                .Distinct()
                .Select(x => GetXAxis(x))
                .ToArray();

            Axis[] unsetAxesY = VerticalAxes
                .Where(x => !x.Dims.HasBeenSet && x.Dims.IsNan)
                .Select(x => x.AxisIndex)
                .Distinct()
                .Select(x => GetYAxis(x))
                .ToArray();

            foreach (Axis xAxis in unsetAxesX)
            {
                AxisAutoX(xAxis.AxisIndex, horizontalMargin);
            }

            foreach (Axis yAxis in unsetAxesY)
            {
                AxisAutoY(yAxis.AxisIndex, verticalMargin);
            }
        }

        /// <summary>
        /// If a scale lock mode is in use, modify the axis limits accordingly
        /// </summary>
        public void EnforceEqualAxisScales()
        {
            EqualScaleMode mode = EqualScaleMode;

            if (mode is EqualScaleMode.PreserveLargest)
            {
                mode = XAxis.Dims.DataSizePx > YAxis.Dims.DataSizePx
                    ? EqualScaleMode.PreserveX
                    : EqualScaleMode.PreserveY;
            }

            if (mode is EqualScaleMode.PreserveSmallest)
            {
                mode = XAxis.Dims.DataSizePx < YAxis.Dims.DataSizePx
                    ? EqualScaleMode.PreserveX
                    : EqualScaleMode.PreserveY;
            }

            switch (mode)
            {
                case EqualScaleMode.Disabled:
                    return;

                case EqualScaleMode.PreserveX:
                    double yHalfSize = (YAxis.Dims.DataSizePx / 2) * XAxis.Dims.UnitsPerPx;
                    AxisSet(null, null, YAxis.Dims.Center - yHalfSize, YAxis.Dims.Center + yHalfSize);
                    return;

                case EqualScaleMode.PreserveY:
                    double xHalfSize = (XAxis.Dims.DataSizePx / 2) * YAxis.Dims.UnitsPerPx;
                    AxisSet(XAxis.Dims.Center - xHalfSize, XAxis.Dims.Center + xHalfSize, null, null);
                    return;

                case EqualScaleMode.ZoomOut:
                    double maxUnitsPerPx = Math.Max(XAxis.Dims.UnitsPerPx, YAxis.Dims.UnitsPerPx);
                    double halfX = (XAxis.Dims.DataSizePx / 2) * maxUnitsPerPx;
                    double halfY = (YAxis.Dims.DataSizePx / 2) * maxUnitsPerPx;
                    AxisSet(XAxis.Dims.Center - halfX, XAxis.Dims.Center + halfX, YAxis.Dims.Center - halfY, YAxis.Dims.Center + halfY);
                    return;

                case EqualScaleMode.PreserveLargest:
                    throw new InvalidOperationException("this mode should have been converted to preserve X or Y");

                case EqualScaleMode.PreserveSmallest:
                    throw new InvalidOperationException("this mode should have been converted to preserve X or Y");

                default:
                    throw new InvalidOperationException("unknown scale lock mode");
            }
        }

        /// <summary>
        /// Automatically adjust X axis limits to fit the data
        /// </summary>
        public void AxisAutoAllX(double? margin = null)
        {
            int[] xAxisIndexes = Axes.Where(x => x.IsHorizontal).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in xAxisIndexes)
                AxisAutoX(i, margin);
        }

        /// <summary>
        /// Automatically adjust Y axis limits to fit the data
        /// </summary>
        public void AxisAutoAllY(double? margin = null)
        {
            int[] yAxisIndexes = Axes.Where(x => x.IsVertical).Select(x => x.AxisIndex).Distinct().ToArray();
            foreach (int i in yAxisIndexes)
                AxisAutoY(i, margin);
        }

        public void AxisAutoX(int xAxisIndex, double? margin = null)
        {
            if (margin < 0 || margin >= 1)
                throw new ArgumentException("Margins must be greater than 0 and less than 1");

            if (margin.HasValue)
                MarginsX = margin.Value;

            var plottableLimits = Plottables.Where(x => x.IsVisible)
                                            .Where(x => x.XAxisIndex == xAxisIndex)
                                            .Select(x => x.GetAxisLimits())
                                            .ToArray();

            double min = double.NaN;
            double max = double.NaN;
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

            double zoomFrac = 1 - MarginsX;
            xAxis.Dims.Zoom(zoomFrac);
        }

        public void AxisAutoY(int yAxisIndex, double? margin = null)
        {
            if (margin < 0 || margin >= 1)
                throw new ArgumentException("Margins must be greater than 0 and less than 1");

            if (margin.HasValue)
                MarginsY = margin.Value;

            var plottableLimits = Plottables.Where(x => x.IsVisible)
                                            .Where(x => x.YAxisIndex == yAxisIndex)
                                            .Select(x => x.GetAxisLimits())
                                            .ToArray();

            double min = double.NaN;
            double max = double.NaN;
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

            double zoomFrac = 1 - MarginsY;
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

        /// <summary>
        /// Pan all axes based on the mouse position now vs that last given to MouseDown()
        /// </summary>
        public void MousePan(float mouseNowX, float mouseNowY)
        {
            RecallAxisLimits();
            AxesPanPx(MouseDownX - mouseNowX, MouseDownY - mouseNowY);
        }

        /// <summary>
        /// Zoom all axes based on the mouse position now vs that last given to MouseDown()
        /// Relative to the center of the plot
        /// </summary>
        public void MouseZoomCenter(float mouseNowX, float mouseNowY)
        {
            RecallAxisLimits();
            AxesZoomPx(mouseNowX - MouseDownX, MouseDownY - mouseNowY);
        }

        /// <summary>
        /// Zoom all axes based on the mouse position now vs that last given to MouseDown()
        /// Relative to the location of the mouse when it was first pressed
        /// </summary>
        public void MouseZoomFromMouseDown(float mouseNowX, float mouseNowY)
        {
            RecallAxisLimits();
            AxesZoomPxTo(mouseNowX - MouseDownX, MouseDownY - mouseNowY, MouseDownX, MouseDownY);
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
                ZoomRectangle.Clear();
                foreach (Axis axis in Axes)
                {
                    if (axis.IsHorizontal)
                    {
                        double x1 = axis.Dims.GetUnit(left);
                        double x2 = axis.Dims.GetUnit(right);
                        axis.Dims.SetAxis(x1, x2);
                    }
                    else
                    {
                        double y1 = axis.Dims.GetUnit(bottom);
                        double y2 = axis.Dims.GetUnit(top);
                        axis.Dims.SetAxis(y1, y2);
                    }
                }
            }
            else
            {
                // TODO: dont require data offset shifting prior to calling this
                ZoomRectangle.Set(left - XAxis.Dims.DataOffsetPx, top - YAxis.Dims.DataOffsetPx, width, height);
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
                    axis.Dims.Resize(Width, XAxis.Dims.DataSizePx, XAxis.Dims.DataOffsetPx);
                else
                    axis.Dims.Resize(Height, YAxis.Dims.DataSizePx, YAxis.Dims.DataOffsetPx);
            }
        }

        /// <summary>
        /// Automatically adjust the layout for every axis
        /// </summary>
        public void LayoutAuto()
        {
            int[] xIndexes = HorizontalAxes.Select(x => x.AxisIndex).Distinct().ToArray();
            int[] yIndexes = VerticalAxes.Select(x => x.AxisIndex).Distinct().ToArray();

            foreach (int xAxisIndex in xIndexes)
            {
                LayoutAuto(xAxisIndex, 0);
            }

            foreach (int yAxisIndex in yIndexes)
            {
                LayoutAuto(0, yAxisIndex);
            }
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
            var dims = GetPlotDimensions(xAxisIndex, yAxisIndex, scaleFactor: 1.0);
            var figSize = new SizeF(Width, Height);

            // first-pass tick calculation based on full image size 
            var dimsFull = new PlotDimensions(figSize, figSize, new PointF(0, 0), dims.AxisLimits, scaleFactor: 1);

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
            var dataSize = new SizeF(XAxis.Dims.DataSizePx, YAxis.Dims.DataSizePx);
            var dataOffset = new PointF(XAxis.Dims.DataOffsetPx, YAxis.Dims.DataOffsetPx);

            var dims3 = new PlotDimensions(figSize, dataSize, dataOffset, dims.AxisLimits, scaleFactor: 1.0);
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

            float padLeft, padRight, padBottom, padTop;

            if (ManualDataPadding is null)
            {
                padLeft = Axes.Where(x => x.Edge == Edge.Left).Select(x => x.GetSize()).Sum();
                padRight = Axes.Where(x => x.Edge == Edge.Right).Select(x => x.GetSize()).Sum();
                padBottom = Axes.Where(x => x.Edge == Edge.Bottom).Select(x => x.GetSize()).Sum();
                padTop = Axes.Where(x => x.Edge == Edge.Top).Select(x => x.GetSize()).Sum();
            }
            else
            {
                padLeft = ManualDataPadding.Value.Left;
                padRight = ManualDataPadding.Value.Right;
                padBottom = ManualDataPadding.Value.Bottom;
                padTop = ManualDataPadding.Value.Top;
            }

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
