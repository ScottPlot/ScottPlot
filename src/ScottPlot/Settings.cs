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
        public Graphics gfxLegend;
        public Bitmap bmpFigure;
        public Bitmap bmpData;
        public Bitmap bmpLegend;

        // new config objects (https://github.com/swharden/ScottPlot/issues/120)
        public Config.Title title = new Config.Title();
        public Config.XLabel xLabel = new Config.XLabel();
        public Config.YLabel yLabel = new Config.YLabel();
        public Config.Misc misc = new Config.Misc();
        public Config.Benchmark benchmark = new Config.Benchmark();
        public Config.Grid grid = new Config.Grid();
        public Config.Colors colors = new Config.Colors();

        // axis (replace with class)
        public double[] axis = new double[] { -10, 10, -10, 10 }; // X1, X2, Y1, Y2
        public double xAxisSpan;
        public double yAxisSpan;
        public double xAxisCenter;
        public double yAxisCenter;
        public double xAxisScale;
        public double yAxisScale;
        public bool axisHasBeenIntentionallySet = false;

        // axis settings
        public int axisPadding = 5;
        public bool[] displayFrameByAxis = new bool[] { true, true, true, true };
        public int[] axisLabelPadding = new int[] { 5, 5, 5, 5 }; // X1, X2, Y1, Y2
        public bool displayAxisFrames = true;
        public bool tighteningOccurred = false;

        // axis ticks
        public Font tickFont = new Font("Segoe UI", 10);
        public TickCollection tickCollectionX;
        public TickCollection tickCollectionY;
        public int tickSize = 5;
        public Color tickColor = Color.Black;
        public bool displayTicksX = true;
        public bool displayTicksXminor = true;
        public bool tickDateTimeX = false;
        public bool displayTicksY = true;
        public bool displayTicksYminor = true;
        public bool tickDateTimeY = false;
        public bool useMultiplierNotation = true;
        public bool useOffsetNotation = true;
        public bool useExponentialNotation = true;
        public double tickSpacingX = 0;
        public double tickSpacingY = 0;

        // legend
        public Font legendFont = new Font("Segoe UI", 12);
        public Color legendFontColor = Color.Black;
        public Color legendBackColor = Color.White;
        public Color legendShadowColor = Color.FromArgb(75, 0, 0, 0);
        public Color legendFrameColor = Color.Black;
        public legendLocation legendLocation = legendLocation.none;
        public shadowDirection legendShadowDirection = shadowDirection.none;
        public Rectangle legendFrame = new Rectangle(0, 0, 1, 1);
        
        // mouse tracking
        public Point mouseDownLocation = new Point(0, 0);
        public double[] mouseDownAxis = new double[4];

        // mouse middle-click-zooming
        public Point mouseZoomDownLocation = new Point(0, 0);
        public Point mouseZoomCurrentLocation = new Point(0, 0);
        public bool mouseZoomRectangleIsHappening = false;

        // plottables
        public readonly List<Plottable> plottables = new List<Plottable>();
        public bool antiAliasData = true;
        public bool antiAliasFigure = true;
        public bool antiAliasLegend = true;

        // string formats (position indicates where their origin is)
        public StringFormat sfEast = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };
        public StringFormat sfNorthWest = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };
        public StringFormat sfNorthEast = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Far };
        public StringFormat sfSouth = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };
        public StringFormat sfSouthWest = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Near };
        public StringFormat sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };

        // experimental settings
        public bool useParallel = false;

        public Settings()
        {

        }

        public Color GetNextColor()
        {
            return colors.GetColor(plottables.Count);
        }

        public void BenchmarkStart()
        {
            benchmark.stopwatch.Restart();
        }

        public void BenchmarkEnd()
        {
            // TODO: move this functionality inside benchmark class
            benchmark.stopwatch.Stop();
            benchmark.msec = benchmark.stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            benchmark.hz = 1000.0 / benchmark.msec;
            string benchmarkMessage;
            benchmarkMessage = "";
            benchmarkMessage += string.Format("Full render of {0:n0} objects ({1:n0} points)", plottables.Count, GetTotalPointCount());
            benchmarkMessage += string.Format(" took {0:0.000} ms ({1:0.00 Hz})", benchmark.msec, benchmark.hz);
            if (plottables.Count == 1)
                benchmarkMessage = benchmarkMessage.Replace("objects", "object");
            if (!bmpFigureRenderRequired)
                benchmarkMessage = benchmarkMessage.Replace("Full", "Data-only");
            benchmark.text = benchmarkMessage;
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
            // "tighten" the plot by reducing whitespce between labels, data, and the edge of the figure

            if (tickCollectionX == null)
                return;

            int tickLetterHeight = (int)gfxFigure.MeasureString("test", tickFont).Height;

            // top
            axisLabelPadding[3] = 1;
            axisLabelPadding[3] += Math.Max((int)title.height, tickLetterHeight);
            axisLabelPadding[3] += axisPadding;

            // bottom
            int xLabelHeight = (int)gfxFigure.MeasureString(xLabel.text, xLabel.font).Height;
            axisLabelPadding[2] = Math.Max(xLabelHeight, tickLetterHeight);
            axisLabelPadding[2] += tickLetterHeight;
            axisLabelPadding[2] += axisPadding;

            // left
            SizeF yLabelSize = gfxFigure.MeasureString(yLabel.text, yLabel.font);
            axisLabelPadding[0] = (int)yLabelSize.Height;
            axisLabelPadding[0] += (int)tickCollectionY.maxLabelSize.Width;
            axisLabelPadding[0] += axisPadding;

            // right
            axisLabelPadding[1] = (int)tickCollectionY.maxLabelSize.Width / 2;
            axisLabelPadding[1] += axisPadding;

            // override for frameles
            if (!displayAxisFrames)
                axisLabelPadding = new int[] { 0, 0, 0, 0 };

            tighteningOccurred = true;
        }

        public bool bmpFigureRenderRequired = true;

        public void AxisUpdate()
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
                axis[2] = zoomCenter.Value.Y - spanTop / yFrac;
                axis[3] = zoomCenter.Value.Y + spanBot / yFrac;
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

        public void AxisAuto(double horizontalMargin = .1, double verticalMargin = .1, bool xExpandOnly = false, bool yExpandOnly = false)
        {

            double[] original = null;
            if (axis != null)
            {
                original = new double[4];
                Array.Copy(axis, 0, original, 0, 4);
            }

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

            if (xExpandOnly && original != null)
            {
                axis[0] = Math.Min(axis[0], original[0]);
                axis[1] = Math.Max(axis[1], original[1]);
                AxisUpdate();
            }

            if (yExpandOnly && original != null)
            {
                axis[2] = Math.Min(axis[2], original[2]);
                axis[3] = Math.Max(axis[3], original[3]);
                AxisUpdate();
            }
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

        public void MouseMoveAxis(int cursorPosX, int cursorPosY, bool lockVertical, bool lockHorizontal)
        {
            if (mouseIsPanning == false && mouseIsZooming == false)
                return;

            Array.Copy(mouseDownAxis, axis, axis.Length);
            AxisUpdate();

            int dX = cursorPosX - mouseDownLocation.X;
            int dY = cursorPosY - mouseDownLocation.Y;

            if (lockVertical)
                dY = 0;
            if (lockHorizontal)
                dX = 0;

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

        public void MouseZoomRectMove(Point eLocation)
        {
            mouseZoomCurrentLocation = eLocation;
            mouseZoomRectangleIsHappening = true;
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
