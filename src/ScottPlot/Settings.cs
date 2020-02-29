using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

// TODO: move plottables to their own module
// TODO: move mouse/axes interaction functions into the mouse module somehow

namespace ScottPlot
{
    /// <summary>
    /// 
    /// This class stores settings and data necessary to create a ScottPlot.
    /// It is a data transfer object which is easy to pass but should be inaccessible to end users.
    /// 
    /// If you are passed this object, you have EVERYTHING you need to render an image.
    /// An ultimate goal is for this settings object to be able to be passed to different rendering engines.
    /// 
    /// </summary>
    public class Settings
    {
        // these properties get set at instantiation or after size or axis adjustments
        public Size figureSize { get { return layout.plot.Size; } }
        public Point dataOrigin { get { return layout.data.Location; } }
        public Size dataSize { get { return layout.data.Size; } }

        // Eventually move graphics objects to their own module.
        public Graphics gfxFigure;
        public Graphics gfxData;
        public Graphics gfxLegend;
        public Bitmap bmpFigure;
        public Bitmap bmpData;
        public Bitmap bmpLegend;

        // plottables
        public readonly List<Plottable> plottables = new List<Plottable>();

        // new config objects (https://github.com/swharden/ScottPlot/issues/120)
        public Config.TextLabel title = new Config.TextLabel() { fontSize = 16, bold = true };
        public Config.TextLabel xLabel = new Config.TextLabel() { fontSize = 16 };
        public Config.TextLabel yLabel = new Config.TextLabel() { fontSize = 16 };
        public Config.Misc misc = new Config.Misc();
        public Config.Benchmark benchmark = new Config.Benchmark();
        public Config.Grid grid = new Config.Grid();
        public Config.Colors colors = new Config.Colors();
        public Config.Axes axes = new Config.Axes();
        public readonly Config.Layout layout = new Config.Layout();
        public Config.Ticks ticks = new Config.Ticks();
        public Config.Legend legend = new Config.Legend();
        public System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.DefaultThreadCurrentCulture;

        // mouse interaction
        public Rectangle? mouseMiddleRect = null;

        // scales calculations must occur at this level because the axes are unaware of pixel dimensions
        public double xAxisScale { get { return dataSize.Width / axes.x.span; } } // pixels per unit
        public double yAxisScale { get { return dataSize.Height / axes.y.span; } } // pixels per unit
        public double xAxisUnitsPerPixel { get { return 1.0 / xAxisScale; } }
        public double yAxisUnitsPerPixel { get { return 1.0 / yAxisScale; } }

        // this has to be here because color module is unaware of plottables list
        public Color GetNextColor() { return colors.GetColor(plottables.Count); }

        public void Resize(int width, int height, bool useMeasuredStrings = false)
        {
            if (useMeasuredStrings && gfxData != null)
            {
                // this section was added before display scaling issues (pixel-referenced font sizes) were figured out.
                // it is probably not needed...

                string sampleString = "IPjg8.8";
                layout.yLabelWidth = (int)gfxData.MeasureString(sampleString, yLabel.font).Height;
                layout.y2LabelWidth = (int)gfxData.MeasureString(sampleString, yLabel.font).Height; // currently y2 isn't supported
                layout.titleHeight = (int)gfxData.MeasureString(sampleString, title.font).Height;
                layout.xLabelHeight = (int)gfxData.MeasureString(sampleString, xLabel.font).Height;

                var tickSize = gfxData.MeasureString("0.001", ticks.font);
                layout.yScaleWidth = (int)tickSize.Width;
                layout.y2ScaleWidth = (int)tickSize.Height; // currently y2 isn't supported
                layout.xScaleHeight = (int)tickSize.Height;
            }

            layout.Update(width, height);
        }

        public void TightenLayout(int padLeft = 15, int padRight = 15, int padBottom = 15, int padTop = 15)
        {
            Resize(figureSize.Width, figureSize.Height);

            // update the layout with sizes based on configuration in settings

            layout.titleHeight = (int)title.size.Height + 3;

            // disable y2 label and scale by default
            layout.y2LabelWidth = 0;
            layout.y2ScaleWidth = 0;

            layout.yLabelWidth = (int)yLabel.size.Height + 3;
            layout.xLabelHeight = (int)xLabel.size.Height + 3;

            // automatically increase yScale size to accomodate wide ticks
            if (ticks?.y?.maxLabelSize.Width > layout.yScaleWidth)
                layout.yScaleWidth = (int)ticks.y.maxLabelSize.Width;

            // automatically increase xScale size to accomodate high ticks
            if (ticks?.x?.maxLabelSize.Height > layout.xScaleHeight)
                layout.xScaleHeight = (int)ticks.x.maxLabelSize.Height;

            // collapse things that are hidden or empty
            if (!ticks.displayXmajor)
                layout.xScaleHeight = 0;
            if (!ticks.displayYmajor)
                layout.yScaleWidth = 0;
            if (title.text == "")
                layout.titleHeight = 0;
            if (yLabel.text == "")
                layout.yLabelWidth = 0;
            if (xLabel.text == "")
                layout.xLabelHeight = 0;

            // eliminate all right-side pixels if right-frame is not drawn
            if (!layout.displayFrameByAxis[1])
            {
                layout.yLabelWidth = 0;
                layout.y2ScaleWidth = 0;
            }

            // expand edges to accomodate argument padding
            layout.yLabelWidth = Math.Max(layout.yLabelWidth, padLeft);
            layout.y2LabelWidth = Math.Max(layout.y2LabelWidth, padRight);
            layout.xLabelHeight = Math.Max(layout.xLabelHeight, padBottom);
            layout.titleHeight = Math.Max(layout.titleHeight, padTop);

            layout.Update(figureSize.Width, figureSize.Height);
            layout.tighteningOccurred = true;
        }

        public void AxesPanPx(int dxPx, int dyPx)
        {
            if (!axes.hasBeenSet)
                AxisAuto();
            axes.x.Pan((double)dxPx / xAxisScale);
            axes.y.Pan((double)dyPx / yAxisScale);
        }

        public void AxesZoomPx(int xPx, int yPx)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + axes.x.span);
            double dYFrac = dY / (Math.Abs(dY) + axes.y.span);
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
                Config.AxisLimits2D plottableLimits = plottable.GetLimits();
                if (autoX && !yExpandOnly)
                    newLimits.ExpandX(plottableLimits);
                if (autoY && !xExpandOnly)
                    newLimits.ExpandY(plottableLimits);
            }

            newLimits.MakeRational();

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
            return dataSize.Height - (float)((locationY - axes.y.min) * yAxisScale);
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
            return (pixelX - dataOrigin.X) / xAxisScale + axes.x.min;
        }

        /// <summary>
        /// Returns the Y axis coordinate corresponding to a Y pixel on the plot
        /// </summary>
        public double GetLocationY(double pixelY)
        {
            return axes.y.max - (pixelY - dataOrigin.Y) / yAxisScale;
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

        public int GetTotalPointCount()
        {
            int totalPointCount = 0;
            foreach (Plottable plottable in plottables)
                totalPointCount += plottable.pointCount;
            return totalPointCount;
        }

        public void Clear(bool axLines = true, bool scatters = true, bool signals = true, bool text = true, bool bar = true, bool finance = true, bool axSpans = true)
        {
            // TODO: This is a switch code smell. Use inversion of control here.
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < plottables.Count; i++)
            {
                if ((plottables[i] is PlottableVLine || plottables[i] is PlottableHLine) && axLines)
                    indicesToDelete.Add(i);
                else if (plottables[i] is PlottableScatter && scatters)
                    indicesToDelete.Add(i);
                else if (plottables[i] is PlottableSignal && signals)
                    indicesToDelete.Add(i);
                else if (plottables[i].GetType().IsGenericType && plottables[i].GetType().GetGenericTypeDefinition() == typeof(PlottableSignalConst<>) && signals)
                    indicesToDelete.Add(i);
                else if (plottables[i] is PlottableText && text)
                    indicesToDelete.Add(i);
                else if (plottables[i] is PlottableBar && bar)
                    indicesToDelete.Add(i);
                else if (plottables[i] is PlottableOHLC && finance)
                    indicesToDelete.Add(i);
                else if ((plottables[i] is PlottableVSpan || plottables[i] is PlottableHSpan) && axSpans)
                    indicesToDelete.Add(i);
            }
            indicesToDelete.Reverse();

            for (int i = 0; i < indicesToDelete.Count; i++)
            {
                plottables.RemoveAt(indicesToDelete[i]);
            }

            axes.x.hasBeenSet = false;
            axes.y.hasBeenSet = false;
        }
    }
}
