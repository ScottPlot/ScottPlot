using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

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
        public Size figureSize { get; private set; }
        public Point dataOrigin { get; private set; }
        public Size dataSize { get; private set; }

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
        public Config.Title title = new Config.Title();
        public Config.TextLabel xLabel = new Config.TextLabel() { fontSize = 16 };
        public Config.TextLabel yLabel = new Config.TextLabel() { fontSize = 16 };
        public Config.Misc misc = new Config.Misc();
        public Config.Benchmark benchmark = new Config.Benchmark();
        public Config.Grid grid = new Config.Grid();
        public Config.Colors colors = new Config.Colors();
        public Config.Axes axes = new Config.Axes();
        public Config.Layout layout = new Config.Layout();
        public Config.Ticks ticks = new Config.Ticks();
        public Config.Legend legend = new Config.Legend();
        public Config.Mouse mouse = new Config.Mouse();

        // scales calculations must occur at this level because the axes are unaware of pixel dimensions
        public double xAxisScale { get { return bmpData.Width / axes.x.span; } }
        public double yAxisScale { get { return bmpData.Height / axes.y.span; } }

        // this has to be here because color module is unaware of plottables list
        public Color GetNextColor() { return colors.GetColor(plottables.Count); }


        public void Resize(int width, int height)
        {
            // TODO: data origin should be calculated at render time, not now.
            figureSize = new Size(width, height);
            dataOrigin = new Point(layout.paddingBySide[0], layout.paddingBySide[3]);
            int dataWidth = figureSize.Width - layout.paddingBySide[0] - layout.paddingBySide[1];
            int dataHeight = figureSize.Height - layout.paddingBySide[2] - layout.paddingBySide[3];
            dataSize = new Size(dataWidth, dataHeight);
        }

        public void AxisTighen()
        {
            // "tighten" the plot by reducing whitespce between labels, data, and the edge of the figure

            if (ticks.x == null)
                return;

            int tickLetterHeight = (int)gfxFigure.MeasureString("test", ticks.font).Height;

            // top
            layout.paddingBySide[3] = 1;
            layout.paddingBySide[3] += Math.Max((int)title.height, tickLetterHeight);
            layout.paddingBySide[3] += layout.padOnAllSides;

            // bottom
            int xLabelHeight = (int)gfxFigure.MeasureString(xLabel.text, xLabel.font).Height;
            layout.paddingBySide[2] = Math.Max(xLabelHeight, tickLetterHeight);
            layout.paddingBySide[2] += tickLetterHeight;
            layout.paddingBySide[2] += layout.padOnAllSides;

            // left
            SizeF yLabelSize = gfxFigure.MeasureString(yLabel.text, yLabel.font);
            layout.paddingBySide[0] = (int)yLabelSize.Height;
            layout.paddingBySide[0] += (int)ticks.y.maxLabelSize.Width;
            layout.paddingBySide[0] += layout.padOnAllSides;

            // right
            layout.paddingBySide[1] = (int)ticks.y.maxLabelSize.Width / 2;
            layout.paddingBySide[1] += layout.padOnAllSides;

            // override for frameles
            if (!layout.displayAxisFrames)
                layout.paddingBySide = new int[] { 0, 0, 0, 0 };

            layout.tighteningOccurred = true;
        }

        public void AxesPanPx(int dxPx, int dyPx)
        {
            if (!axes.hasBeenSet)
                AxisAuto();
            axes.x.Pan((double)dxPx / xAxisScale);
            axes.y.Pan((double)dyPx / yAxisScale);
        }

        private void AxesZoomPx(int xPx, int yPx)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + axes.x.span);
            double dYFrac = dY / (Math.Abs(dY) + axes.y.span);
            axes.Zoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void AxisAuto(double horizontalMargin = .1, double verticalMargin = .1, bool xExpandOnly = false, bool yExpandOnly = false)
        {
            // separately adjust on plottables vs. axis lines
            List<Plottable> plottables2d = new List<Plottable>();
            List<PlottableAxLine> axisLines = new List<PlottableAxLine>();
            foreach (Plottable plottable in plottables)
            {
                if (plottable is PlottableAxLine axLine)
                    axisLines.Add(axLine);
                else
                    plottables2d.Add(plottable);
            }

            // expand on non-axis lines first
            if (plottables2d.Count == 0)
            {
                axes.Set(-10, 10, -10, 10);
            }
            else
            {
                axes.Set(plottables[0].GetLimits());
                foreach (Plottable plottable in plottables)
                {
                    if (!(plottable is PlottableAxLine axLine))
                        axes.Expand(plottable.GetLimits());
                }
            }

            // expand on axis lines last
            foreach (Plottable plottable in plottables)
            {
                if (plottable is PlottableAxLine axLine)
                {
                    var axl = (PlottableAxLine)plottable;
                    double[] limits = plottable.GetLimits();
                    if (axl.vertical)
                    {
                        limits[2] = axes.y.min;
                        limits[3] = axes.y.max;
                    }
                    else
                    {
                        limits[0] = axes.x.min;
                        limits[1] = axes.x.max;
                    }
                    axes.Expand(limits);
                }
            }

            axes.hasBeenSet = true;
            axes.Zoom(1 - horizontalMargin, 1 - verticalMargin);
        }

        public void MouseDown(int cusorPosX, int cursorPosY, bool panning = false, bool zooming = false)
        {
            mouse.downLoc = new Point(cusorPosX, cursorPosY);
            mouse.isPanning = panning;
            mouse.isZooming = zooming;
            Array.Copy(axes.limits, mouse.downLimits, 4);
        }

        public void MouseMoveAxis(int cursorPosX, int cursorPosY, bool lockVertical, bool lockHorizontal)
        {
            if (mouse.isPanning == false && mouse.isZooming == false)
                return;

            axes.x.min = mouse.downLimits[0];
            axes.x.max = mouse.downLimits[1];
            axes.y.min = mouse.downLimits[2];
            axes.y.max = mouse.downLimits[3];

            int dX = cursorPosX - mouse.downLoc.X;
            int dY = cursorPosY - mouse.downLoc.Y;

            if (lockVertical)
                dY = 0;
            if (lockHorizontal)
                dX = 0;

            if (mouse.isPanning)
                AxesPanPx(-dX, dY);

            if (mouse.isZooming)
                AxesZoomPx(dX, -dY);
        }

        public void MouseUpAxis()
        {
            mouse.isPanning = false;
            mouse.isZooming = false;
        }

        public void MouseZoomRectMove(Point eLocation)
        {
            mouse.currentLoc = eLocation;
            mouse.rectangleIsHappening = true;
        }

        public PointF GetPixel(double locationX, double locationY)
        {
            // Return the pixel location on the data bitmap corresponding to an X/Y location.
            // This is useful when drawing graphics on the data bitmap.
            float xPx = (float)((locationX - axes.x.min) * xAxisScale);
            float yPx = dataSize.Height - (float)((locationY - axes.y.min) * yAxisScale);
            return new PointF(xPx, yPx);
        }

        public PointF GetLocation(int pixelX, int pixelY)
        {
            // Return the X/Y location corresponding to a pixel position on the figure bitmap.
            // This is useful for converting a mouse position to an X/Y coordinate.
            double locationX = (pixelX - dataOrigin.X) / xAxisScale + axes.x.min;
            double locationY = axes.y.max - (pixelY - dataOrigin.Y) / yAxisScale;
            return new PointF((float)locationX, (float)locationY);
        }

        public int GetTotalPointCount()
        {
            int totalPointCount = 0;
            foreach (Plottable plottable in plottables)
                totalPointCount += plottable.pointCount;
            return totalPointCount;
        }

        public void Clear(bool axLines = true, bool scatters = true, bool signals = true, bool text = true, bool bar = true, bool finance = true)
        {
            List<int> indicesToDelete = new List<int>();
            for (int i = 0; i < plottables.Count; i++)
            {
                if (plottables[i] is PlottableAxLine && axLines)
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
            }
            indicesToDelete.Reverse();

            for (int i = 0; i < indicesToDelete.Count; i++)
            {
                plottables.RemoveAt(indicesToDelete[i]);
            }
        }
    }
}
