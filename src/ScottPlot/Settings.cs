using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* code here contains small settings objects which can be passed around instead of functions with lots of arguments */

namespace ScottPlot
{
    public class Settings
    {

        ////////////////////////////////////////////////////////////////////////////////////
        // figure geometry

        public int width;
        public int height;

        public Size Size { get { return new Size(width, height); } }
        public int centerXpx { get { return width / 2; } }
        public int centerYpx { get { return height / 2; } }

        public int minimumWidth { get { return dataPadLeft + dataPadTop + 1; } }
        public int minimumHeight { get { return dataPadTop + dataPadBottom + 1; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // data area geometry

        public int dataPadLeft = 60;
        public int dataPadRight = 30;
        public int dataPadTop = 40;
        public int dataPadBottom = 50;

        public int dataPlotWidth { get { return width - dataPadLeft - dataPadRight; } }
        public int dataPlotHeight { get { return height - dataPadTop - dataPadBottom; } }
        public int dataPlotPosLeft { get { return dataPadLeft; } }
        public int dataPlotPosRight { get { return dataPlotWidth + dataPadLeft; } }
        public int dataPlotPosTop { get { return dataPadTop; } }
        public int dataPlotPosBottom { get { return dataPadTop + dataPlotHeight; } }
        public int dataPlotCenterX { get { return dataPlotWidth / 2 + dataPadLeft; } }
        public int dataPlotCenterY { get { return dataPlotHeight / 2 + dataPadTop; } }
        public Point dataPlotOrigin { get { return new Point(dataPlotPosLeft, dataPlotPosTop); } }
        public Size dataPlotSize { get { return new Size(dataPlotWidth, dataPlotHeight); } }
        public Rectangle dataPlotRectangle { get { return new Rectangle(dataPlotOrigin, dataPlotSize); } }

        ////////////////////////////////////////////////////////////////////////////////////
        // background colors
        public Color figureBgColor = Color.White;
        public Color dataBgColor = Color.White;

        ////////////////////////////////////////////////////////////////////////////////////
        // anti-aliasing
        public bool figureAntiAliasLines = true;
        public bool figureAntiAliasText = true;
        public bool dataAntiAliasLines = true;
        public bool dataAntiAliasText = true;
        public SmoothingMode figureSmoothingMode { get { return (figureAntiAliasLines) ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed; } }
        public TextRenderingHint figureTextHint { get { return (figureAntiAliasText) ? TextRenderingHint.AntiAlias : TextRenderingHint.SingleBitPerPixelGridFit; } }
        public SmoothingMode dataSmoothingMode { get { return (dataAntiAliasLines) ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed; } }
        public TextRenderingHint dataTextHint { get { return (dataAntiAliasText) ? TextRenderingHint.AntiAlias : TextRenderingHint.SingleBitPerPixelGridFit; } }

        ////////////////////////////////////////////////////////////////////////////////////
        // axis limits and unit conversions
        public Axis axisX;
        public Axis axisY;

        ////////////////////////////////////////////////////////////////////////////////////
        // mouse tracking
        public MouseTracker mouse;

        ////////////////////////////////////////////////////////////////////////////////////
        // axis styling and labels

        public bool drawAxes = true;
        public Color tickColor = Color.Black;
        public Pen penAxisBorder { get { return new Pen(tickColor); } }
        public Pen penAxisTicks { get { return new Pen(tickColor); } }
        public Brush brushAxisTickLabels { get { return new SolidBrush(tickColor); } }

        public Color labelColor = Color.Black;
        public Brush brushLabels { get { return new SolidBrush(labelColor); } }

        public Font fontTicks = new Font("Arial", 9, FontStyle.Regular);
        public Font fontTitle = new Font("Arial", 20, FontStyle.Bold);
        public Font fontAxis = new Font("Arial", 12, FontStyle.Bold);

        public string axisLabelX = "Horizontal Axis (units)";
        public string axisLabelY = "Vertical Axis (units)";
        public string title = "Data Title";

        ////////////////////////////////////////////////////////////////////////////////////
        // grid settings
        public bool gridEnabled = true;
        public int gridWidth = 1;
        public Color gridColor = ColorTranslator.FromHtml("#E6E6E6");
        public Pen gridPen { get { return new Pen(gridColor, gridWidth); } }

        ////////////////////////////////////////////////////////////////////////////////////
        // dev settings
        public bool benchmarkShow = false;
        public Color benchmarkColor = Color.Gray;
        public Brush benchmarkBrush { get { return new SolidBrush(benchmarkColor); } }

        ////////////////////////////////////////////////////////////////////////////////////
        // not settings, but commonly used resources

        public StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center };
        public StringFormat sfLeft = new StringFormat { Alignment = StringAlignment.Near };
        public StringFormat sfRight = new StringFormat { Alignment = StringAlignment.Far };

        ////////////////////////////////////////////////////////////////////////////////////
        // be able to access data if we want
        private Data data;

        ////////////////////////////////////////////////////////////////////////////////////
        // functions

        public Settings(int pxWidth, int pxHeight, ref Data data)
        {
            axisX = new Axis(tickSpacingPx: 100);
            axisY = new Axis(tickSpacingPx: 50);
            mouse = new MouseTracker(ref axisX, ref axisY);
            this.data = data;
            Resize(pxWidth, pxHeight);
        }

        public void Resize(int newWidthPx, int newHeightPx)
        {

            if (newWidthPx < minimumWidth)
            {
                width = minimumWidth;
                Console.WriteLine("WARNING: minimum width");
            }
            else
            {
                width = newWidthPx;
            }

            if (newHeightPx < minimumHeight)
            {
                height = minimumHeight;
                Console.WriteLine("WARNING: minimum height");
            }
            else
            {
                height = newHeightPx;
            }

            AxesRecalculate();
        }

        public void AxesRecalculate()
        {
            axisX.Resize(dataPlotWidth);
            axisY.Resize(dataPlotHeight);
        }

        public void SetDataPadding(int? left, int? right, int? bottom, int? top)
        {
            dataPadLeft = left ?? dataPadLeft;
            dataPadRight = right ?? dataPadRight;
            dataPadBottom = bottom ?? dataPadBottom;
            dataPadTop = top ?? dataPadTop;
            AxesRecalculate();
        }

        public void AxesSet(double? x1, double? x2, double? y1, double? y2)
        {
            axisX.Set(x1, x2);
            axisY.Set(y1, y2);
        }

        public void AxisFit(double zoomOutX = .1, double zoomOutY = .1)
        {
            double[] axisLimits = data.GetAxisLimits();

            if (axisLimits == null)
                return;

            if (axisLimits[0] < axisLimits[1])
            {
                axisX.Set(axisLimits[0], axisLimits[1]);
                axisX.Zoom(1 - zoomOutX);
            }

            if (axisLimits[2] < axisLimits[3])
            {
                axisY.Set(axisLimits[2], axisLimits[3]);
                axisY.Zoom(1 - zoomOutY);
            }
        }

        public void AxesZoom(double xZoom = 1.5, double yZoom = 1.5)
        {
            axisX.Zoom(xZoom);
            axisY.Zoom(yZoom);
        }

        public void AxesPan(double? dx, double? dy)
        {
            if (dx != null) axisX.Pan((double)dx);
            if (dy != null) axisY.Pan((double)dy);
        }

    }
}
