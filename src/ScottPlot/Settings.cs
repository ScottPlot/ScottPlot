using ScottPlot.Renderable;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;
using System.Globalization;

namespace ScottPlot
{
    /// <summary>
    /// This module holds state for figure dimensions, axis limits, plot contents, and styling options.
    /// A plot can be duplicated by copying the full stae of this settings module.
    /// </summary>
    public class Settings
    {
        public readonly PlotDimensions Dims = new PlotDimensions();
        public bool AxesHaveBeenSet = false;

        public int Width => (int)Dims.Width;
        public int Height => (int)Dims.Height;
        public float DataOffsetX => Dims.DataOffsetX;
        public float DataOffsetY => Dims.DataOffsetY;
        public float DataWidth => Dims.DataWidth;
        public float DataHeight => Dims.DataHeight;

        /// <summary>
        /// Adjust data padding based on axis size
        /// </summary>
        public void TightenLayout()
        {
            float padLeft = Axes.Where(x => x.Edge == Edge.Left).Select(x => x.PixelSize).Sum();
            float padRight = Axes.Where(x => x.Edge == Edge.Right).Select(x => x.PixelSize).Sum();
            float padBottom = Axes.Where(x => x.Edge == Edge.Bottom).Select(x => x.PixelSize).Sum();
            float padTop = Axes.Where(x => x.Edge == Edge.Top).Select(x => x.PixelSize).Sum();
            Dims.ResizeDataWithPadding(padLeft, padRight, padBottom, padTop);
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

        public double GetPixelX(double locationX) => Dims.GetPixelX(locationX);
        public double GetPixelY(double locationY) => Dims.GetPixelY(locationY);
        public PointF GetPixel(double locationX, double locationY) => new PointF((float)GetPixelX(locationX), (float)GetPixelY(locationY));

        public double GetLocationX(double pixelX) => Dims.GetCoordinateX((float)pixelX);
        public double GetLocationY(double pixelY) => Dims.GetCoordinateY((float)pixelY);
        public PointF GetLocation(double pixelX, double pixelY) => new PointF((float)GetLocationX(pixelX), (float)GetLocationY(pixelY));

        public void Resize(int width, int height) => Dims.Resize(width, height);

        public void AxesPanPx(int dxPx, int dyPx) => Dims.PanPx(dxPx, dyPx);

        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            double dX = xPx * Dims.UnitsPerPxX;
            double dY = yPx * Dims.UnitsPerPxY;
            double dXFrac = dX / (Math.Abs(dX) + Dims.XSpan);
            double dYFrac = dY / (Math.Abs(dY) + Dims.YSpan);

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

            Dims.Zoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void AxisAuto(
            double horizontalMargin = .1, double verticalMargin = .1,
            bool xExpandOnly = false, bool yExpandOnly = false,
            bool autoX = true, bool autoY = true
            )
        {
            var oldLimits = new AxisLimits2D(Dims.XMin, Dims.XMax, Dims.YMin, Dims.YMax);
            var newLimits = new AxisLimits2D();

            foreach (var plottable in Plottables)
            {
                if (plottable is IHasAxisLimits plottableWithLimits)
                {
                    var plottableLimits = plottableWithLimits.GetLimits();
                    if (autoX)
                        newLimits.Expand(plottableLimits.XMin, plottableLimits.XMax, null, null);
                    if (autoY)
                        newLimits.Expand(null, null, plottableLimits.YMin, plottableLimits.YMax);
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
                Dims.SetAxis(newLimits.XMin, newLimits.XMax, oldLimits.YMin, oldLimits.YMax);
            else if (yExpandOnly)
                Dims.SetAxis(oldLimits.XMin, oldLimits.XMax, newLimits.YMin, newLimits.YMax);
            else
                Dims.SetAxis(newLimits.XMin, newLimits.XMax, newLimits.YMin, newLimits.YMax);

            double zoomFracX = yExpandOnly ? 1 : 1 - horizontalMargin;
            double zoomFracY = xExpandOnly ? 1 : 1 - verticalMargin;
            Dims.Zoom(zoomFracX, zoomFracY);

            AxesHaveBeenSet = Plottables.Count > 0;
        }
    }
}
