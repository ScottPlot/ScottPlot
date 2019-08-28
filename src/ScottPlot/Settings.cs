using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

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
        public Size figureSize { get; private set; } // TODO: this pass through to bmpFigure.Size and Resize()
        public Point dataOrigin { get; private set; }
        public Size dataSize { get; private set; }

        // hold copies of graphics objects to make them easy to plot to
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

        // scales calculations must occur at this level because the axes are unaware of pixel dimensions
        public double xAxisScale { get { return bmpData.Width / axes.x.span; } }
        public double yAxisScale { get { return bmpData.Height / axes.y.span; } }

        // mouse tracking
        public Point mouseDownLocation = new Point(0, 0);
        public double[] mouseDownLimits = new double[4];

        // mouse middle-click-zooming
        public Point mouseZoomDownLocation = new Point(0, 0);
        public Point mouseZoomCurrentLocation = new Point(0, 0);
        public bool mouseZoomRectangleIsHappening = false;

        // experimental settings
        public bool useParallel = false;

        public Settings()
        {

        }

        public Color GetNextColor()
        {
            return colors.GetColor(plottables.Count);
        }

        public void Resize(int width, int height)
        {
            figureSize = new Size(width, height);
            dataOrigin = new Point(layout.paddingBySide[0], layout.paddingBySide[3]);
            int dataWidth = figureSize.Width - layout.paddingBySide[0] - layout.paddingBySide[1];
            int dataHeight = figureSize.Height - layout.paddingBySide[2] - layout.paddingBySide[3];
            dataSize = new Size(dataWidth, dataHeight);
            AxisUpdate();
        }

        public void AxisSet(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            axes.x.min = x1 ?? axes.x.min;
            axes.x.max = x2 ?? axes.x.max;
            axes.y.min = y1 ?? axes.y.min;
            axes.y.max = y2 ?? axes.y.max;
            axes.hasBeenSet = true;
            AxisUpdate();
        }

        public void AxisTighen()
        {
            // "tighten" the plot by reducing whitespce between labels, data, and the edge of the figure

            if (ticks.tickCollectionX == null)
                return;

            int tickLetterHeight = (int)gfxFigure.MeasureString("test", ticks.tickFont).Height;

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
            layout.paddingBySide[0] += (int)ticks.tickCollectionY.maxLabelSize.Width;
            layout.paddingBySide[0] += layout.padOnAllSides;

            // right
            layout.paddingBySide[1] = (int)ticks.tickCollectionY.maxLabelSize.Width / 2;
            layout.paddingBySide[1] += layout.padOnAllSides;

            // override for frameles
            if (!layout.displayAxisFrames)
                layout.paddingBySide = new int[] { 0, 0, 0, 0 };

            layout.tighteningOccurred = true;
        }

        public bool bmpFigureRenderRequired = true;

        public void AxisUpdate()
        {
            bmpFigureRenderRequired = true;
        }

        public void AxisPan(double? dx = null, double? dy = null)
        {
            if (!axes.hasBeenSet)
                AxisAuto();

            if (dx != null)
                axes.x.Pan((double)dx);

            if (dy != null)
                axes.y.Pan((double)dy);

            AxisUpdate();
        }

        public void AxisZoom(double xFrac = 1, double yFrac = 1, PointF? zoomCenter = null)
        {
            if (!axes.hasBeenSet)
                AxisAuto();

            if (zoomCenter == null)
            {
                axes.x.Zoom(xFrac);
                axes.y.Zoom(yFrac);
            }
            else
            {
                axes.x.Zoom(xFrac, (double)((PointF)zoomCenter).X);
                axes.y.Zoom(yFrac, (double)((PointF)zoomCenter).Y);
            }

            AxisUpdate();
        }

        private void AxisZoomPx(int xPx, int yPx)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + axes.x.span);
            double dYFrac = dY / (Math.Abs(dY) + axes.y.span);
            AxisZoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void AxisAuto(double horizontalMargin = .1, double verticalMargin = .1, bool xExpandOnly = false, bool yExpandOnly = false)
        {

            double[] original = new double[4] { axes.x.min, axes.x.max, axes.y.min, axes.y.max };

            double[] newAxes = new double[4];

            List<Plottable> plottables2d = new List<Plottable>();
            List<PlottableAxLine> axisLines = new List<PlottableAxLine>();

            foreach (Plottable plottable in plottables)
            {
                if (plottable is PlottableAxLine axLine)
                    axisLines.Add(axLine);
                else
                    plottables2d.Add(plottable);
            }

            foreach (Plottable plottable in plottables2d)
            {
                double[] limits = plottable.GetLimits();

                if (newAxes == null)
                {
                    newAxes = limits;
                }
                else
                {
                    if (limits[0] < newAxes[0])
                        newAxes[0] = limits[0];
                    if (limits[1] > newAxes[1])
                        newAxes[1] = limits[1];
                    if (limits[2] < newAxes[2])
                        newAxes[2] = limits[2];
                    if (limits[3] > newAxes[3])
                        newAxes[3] = limits[3];
                }
            }

            foreach (PlottableAxLine axLine in axisLines)
            {
                if (newAxes == null)
                    continue;

                if (axLine.vertical)
                {
                    if (axLine.position < newAxes[0])
                        newAxes[0] = axLine.position;
                    if (axLine.position > newAxes[1])
                        newAxes[1] = axLine.position;
                }
                else
                {
                    if (axLine.position < newAxes[2])
                        newAxes[2] = axLine.position;
                    if (axLine.position > newAxes[3])
                        newAxes[3] = axLine.position;
                }
            }

            if (newAxes == null)
            {
                newAxes = new double[] { -10, 10, -10, 10 };
            }
            else
            {
                if (newAxes[0] == newAxes[1])
                {
                    newAxes[0] = newAxes[0] - 1;
                    newAxes[1] = newAxes[1] + 1;
                }

                if (newAxes[2] == newAxes[3])
                {
                    newAxes[2] = newAxes[2] - 1;
                    newAxes[3] = newAxes[3] + 1;
                }
            }

            axes.x.min = newAxes[0];
            axes.x.max = newAxes[1];
            axes.y.min = newAxes[2];
            axes.y.max = newAxes[3];

            axes.hasBeenSet = true;
            AxisUpdate();
            AxisZoom(1 - horizontalMargin, 1 - verticalMargin);

            if (xExpandOnly && original != null)
            {
                axes.x.min = Math.Min(axes.x.min, original[0]);
                axes.x.max = Math.Max(axes.x.max, original[1]);
                AxisUpdate();
            }

            if (yExpandOnly && original != null)
            {
                axes.y.min = Math.Min(axes.y.min, original[2]);
                axes.y.max = Math.Max(axes.y.max, original[3]);
                AxisUpdate();
            }

        }

        public void Validate()
        {
            if (figureSize == null || figureSize.Width < 1 || figureSize.Height < 1)
                throw new Exception("figure width and height must be greater than 0px");
        }

        public bool mouseIsPanning = false;
        public bool mouseIsZooming = false;
        public void MouseDown(int cusorPosX, int cursorPosY, bool panning = false, bool zooming = false)
        {
            mouseDownLocation = new Point(cusorPosX, cursorPosY);
            mouseIsPanning = panning;
            mouseIsZooming = zooming;
            Array.Copy(axes.limits, mouseDownLimits, 4);
        }

        public void MouseMoveAxis(int cursorPosX, int cursorPosY, bool lockVertical, bool lockHorizontal)
        {
            if (mouseIsPanning == false && mouseIsZooming == false)
                return;

            axes.x.min = mouseDownLimits[0];
            axes.x.max = mouseDownLimits[1];
            axes.y.min = mouseDownLimits[2];
            axes.y.max = mouseDownLimits[3];

            Console.WriteLine($"reset to original limits: {axes}");
            AxisUpdate();

            int dX = cursorPosX - mouseDownLocation.X;
            int dY = cursorPosY - mouseDownLocation.Y;

            if (lockVertical)
                dY = 0;
            if (lockHorizontal)
                dX = 0;

            if (mouseIsPanning)
                AxisPan(-dX / xAxisScale, dY / yAxisScale);

            if (mouseIsZooming)
                AxisZoomPx(dX, -dY);
        }

        public void MouseUpAxis()
        {
            mouseIsPanning = false;
            mouseIsZooming = false;
        }

        public void MouseZoomRectMove(Point eLocation)
        {
            mouseZoomCurrentLocation = eLocation;
            mouseZoomRectangleIsHappening = true;
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
