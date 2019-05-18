using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ScottPlot
{

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

        // mouse adjustments
        public Point mouseDownLocation = new Point(0, 0);
        public double[] mouseDownAxis = new double[4];

        // assigned default values
        public int[] dataPadding = new int[] { 140, 120, 140, 55 }; // X1, X2, Y1, Y2
        public Color figureBackgroundColor = Color.White;
        public Color dataBackgroundColor = Color.White;

        // useful string formats (position indicates where their origin is)
        public StringFormat sfEast = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfNorth = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };
        public StringFormat sfSouth = new StringFormat() { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };
        public StringFormat sfWest = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        public StringFormat sfNorthWest = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };

        // axis labels
        public bool[] axisFrame = new bool[] { true, true, true, true };
        public readonly Pen axisFramePen = Pens.Black;
        public int axisPadding = 5;

        // title
        public readonly string title = "ScottPlot Title";
        public readonly Font titleFont = new Font("Segoe UI", 20, FontStyle.Bold);
        public readonly Brush titleFontBrush = Brushes.Black;

        // axis labels
        public readonly string axisLabelX = "horizontal goodness";
        public readonly string axisLabelY = "vertical awesome";
        public readonly Font axisLabelFont = new Font("Segoe UI", 16);
        public readonly Brush axisLabelBrush = Brushes.Black;

        // axis ticks
        public readonly Font tickFont = new Font("Segoe UI", 10);
        public List<Tick> ticksX;
        public List<Tick> ticksY;
        public int tickSize = 5;
        public bool displayTicks = true;

        // frame
        public bool dataFrame = true;
        public Color dataFrameColor = Color.Black;

        // debugging
        public readonly Font debugFont = new Font("Consolas", 8);
        public readonly Brush debugFontBrush = Brushes.Black;
        public readonly Brush debugBackgroundBrush = new SolidBrush(Color.FromArgb(150, Color.LightYellow));
        public readonly Pen debugBorderPen = Pens.Black;

        // anti-aliasing
        public bool antiAliasFigureDrawings = false;
        public bool antiAliasFigureText = true;
        public bool antiAliasDataDrawings = false;
        public bool antiAliasDataText = true;

        // benchmarking
        public readonly Stopwatch stopwatch = new Stopwatch();
        public double benchmarkMsec;
        public double benchmarkHz;
        public string benchmarkMessage;
        public bool displayBenchmark = true;

        // plottables
        public readonly List<Plottable> plottables = new List<Plottable>();

        // grid
        public bool displayGrid = true;
        public Pen gridPen = Pens.LightGray;

        public Settings()
        {
        }

        public void BenchmarkStart()
        {
            stopwatch.Restart();
        }
        public void BenchmarkEnd()
        {
            stopwatch.Stop();
            benchmarkMsec = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            benchmarkHz = 1000.0 / benchmarkMsec;
            benchmarkMessage = "";
            benchmarkMessage += $"Full render of {plottables.Count} objects {GetTotalPointCount()} points)";
            benchmarkMessage += string.Format(" took {0:000.000} ms ({1:000.00 Hz})", benchmarkMsec, benchmarkHz);
            if (!backgroundRenderNeeded)
                benchmarkMessage = benchmarkMessage.Replace("Full", "Data-only");
        }

        public void Resize(int width, int height)
        {
            figureSize = new Size(width, height);
            dataOrigin = new Point(dataPadding[0], dataPadding[3]);
            int dataWidth = figureSize.Width - dataPadding[0] - dataPadding[1];
            int dataHeight = figureSize.Height - dataPadding[2] - dataPadding[3];
            dataSize = new Size(dataWidth, dataHeight);
            AxisUpdate();
        }

        public void AxisSet(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            bool axisChanged = false;

            if (x1 != null && (double)x1 != axis[0])
            {
                axis[0] = (double)x1;
                axisChanged = true;
            }

            if (x2 != null && (double)x2 != axis[1])
            {
                axis[1] = (double)x2;
                axisChanged = true;
            }

            if (y1 != null && (double)y1 != axis[2])
            {
                axis[2] = (double)y1;
                axisChanged = true;
            }

            if (y2 != null && (double)y2 != axis[3])
            {
                axis[3] = (double)y2;
                axisChanged = true;
            }

            if (axisChanged)
                AxisUpdate();
        }

        public void AxisTighen()
        {
            Ticks ticks = new Ticks(this);
            Size maxTickSizeHoriz = ticks.GetMaxTickSize(ticksX);
            Size maxTickSizeVert = ticks.GetMaxTickSize(ticksY);

            // top
            SizeF titleSize = gfxFigure.MeasureString(title, titleFont);
            dataPadding[3] = (int)(titleSize.Height) + axisPadding * 2;

            // bottom
            SizeF xLabelSize = gfxFigure.MeasureString(axisLabelX, axisLabelFont);
            dataPadding[2] = (int)(xLabelSize.Height) + axisPadding * 2;
            dataPadding[2] += maxTickSizeHoriz.Height;

            // left
            SizeF yLabelSize = gfxFigure.MeasureString(axisLabelY, axisLabelFont);
            dataPadding[0] = (int)(yLabelSize.Height) + axisPadding * 2;
            dataPadding[0] += maxTickSizeVert.Width;

            // right
            dataPadding[1] = axisPadding + maxTickSizeHoriz.Width / 2;
        }

        public bool backgroundRenderNeeded = true;

        private void AxisUpdate()
        {
            xAxisSpan = axis[1] - axis[0];
            xAxisCenter = (axis[1] + axis[0]) / 2;
            yAxisSpan = axis[3] - axis[2];
            yAxisCenter = (axis[3] + axis[2]) / 2;
            xAxisScale = dataSize.Width / xAxisSpan; // px per unit
            yAxisScale = dataSize.Height / yAxisSpan; // px per unit
            backgroundRenderNeeded = true;
        }

        private void AxisPan(double? dx = null, double? dy = null)
        {
            bool axisChanged = false;

            if (dx != null && dx != 0)
            {
                axis[0] += (double)dx;
                axis[1] += (double)dx;
                axisChanged = true;
            }

            if (dy != null && dy != 0)
            {
                axis[2] += (double)dy;
                axis[3] += (double)dy;
                axisChanged = true;
            }

            if (axisChanged)
                AxisUpdate();
        }

        private void AxisPan(int dX, int dY)
        {
            AxisPan(-dX / xAxisScale, dY / yAxisScale);
        }

        private void AxisZoom(double xFrac = 1, double yFrac = 1)
        {
            bool axisChanged = false;

            if (xFrac != 1)
            {
                double halfNewSpan = xAxisSpan / xFrac / 2;
                axis[0] = xAxisCenter - halfNewSpan;
                axis[1] = xAxisCenter + halfNewSpan;
                axisChanged = true;
            }

            if (yFrac != 1)
            {
                double halfNewSpan = yAxisSpan / yFrac / 2;
                axis[2] = yAxisCenter - halfNewSpan;
                axis[3] = yAxisCenter + halfNewSpan;
                axisChanged = true;
            }

            if (axisChanged)
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

        public void Validate()
        {
            if (figureSize == null || figureSize.Width < 1 || figureSize.Height < 1)
                throw new Exception("figure width and height must be greater than 0px");
            if (axis == null)
                throw new Exception("axis has not yet been initialized");
        }

        private bool mouseIsPanning = false;
        private bool mouseIsZooming = false;
        public void MouseDown(int cusorPosX, int cursorPosY, bool panning = false, bool zooming = false)
        {
            mouseDownLocation = new Point(cusorPosX, cursorPosY);
            mouseIsPanning = panning;
            mouseIsZooming = zooming;
            Array.Copy(axis, mouseDownAxis, axis.Length);
        }

        public void MouseMove(int cursorPosX, int cursorPosY)
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

        public void MouseUp()
        {
            mouseIsPanning = false;
            mouseIsZooming = false;
        }

        public Point GetPoint(double x, double y)
        {
            int xPx = (int)((x - axis[0]) * xAxisScale);
            int yPx = dataSize.Height - (int)((y - axis[2]) * yAxisScale);
            return new Point(xPx, yPx);
        }

        public int GetTotalPointCount()
        {
            int totalPointCount = 0;
            foreach (Plottable plottable in plottables)
                totalPointCount += plottable.pointCount;
            return totalPointCount;
        }


    }
}
