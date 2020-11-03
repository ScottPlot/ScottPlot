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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }

        /// <summary>
        /// Adjust data padding based on axis size
        /// </summary>
        public void RecalculateLayout()
        {
            foreach (var axis in AllAxes)
                axis.AutoSize();

            float padLeft = AllAxes.Where(x => x.Edge == Edge.Left).Select(x => x.PixelSize).Sum();
            float padRight = AllAxes.Where(x => x.Edge == Edge.Right).Select(x => x.PixelSize).Sum();
            float padBottom = AllAxes.Where(x => x.Edge == Edge.Bottom).Select(x => x.PixelSize).Sum();
            float padTop = AllAxes.Where(x => x.Edge == Edge.Top).Select(x => x.PixelSize).Sum();

            DataOffsetX = padLeft;
            DataOffsetY = padTop;
            DataWidth = Width - padLeft - padRight;
            DataHeight = Height - padTop - padBottom;
        }

        public PlotDimensions GetDimensions(bool recalculateLayout = false)
        {
            if (recalculateLayout)
                RecalculateLayout();

            var dims = new PlotDimensions(
                figureSize: new SizeF(Width, Height),
                dataSize: new SizeF(DataWidth, DataHeight),
                dataOffset: new PointF(DataOffsetX, DataOffsetY),
                axisLimits: axes.Limits);

            if (axes.equalAxes)
            {
                if (dims.UnitsPerPxY > dims.UnitsPerPxX)
                    axes.Zoom(dims.UnitsPerPxX / dims.UnitsPerPxY, 1);
                else
                    axes.Zoom(1, dims.UnitsPerPxY / dims.UnitsPerPxX);
            }

            return dims;
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
        public CultureInfo Culture = CultureInfo.DefaultThreadCurrentCulture;
        public Palette PlottablePalette = Palette.Category10;

        // axes contain axis label, tick, and grid settings
        public readonly List<Axis> XAxes = new List<Axis>() {
            new Axis() { Edge = Edge.Bottom, PixelSize = 40, MajorGrid = true },
            new Axis() { Edge = Edge.Top, PixelSize = 40, Bold = true },
        };
        public readonly List<Axis> YAxes = new List<Axis>() {
            new Axis() { Edge = Edge.Left, PixelSize = 60, MajorGrid = true },
            new Axis() { Edge = Edge.Right, PixelSize = 60 }
        };
        public Axis[] AllAxes { get => XAxes.Concat(YAxes).ToArray(); }

        public Axis XAxis { get => XAxes[0]; } // bottom
        public Axis XAxis2 { get => XAxes[1]; } // top (title)
        public Axis YAxis { get => YAxes[0]; } // left
        public Axis YAxis2 { get => YAxes[1]; } // right
        public Axis[] PrimaryAxes => new Axis[] { YAxis, YAxis2, XAxis, XAxis2 }; // LRBT

        /*
         * ##################################################################################
         * # OLD SETTINGS WHICH I AM WORKING TO STRANGLE
         * 
         */

        // TODO: move these settings into each Axis module
        public Config.Ticks ticks = new Config.Ticks();

        // TODO: move this functionality into the PlotDimensions module
        public Config.Axes axes = new Config.Axes();
        public double xAxisScale { get { return DataWidth / axes.x.span; } } // pixels per unit
        public double yAxisScale { get { return DataHeight / axes.y.span; } } // pixels per unit
        public double xAxisUnitsPerPixel { get { return 1.0 / xAxisScale; } }
        public double yAxisUnitsPerPixel { get { return 1.0 / yAxisScale; } }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            if (axes.equalAxes)
            {
                var limits = new Config.AxisLimits2D(axes.ToArray());

                double xUnitsPerPixel = limits.xSpan / DataWidth;
                double yUnitsPerPixel = limits.ySpan / DataHeight;

                if (yUnitsPerPixel > xUnitsPerPixel)
                    axes.Zoom(xUnitsPerPixel / yUnitsPerPixel, 1);
                else
                    axes.Zoom(1, yUnitsPerPixel / xUnitsPerPixel);
            }
        }

        public void AxesPanPx(int dxPx, int dyPx)
        {
            if (!axes.hasBeenSet)
                AxisAuto();
            axes.x.Pan((double)dxPx / xAxisScale);
            axes.y.Pan((double)dyPx / yAxisScale);
        }

        public void AxesZoomPx(int xPx, int yPx, bool lockRatio = false)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + axes.x.span);
            double dYFrac = dY / (Math.Abs(dY) + axes.y.span);
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
            axes.Zoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void AxisAuto(
            double horizontalMargin = .1, double verticalMargin = .1,
            bool xExpandOnly = false, bool yExpandOnly = false,
            bool autoX = true, bool autoY = true
            )
        {
            var oldLimits = new Config.AxisLimits2D(axes.ToArray());
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

            if (xExpandOnly)
            {
                oldLimits.ExpandX(newLimits);
                axes.Set(oldLimits.x1, oldLimits.x2, null, null);
                axes.Zoom(1 - horizontalMargin, 1);
            }

            if (yExpandOnly)
            {
                oldLimits.ExpandY(newLimits);
                axes.Set(null, null, oldLimits.y1, oldLimits.y2);
                axes.Zoom(1, 1 - verticalMargin);
            }

            if ((!xExpandOnly) && (!yExpandOnly))
            {
                axes.Set(newLimits);
                axes.Zoom(1 - horizontalMargin, 1 - verticalMargin);
            }

            if (Plottables.Count == 0)
            {
                axes.x.hasBeenSet = false;
                axes.y.hasBeenSet = false;
            }
        }

        /// <summary>
        /// Returns the X pixel corresponding to an X axis coordinate
        /// </summary>
        public double GetPixelX(double locationX)
        {
            return (locationX - axes.x.min) * xAxisScale;
        }

        /// <summary>
        /// Returns the Y pixel corresponding to a Y axis coordinate
        /// </summary>
        public double GetPixelY(double locationY)
        {
            return DataHeight - (float)((locationY - axes.y.min) * yAxisScale);
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
            return (pixelX - DataOffsetX) / xAxisScale + axes.x.min;
        }

        /// <summary>
        /// Returns the Y axis coordinate corresponding to a Y pixel on the plot
        /// </summary>
        public double GetLocationY(double pixelY)
        {
            return axes.y.max - (pixelY - DataOffsetY) / yAxisScale;
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
