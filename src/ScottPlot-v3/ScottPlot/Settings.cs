using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ScottPlot
{
    public struct XY
    {
        public double x;
        public double y;

        public XY(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Settings
    {
        // these properties get set at instantiation or after size or axis adjustments
        public Size figureSize { get; private set; }
        public Point dataOrigin { get; private set; }
        public Size dataSize { get; private set; }

        // axis (replace with class)
        public double[] axis = new double[] { -10, 10, -10, 10 }; // X1, X2, Y1, Y2
        public double xAxisSpan;
        public double yAxisSpan;
        public double xAxisCenter;
        public double yAxisCenter;
        public double xAxisScale;
        public double yAxisScale;
        public int xAxisOffset;
        public int yAxisOffset;

        // mouse adjustments
        public Point mouseDownLocation = new Point(0, 0);
        public double[] mouseDownAxis = new double[4];

        // assigned default values
        public int[] dataPadding = new int[] { 140, 120, 140, 55 }; // X1, X2, Y1, Y2
        public Color figureBackgroundColor = defaultBackgroundColor;

        // default colors
        public readonly static Color defaultBackgroundColor = Color.LightGray;

        // useful string format
        public StringFormat sfAlignRight = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public StringFormat sfAlignCenter = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Center };

        // ticks
        public readonly Font tickFont = new Font("Segoe UI", 12);
        public List<Tick> ticksX;
        public List<Tick> ticksY;

        // PlottableScatter
        public readonly static Color defaultPlottableLineColor = Color.Black;
        public readonly static float defaultPlottableLineWidth = 1;

        // PlottableMarker
        public readonly static Color defaultPlottableMarkerColor = Color.Black;
        public readonly static float defaultPlottableMarkerSize = 10;

        // PlottableText
        public readonly static Color defaultPlottableTextColor = Color.Black;
        public readonly static float defaultPlottableTextSize = 12;
        public readonly static string defaultPlottableTextFont = "Segoe UI";

        public Settings()
        {
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
            axis[0] = (x1 == null) ? -10 : (double)x1;
            axis[1] = (x2 == null) ? 10 : (double)x2;
            axis[2] = (y1 == null) ? -10 : (double)y1;
            axis[3] = (y2 == null) ? 10 : (double)y2;
            AxisUpdate();
        }

        private void AxisUpdate()
        {
            if (figureSize != null)
            {
                xAxisSpan = axis[1] - axis[0];
                xAxisCenter = (axis[1] + axis[0]) / 2;
                yAxisSpan = axis[3] - axis[2];
                yAxisCenter = (axis[3] + axis[2]) / 2;
                xAxisScale = dataSize.Width / xAxisSpan; // px per unit
                yAxisScale = dataSize.Height / yAxisSpan; // px per unit
                xAxisOffset = dataOrigin.X;
                yAxisOffset = figureSize.Height - dataPadding[2];
            }
        }

        private void AxisPan(double? dx = null, double? dy = null)
        {
            if (dx != null && dx != 0)
            {
                axis[0] += (double)dx;
                axis[1] += (double)dx;
            }

            if (dy != null && dy != 0)
            {
                axis[2] += (double)dy;
                axis[3] += (double)dy;
            }

            AxisUpdate();
        }

        private void AxisPan(int dX, int dY)
        {
            AxisPan(-dX / xAxisScale, dY / yAxisScale);
        }

        private void AxisZoom(double xFrac = 1, double yFrac = 1)
        {
            if (xFrac != 1)
            {
                double halfNewSpan = xAxisSpan / xFrac / 2;
                axis[0] = xAxisCenter - halfNewSpan;
                axis[1] = xAxisCenter + halfNewSpan;
            }

            if (yFrac != 1)
            {
                double halfNewSpan = yAxisSpan / yFrac / 2;
                axis[2] = yAxisCenter - halfNewSpan;
                axis[3] = yAxisCenter + halfNewSpan;
            }

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
            int xPx = (int)((x - axis[0]) * xAxisScale) + xAxisOffset;
            int yPx = yAxisOffset - (int)((y - axis[2]) * yAxisScale);
            return new Point(xPx, yPx);
        }

    }
}
