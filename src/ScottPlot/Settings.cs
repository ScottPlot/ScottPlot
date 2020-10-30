using ScottPlot.Renderable;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot
{
    public class Settings
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float DataOffsetX { get; private set; }
        public float DataOffsetY { get; private set; }
        public float DataWidth { get; private set; }
        public float DataHeight { get; private set; }

        // plottables
        public readonly List<IRenderable> plottables = new List<IRenderable>();

        public Config.Misc misc = new Config.Misc();
        public Config.Axes axes = new Config.Axes();
        public readonly Config.Layout layout = new Config.Layout();
        public Config.Ticks ticks = new Config.Ticks();
        public System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.DefaultThreadCurrentCulture;

        // default colorset
        public Drawing.Colorset colorset = Drawing.Colorset.Category10;

        // mouse interaction
        public Rectangle? mouseMiddleRect = null;

        // scales calculations must occur at this level because the axes are unaware of pixel dimensions
        public double xAxisScale { get { return DataWidth / axes.x.span; } } // pixels per unit
        public double yAxisScale { get { return DataHeight / axes.y.span; } } // pixels per unit
        public double xAxisUnitsPerPixel { get { return 1.0 / xAxisScale; } }
        public double yAxisUnitsPerPixel { get { return 1.0 / yAxisScale; } }

        // this has to be here because color module is unaware of plottables list
        public Color GetNextColor() { return colorset.GetColor(plottables.Count); }

        public void Resize(int width, int height, bool useMeasuredStrings = false)
        {
            layout.Update(width, height);

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

            Width = width;
            Height = height;
            DataWidth = layout.data.Width;
            DataHeight = layout.data.Height;
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

            foreach (var plottable in plottables)
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

            if (plottables.Count == 0)
            {
                axes.x.hasBeenSet = false;
                axes.y.hasBeenSet = false;
            }

            layout.tighteningOccurred = false;
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
