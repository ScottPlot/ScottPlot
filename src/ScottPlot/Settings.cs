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
        // TODO: perhaps make this an object with error checking for bad state
        public readonly PlotDimensions Dims = new PlotDimensions();
        public bool AxesHaveBeenSet = false;
        public double[] AxisLimits => new double[] { Dims.XMin, Dims.XMax, Dims.YMin, Dims.YMax };

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

        public PlotDimensions GetDimensions() => Dims;

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

        // TODO: move this functionality into the PlotDimensions module
        public double xAxisScale => Dims.PxPerUnitX;
        public double yAxisScale => Dims.PxPerUnitY;
        public double xAxisUnitsPerPixel => Dims.UnitsPerPxX;
        public double yAxisUnitsPerPixel => Dims.UnitsPerPxY;

        public void Resize(int width, int height) => Dims.Resize(width, height);

        public void AxesPanPx(int dxPx, int dyPx) => Dims.PanPx(dxPx, dyPx);

        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
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
            var oldLimits = new Config.AxisLimits2D(Dims.XMin, Dims.XMax, Dims.YMin, Dims.YMax);
            var newLimits = new Config.AxisLimits2D();

            foreach (var plottable in Plottables)
            {
                if (plottable is IHasAxisLimits plottableWithLimits)
                {
                    var plottableLimits = plottableWithLimits.GetLimits();
                    if (autoX && !yExpandOnly)
                        newLimits.ExpandX(plottableLimits);
                    if (autoY && !xExpandOnly)
                        newLimits.ExpandY(plottableLimits);
                }
            }

            newLimits.MakeRational();

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
                oldLimits.ExpandX(newLimits);
                Dims.SetAxis(oldLimits.x1, oldLimits.x2, null, null);
                Dims.ZoomX(1 - horizontalMargin);
                AxesHaveBeenSet = Plottables.Count > 0;
            }
            else if (yExpandOnly)
            {
                oldLimits.ExpandY(newLimits);
                Dims.SetAxis(null, null, oldLimits.y1, oldLimits.y2);
                Dims.ZoomY(1 - verticalMargin);
                AxesHaveBeenSet = Plottables.Count > 0;
            }
            else
            {
                Dims.SetAxis(newLimits.x1, newLimits.x2, newLimits.y1, newLimits.y2);
                Dims.Zoom(1 - horizontalMargin, 1 - verticalMargin);
                AxesHaveBeenSet = Plottables.Count > 0;
            }
        }

        /// <summary>
        /// Returns the X pixel corresponding to an X axis coordinate
        /// </summary>
        public double GetPixelX(double locationX)
        {
            return (locationX - Dims.XMin) * xAxisScale;
        }

        /// <summary>
        /// Returns the Y pixel corresponding to a Y axis coordinate
        /// </summary>
        public double GetPixelY(double locationY)
        {
            return DataHeight - (float)((locationY - Dims.YMin) * yAxisScale);
        }

        /// <summary>
        /// Returns the pixel corresponding to axis coordinates
        /// </summary>
        public PointF GetPixel(double locationX, double locationY)
        {
            return new PointF((float)GetPixelX(locationX), (float)GetPixelY(locationY));
        }

        /// <summary>
        /// Returns the X axis coordinate corresponding to a X pixel on the plot
        /// </summary>
        public double GetLocationX(double pixelX)
        {
            return (pixelX - DataOffsetX) / xAxisScale + Dims.XMin;
        }

        /// <summary>
        /// Returns the Y axis coordinate corresponding to a Y pixel on the plot
        /// </summary>
        public double GetLocationY(double pixelY)
        {
            return Dims.YMax - (pixelY - DataOffsetY) / yAxisScale;
        }

        /// <summary>
        /// Returns axis coordinates corresponding to a pixel on the plot
        /// </summary>
        public PointF GetLocation(double pixelX, double pixelY)
        {
            // Return the X/Y location corresponding to a pixel position on the figure bitmap.
            // This is useful for converting a mouse position to an X/Y coordinate.
            return new PointF((float)GetLocationX(pixelX), (float)GetLocationY(pixelY));
        }
    }
}
