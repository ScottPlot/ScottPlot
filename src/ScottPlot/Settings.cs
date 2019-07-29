using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ScottPlot
{
    /// <summary>
    /// This class stores settings and data necessary to create a ScottPlot.
    /// It is a data transfer object which is easy to pass but inaccessible to users.
    /// </summary>
    public class Settings
    {
        // these properties get set at instantiation or after size or axis adjustments
        public Size figureSize { get; private set; }
        public Point dataOrigin { get; private set; }
        public Size dataSize { get; private set; }

        // hold copies of graphics objects to make them easy to plot to
        public Graphics gfxFigure;
        public Graphics gfxData;
        public Bitmap bmpFigure;
        public Bitmap bmpData;

        // axis (replace with class)
        public double[] axis = new double[] { -10, 10, -10, 10 }; // X1, X2, Y1, Y2
        public double xAxisSpan;
        public double yAxisSpan;
        public double xAxisCenter;
        public double yAxisCenter;
        public double xAxisScale;
        public double yAxisScale;
        public bool axisHasBeenIntentionallySet = false;

        // background colors
        public Color figureBackgroundColor = Color.White;
        public Color dataBackgroundColor = Color.White;

        // axis settings
        public int axisPadding = 5;
        public bool[] displayFrameByAxis = new bool[] { true, true, true, true };
        public int[] axisLabelPadding = new int[] { 140, 120, 140, 55 }; // X1, X2, Y1, Y2
        public bool displayAxisFrames = true;
        public bool tighteningOccurred = false;

        // axis ticks
        public Font tickFont = new Font("Segoe UI", 10);
        public List<Tick> ticksX;
        public List<Tick> ticksY;
        public int tickSize = 5;
        public Color tickColor = Color.Black;
        public bool displayTicksX = true;
        public bool displayTicksY = true;

        // title and axis labels
        public string title = "";
        public Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
        public Color titleColor = Color.Black;
        public string axisLabelX = "";
        public string axisLabelY = "";
        public Font axisLabelFont = new Font("Segoe UI", 16);
        public Color axisLabelColor = Color.Black;

        // grid
        public bool displayGrid = true;
        public Color gridColor = Color.LightGray;
        public double gridSpacingX = 0;
        public double gridSpacingY = 0;

        // legend
        public Font legendFont = new Font("Segoe UI", 12);
        public Color legendFontColor = Color.Black;
        public Color legendBackColor = Color.White;
        public Color legendShadowColor = Color.FromArgb(75, 0, 0, 0);
        public Color legendFrameColor = Color.Black;
        public legendLocation legendLocation = legendLocation.none;
        public shadowDirection legendShadowDirection = shadowDirection.none;

        // benchmarking
        public Font benchmarkFont = new Font("Consolas", 8);
        public Brush benchmarkFontBrush = Brushes.Black;
        public Brush benchmarkBackgroundBrush = new SolidBrush(Color.FromArgb(150, Color.LightYellow));
        public Pen benchmarkBorderPen = Pens.Black;
        public Stopwatch benchmarkStopwatch = new Stopwatch();
        public double benchmarkMsec;
        public double benchmarkHz;
        public string benchmarkMessage;
        public bool displayBenchmark = false;

        // mouse tracking
        private Point mouseDownLocation = new Point(0, 0);
        private double[] mouseDownAxis = new double[4];

        // plottables
        public readonly List<Plottable> plottables = new List<Plottable>();
        public bool useTwentyColors = false;
        public bool antiAliasData = true;
        public bool antiAliasFigure = true;

        // plot colors (https://github.com/vega/vega/wiki/Scales#scale-range-literals)
        string[] plottableColors10 = new string[] { "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728",
                    "#9467bd", "#8c564b", "#e377c2", "#7f7f7f", "#bcbd22", "#17becf" };
        string[] plottableColors20 = new string[] { "#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78",
                "#2ca02c", "#98df8a", "#d62728", "#ff9896", "#9467bd", "#c5b0d5",
                "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7",
                "#bcbd22", "#dbdb8d", "#17becf", "#9edae5", };

        // string formats (position indicates where their origin is)
        public StringFormat sfEast = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };
        public StringFormat sfNorthWest = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };
        public StringFormat sfNorthEast = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Far };
        public StringFormat sfSouth = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };
        public StringFormat sfSouthWest = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Near };
        public StringFormat sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };

        public Settings()
        {

        }

        public void BenchmarkStart()
        {
            benchmarkStopwatch.Restart();
        }

        public void BenchmarkEnd()
        {
            benchmarkStopwatch.Stop();
            benchmarkMsec = benchmarkStopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            benchmarkHz = 1000.0 / benchmarkMsec;
            benchmarkMessage = "";
            benchmarkMessage += string.Format("Full render of {0} objects ({1:n} points)", plottables.Count, GetTotalPointCount());
            benchmarkMessage += string.Format(" took {0:0.000} ms ({1:0.00 Hz})", benchmarkMsec, benchmarkHz);
            if (plottables.Count == 1)
                benchmarkMessage = benchmarkMessage.Replace("objects", "object");
            if (!bmpFigureRenderRequired)
                benchmarkMessage = benchmarkMessage.Replace("Full", "Data-only");
        }

        public void Resize(int width, int height)
        {
            figureSize = new Size(width, height);
            dataOrigin = new Point(axisLabelPadding[0], axisLabelPadding[3]);
            int dataWidth = figureSize.Width - axisLabelPadding[0] - axisLabelPadding[1];
            int dataHeight = figureSize.Height - axisLabelPadding[2] - axisLabelPadding[3];
            dataSize = new Size(dataWidth, dataHeight);
            AxisUpdate();
        }

        public void AxisSet(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            if (x1 != null && (double)x1 != axis[0])
                axis[0] = (double)x1;

            if (x2 != null && (double)x2 != axis[1])
                axis[1] = (double)x2;

            if (y1 != null && (double)y1 != axis[2])
                axis[2] = (double)y1;

            if (y2 != null && (double)y2 != axis[3])
                axis[3] = (double)y2;

            axisHasBeenIntentionallySet = true;
            if (x1 != null || x2 != null || y1 != null || y2 != null)
                AxisUpdate();
        }

        public void AxisTighen()
        {
            Ticks ticks = new Ticks(this);
            Size maxTickSizeHoriz = ticks.GetMaxTickSize(ticksX);
            Size maxTickSizeVert = ticks.GetMaxTickSize(ticksY);
            if (displayTicksX == false)
                maxTickSizeHoriz = new Size(0, 0);
            if (displayTicksY == false)
                maxTickSizeVert = new Size(0, 0);

            SizeF biggestTickSize = gfxFigure.MeasureString("-888", tickFont);
            int tickHeight = (int)biggestTickSize.Height;

            // top
            SizeF titleSize = gfxFigure.MeasureString(title, titleFont);
            int titleHeight = (int)(titleSize.Height);
            axisLabelPadding[3] = Math.Max(titleHeight, tickHeight) + axisPadding * 2;

            // bottom
            SizeF xLabelSize = gfxFigure.MeasureString(axisLabelX, axisLabelFont);
            int xLabelHeight = (int)(xLabelSize.Height);
            axisLabelPadding[2] = Math.Max(xLabelHeight, tickHeight) + axisPadding * 2;
            axisLabelPadding[2] += maxTickSizeHoriz.Height;

            // left
            SizeF yLabelSize = gfxFigure.MeasureString(axisLabelY, axisLabelFont);
            axisLabelPadding[0] = (int)(yLabelSize.Height) + axisPadding * 2;
            axisLabelPadding[0] += maxTickSizeVert.Width;

            // right
            axisLabelPadding[1] = axisPadding + maxTickSizeHoriz.Width / 2;

            // override for frameles
            if (axisPadding == 0)
                axisLabelPadding = new int[] { 0, 0, 0, 0 };

            tighteningOccurred = true;
        }

        public bool bmpFigureRenderRequired = true;

        private void AxisUpdate()
        {
            xAxisSpan = axis[1] - axis[0];
            xAxisCenter = (axis[1] + axis[0]) / 2;
            yAxisSpan = axis[3] - axis[2];
            yAxisCenter = (axis[3] + axis[2]) / 2;
            xAxisScale = dataSize.Width / xAxisSpan; // px per unit
            yAxisScale = dataSize.Height / yAxisSpan; // px per unit
            bmpFigureRenderRequired = true;
        }

        public void AxisPan(double? dx = null, double? dy = null)
        {
            if (!axisHasBeenIntentionallySet)
                AxisAuto();

            bool shiftedX = (dx != null && dx != 0);
            bool shiftedY = (dy != null && dy != 0);

            if (shiftedX)
            {
                axis[0] += (double)dx;
                axis[1] += (double)dx;
            }

            if (shiftedY)
            {
                axis[2] += (double)dy;
                axis[3] += (double)dy;
            }

            if (shiftedX || shiftedY)
                AxisUpdate();
        }

        public void AxisZoom(double xFrac = 1, double yFrac = 1, PointF? zoomCenter = null)
        {
            if (!axisHasBeenIntentionallySet)
                AxisAuto();

            if (zoomCenter == null)
            {
                double halfNewXspan = xAxisSpan / xFrac / 2;
                double halfNewYspan = yAxisSpan / yFrac / 2;
                axis[0] = xAxisCenter - halfNewXspan;
                axis[1] = xAxisCenter + halfNewXspan;
                axis[2] = yAxisCenter - halfNewYspan;
                axis[3] = yAxisCenter + halfNewYspan;
            }
            else
            {
                double spanLeft = zoomCenter.Value.X - axis[0];
                double spanRight = axis[1] - zoomCenter.Value.X;
                double spanTop = zoomCenter.Value.Y - axis[2];
                double spanBot = axis[3] - zoomCenter.Value.Y;
                axis[0] = zoomCenter.Value.X - spanLeft / xFrac;
                axis[1] = zoomCenter.Value.X + spanRight / xFrac;
                axis[2] = zoomCenter.Value.Y - spanTop / xFrac;
                axis[3] = zoomCenter.Value.Y + spanBot / xFrac;
            }

            if ((xFrac != 1) || (yFrac != 1))
                AxisUpdate();
        }

        private void AxisZoomPx(int xPx, int yPx)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + xAxisSpan);
            double dYFrac = dY / (Math.Abs(dY) + yAxisSpan);
            AxisZoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void AxisAuto(double horizontalMargin = .1, double verticalMargin = .1)
        {
            axis = null;

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

                if (axis == null)
                {
                    axis = limits;
                }
                else
                {
                    if (limits[0] < axis[0])
                        axis[0] = limits[0];
                    if (limits[1] > axis[1])
                        axis[1] = limits[1];
                    if (limits[2] < axis[2])
                        axis[2] = limits[2];
                    if (limits[3] > axis[3])
                        axis[3] = limits[3];
                }
            }

            foreach (PlottableAxLine axLine in axisLines)
            {
                if (axis == null)
                    continue;

                if (axLine.vertical)
                {
                    if (axLine.position < axis[0])
                        axis[0] = axLine.position;
                    if (axLine.position > axis[1])
                        axis[1] = axLine.position;
                }
                else
                {
                    if (axLine.position < axis[2])
                        axis[2] = axLine.position;
                    if (axLine.position > axis[3])
                        axis[3] = axLine.position;
                }
            }

            if (axis == null)
            {
                axis = new double[] { -10, 10, -10, 10 };
            }
            else
            {
                if (axis[0] == axis[1])
                {
                    axis[0] = axis[0] - 1;
                    axis[1] = axis[1] + 1;
                }

                if (axis[2] == axis[3])
                {
                    axis[2] = axis[2] - 1;
                    axis[3] = axis[3] + 1;
                }
            }

            axisHasBeenIntentionallySet = true;
            AxisUpdate();
            AxisZoom(1 - horizontalMargin, 1 - verticalMargin);
        }

        public void Validate()
        {
            if (figureSize == null || figureSize.Width < 1 || figureSize.Height < 1)
                throw new Exception("figure width and height must be greater than 0px");
            if (axis == null)
                throw new Exception("axis has not yet been initialized");
        }

        public bool mouseIsPanning = false;
        public bool mouseIsZooming = false;
        public void MouseDown(int cusorPosX, int cursorPosY, bool panning = false, bool zooming = false)
        {
            mouseDownLocation = new Point(cusorPosX, cursorPosY);
            mouseIsPanning = panning;
            mouseIsZooming = zooming;
            Array.Copy(axis, mouseDownAxis, axis.Length);
        }

        public void MouseMoveAxis(int cursorPosX, int cursorPosY)
        {
            if (mouseIsPanning == false && mouseIsZooming == false)
                return;

            Array.Copy(mouseDownAxis, axis, axis.Length);
            AxisUpdate();

            int dX = cursorPosX - mouseDownLocation.X;
            int dY = cursorPosY - mouseDownLocation.Y;

            if (mouseIsPanning)
                AxisPan(-dX / xAxisScale, dY / yAxisScale);
            else if (mouseIsZooming)
                AxisZoomPx(dX, -dY);
        }

        public void MouseUpAxis()
        {
            mouseIsPanning = false;
            mouseIsZooming = false;
        }

        public PointF GetPixel(double locationX, double locationY)
        {
            // Return the pixel location on the data bitmap corresponding to an X/Y location.
            // This is useful when drawing graphics on the data bitmap.
            float xPx = (float)((locationX - axis[0]) * xAxisScale);
            float yPx = dataSize.Height - (float)((locationY - axis[2]) * yAxisScale);
            return new PointF(xPx, yPx);
        }

        public PointF GetLocation(int pixelX, int pixelY)
        {
            // Return the X/Y location corresponding to a pixel position on the figure bitmap.
            // This is useful for converting a mouse position to an X/Y coordinate.
            double locationX = (pixelX - dataOrigin.X) / xAxisScale + axis[0];
            double locationY = axis[3] - (pixelY - dataOrigin.Y) / yAxisScale;
            return new PointF((float)locationX, (float)locationY);
        }

        public int GetTotalPointCount()
        {
            int totalPointCount = 0;
            foreach (Plottable plottable in plottables)
                totalPointCount += plottable.pointCount;
            return totalPointCount;
        }

        public Color GetNextColor()
        {
            string[] colors = (useTwentyColors) ? plottableColors20 : plottableColors10;
            return ColorTranslator.FromHtml(colors[plottables.Count % colors.Length]);
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
