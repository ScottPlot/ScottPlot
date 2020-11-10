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
        public bool AxesHaveBeenSet = false;

        public int Width => (int)XAxis.Dims.FigureSizePx;
        public int Height => (int)YAxis.Dims.FigureSizePx;
        public float DataOffsetX => XAxis.Dims.DataOffsetPx;
        public float DataOffsetY => YAxis.Dims.DataOffsetPx;
        public float DataWidth => XAxis.Dims.DataSizePx;
        public float DataHeight => YAxis.Dims.DataSizePx;

        /// <summary>
        /// Adjust data padding based on axis size
        /// </summary>
        public void TightenLayout()
        {
            float padLeft = Axes.Where(x => x.Edge == Edge.Left).Select(x => x.PixelSize).Sum();
            float padRight = Axes.Where(x => x.Edge == Edge.Right).Select(x => x.PixelSize).Sum();
            float padBottom = Axes.Where(x => x.Edge == Edge.Bottom).Select(x => x.PixelSize).Sum();
            float padTop = Axes.Where(x => x.Edge == Edge.Top).Select(x => x.PixelSize).Sum();
            XAxis.Dims.SetPadding(padLeft, padRight);
            YAxis.Dims.SetPadding(padTop, padBottom);
        }

        // plottables
        public readonly List<IRenderable> Plottables = new List<IRenderable>();
        public Color GetNextColor() { return PlottablePalette.GetColor(Plottables.Count); }

        // settings the user can customize
        public readonly FigureBackground FigureBackground = new FigureBackground();
        public readonly DataBackground DataBackground = new DataBackground();
        public readonly BenchmarkMessage BenchmarkMessage = new BenchmarkMessage();
        public readonly ErrorMessage ErrorMessage = new ErrorMessage();
        public readonly Legend CornerLegend = new Legend();
        public readonly ZoomRectangle ZoomRectangle = new ZoomRectangle();
        public Palette PlottablePalette = Palette.Category10;

        public List<Axis> Axes = new List<Axis>() {
            new DefaultLeftAxis(),
            new DefaultRightAxis(),
            new DefaultBottomAxis(),
            new DefaultTopAxis()
        };

        public Axis YAxis => Axes[0];
        public Axis YAxis2 => Axes[1];
        public Axis XAxis => Axes[2];
        public Axis XAxis2 => Axes[3];
        public Axis[] PrimaryAxes => Axes.Take(4).ToArray();

        /*
         * ##################################################################################
         * # OLD SETTINGS WHICH I AM WORKING TO STRANGLE
         * 
         */

        public double GetPixelX(double locationX) => XAxis.Dims.GetPixel(locationX);
        public double GetPixelY(double locationY) => YAxis.Dims.GetPixel(locationY);
        public PointF GetPixel(double locationX, double locationY) => new PointF((float)GetPixelX(locationX), (float)GetPixelY(locationY));

        public double GetLocationX(double pixelX) => XAxis.Dims.GetUnit((float)pixelX);
        public double GetLocationY(double pixelY) => YAxis.Dims.GetUnit((float)pixelY);
        public PointF GetLocation(double pixelX, double pixelY) => new PointF((float)GetLocationX(pixelX), (float)GetLocationY(pixelY));

        public PlotDimensions GetPlotDimensions()
        {
            (double xMin, double xMax) = XAxis.Dims.RationalLimits();
            (double yMin, double yMax) = YAxis.Dims.RationalLimits();
            return new PlotDimensions(
                figureSize: new SizeF(XAxis.Dims.FigureSizePx, YAxis.Dims.FigureSizePx),
                dataSize: new SizeF(XAxis.Dims.DataSizePx, YAxis.Dims.DataSizePx),
                dataOffset: new PointF(XAxis.Dims.DataOffsetPx, YAxis.Dims.DataOffsetPx),
                axisLimits: new AxisLimits2D(xMin, xMax, yMin, yMax));
        }

        public void Resize(int width, int height)
        {
            XAxis.Dims.Resize(width);
            YAxis.Dims.Resize(height);
        }

        public void ResetAxes()
        {
            XAxis.Dims.ResetLimits();
            YAxis.Dims.ResetLimits();
        }

        public void AxisSet(double? xMin, double? xMax, double? yMin, double? yMax)
        {
            XAxis.Dims.SetAxis(xMin, xMax);
            YAxis.Dims.SetAxis(yMin, yMax);
        }

        public double[] AxisLimitsArray()
        {
            return new double[] { XAxis.Dims.Min, XAxis.Dims.Max, YAxis.Dims.Min, YAxis.Dims.Max };
        }

        public void AxesPanPx(int dxPx, int dyPx)
        {
            XAxis.Dims.PanPx(dxPx);
            YAxis.Dims.PanPx(dyPx);
        }

        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            double dX = xPx * XAxis.Dims.UnitsPerPx;
            double dY = yPx * YAxis.Dims.UnitsPerPx;
            double dXFrac = dX / (Math.Abs(dX) + XAxis.Dims.Span);
            double dYFrac = dY / (Math.Abs(dY) + YAxis.Dims.Span);

            // TODO: equal axes
            /*
            if (axes.equalAxes)
            {
                double zoomValue = dX + dY; // NE - max zoomIn, SW - max ZoomOut, NW and SE - 0 zoomValue
                double zoomFrac = zoomValue / (Math.Abs(zoomValue) + axes.x.span);
                dXFrac = zoomFrac;
                dYFrac = zoomFrac;
            }
            if (lockRatio)
            {
                double meanFrac = (dXFrac + dYFrac) / 2;
                dXFrac = meanFrac;
                dYFrac = meanFrac;
            }
            */

            XAxis.Dims.Zoom(Math.Pow(10, dXFrac));
            YAxis.Dims.Zoom(Math.Pow(10, dYFrac));
        }

        public void AxisAuto(
            double horizontalMargin = .1, double verticalMargin = .1,
            bool xExpandOnly = false, bool yExpandOnly = false,
            bool autoX = true, bool autoY = true
            )
        {
            var oldLimits = new AxisLimits2D(XAxis.Dims.Min, XAxis.Dims.Max, YAxis.Dims.Min, YAxis.Dims.Max);
            var newLimits = new AxisLimits2D();

            foreach (var plottable in Plottables)
            {
                if (plottable is IUsesAxes plottableWithLimits)
                {
                    var (xMin, xMax, yMin, yMax) = plottableWithLimits.GetAxisLimits();
                    if (autoX)
                        newLimits.Expand(xMin, xMax, null, null);
                    if (autoY)
                        newLimits.Expand(null, null, yMin, yMax);
                }
            }

            // TODO: equal axis
            /*
            if (axes.equalAxes)
            {
                var xUnitsPerPixel = newLimits.xSpan / (DataWidth * (1 - horizontalMargin));
                var yUnitsPerPixel = newLimits.ySpan / (DataHeight * (1 - verticalMargin));
                axes.Set(newLimits);
                if (yUnitsPerPixel > xUnitsPerPixel)
                    axes.Zoom((1 - horizontalMargin) * xUnitsPerPixel / yUnitsPerPixel, 1 - verticalMargin);
                else
                    axes.Zoom(1 - horizontalMargin, (1 - verticalMargin) * yUnitsPerPixel / xUnitsPerPixel);
                return;
            }
            */

            if (xExpandOnly)
            {
                XAxis.Dims.SetAxis(newLimits.XMin, newLimits.XMax);
                YAxis.Dims.SetAxis(oldLimits.YMin, oldLimits.YMax);
            }
            else if (yExpandOnly)
            {
                XAxis.Dims.SetAxis(oldLimits.XMin, oldLimits.XMax);
                YAxis.Dims.SetAxis(newLimits.YMin, newLimits.YMax);
            }
            else
            {
                XAxis.Dims.SetAxis(newLimits.XMin, newLimits.XMax);
                YAxis.Dims.SetAxis(newLimits.YMin, newLimits.YMax);
            }

            double zoomFracX = yExpandOnly ? 1 : 1 - horizontalMargin;
            double zoomFracY = xExpandOnly ? 1 : 1 - verticalMargin;

            XAxis.Dims.Zoom(zoomFracX);
            YAxis.Dims.Zoom(zoomFracY);

            AxesHaveBeenSet = Plottables.Count > 0;
        }
    }
}
