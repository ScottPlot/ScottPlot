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
        // these properties get set at instantiation or after size or axes adjustments
        public Size figureSize { get; private set; }
        public Point dataOrigin { get; private set; }
        public Size dataSize { get; private set; }

        // axes (replace with class)
        public double[] axes = new double[] { -10, 10, -10, 10 }; // X1, X2, Y1, Y2
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
        public double[] mouseDownAxes = new double[4];

        // assigned default values
        public int[] dataPadding = new int[] { 140, 120, 140, 55 }; // left, right, bottom, top
        public Color figureBackgroundColor = defaultBackgroundColor;

        // default colors
        public readonly static Color defaultBackgroundColor = Color.LightGray;

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
            AxesUpdate();
        }

        public void AxesSet(double? x1 = null, double? x2 = null, double? y1 = null, double? y2 = null)
        {
            axes[0] = (x1 == null) ? -10 : (double)x1;
            axes[1] = (x2 == null) ? 10 : (double)x2;
            axes[2] = (y1 == null) ? -10 : (double)y1;
            axes[3] = (y2 == null) ? 10 : (double)y2;
            AxesUpdate();
        }

        private void AxesUpdate()
        {
            if (figureSize != null)
            {
                xAxisSpan = axes[1] - axes[0];
                xAxisCenter = (axes[1] + axes[0]) / 2;
                yAxisSpan = axes[3] - axes[2];
                yAxisCenter = (axes[3] + axes[2]) / 2;
                xAxisScale = dataSize.Width / xAxisSpan;
                yAxisScale = dataSize.Height / yAxisSpan;
                xAxisOffset = dataOrigin.X;
                yAxisOffset = figureSize.Height - dataPadding[2];
            }
        }

        private void AxesPan(double? dx = null, double? dy = null)
        {
            if (dx != null && dx != 0)
            {
                axes[0] += (double)dx;
                axes[1] += (double)dx;
            }

            if (dy != null && dy != 0)
            {
                axes[2] += (double)dy;
                axes[3] += (double)dy;
            }

            AxesUpdate();
        }

        private void AxesPanPx(int dX, int dY)
        {
            AxesPan(-dX / xAxisScale, dY / yAxisScale);
        }

        private void AxesZoom(double xFrac = 1, double yFrac = 1)
        {
            if (xFrac != 1)
            {
                double halfNewSpan = xAxisSpan / xFrac / 2;
                axes[0] = xAxisCenter - halfNewSpan;
                axes[1] = xAxisCenter + halfNewSpan;
            }

            if (yFrac != 1)
            {
                double halfNewSpan = yAxisSpan / yFrac / 2;
                axes[2] = yAxisCenter - halfNewSpan;
                axes[3] = yAxisCenter + halfNewSpan;
            }

            AxesUpdate();
        }

        private void AxesZoomPx(int xPx, int yPx)
        {
            double dX = (double)xPx / xAxisScale;
            double dY = (double)yPx / yAxisScale;
            double dXFrac = dX / (Math.Abs(dX) + xAxisSpan);
            double dYFrac = dY / (Math.Abs(dY) + yAxisSpan);
            AxesZoom(Math.Pow(10, dXFrac), Math.Pow(10, dYFrac));
        }

        public void Validate()
        {
            if (figureSize == null || figureSize.Width < 1 || figureSize.Height < 1)
                throw new Exception("figure width and height must be greater than 0px");
            if (axes == null)
                throw new Exception("axes have not yet been initialized");
        }

        private bool mouseIsPanning = false;
        private bool mouseIsZooming = false;
        public void MouseDown(int cusorPosX, int cursorPosY, bool panning = false, bool zooming = false)
        {
            mouseDownLocation = new Point(cusorPosX, cursorPosY);
            mouseIsPanning = panning;
            mouseIsZooming = zooming;
            Array.Copy(axes, mouseDownAxes, axes.Length);
        }

        public void MouseMove(int cursorPosX, int cursorPosY)
        {
            if (mouseIsPanning == false && mouseIsZooming == false)
                return;

            Array.Copy(mouseDownAxes, axes, axes.Length);
            AxesUpdate();

            int dX = cursorPosX - mouseDownLocation.X;
            int dY = cursorPosY - mouseDownLocation.Y;

            if (mouseIsPanning)
                AxesPan(-dX / xAxisScale, dY / yAxisScale);
            else if (mouseIsZooming)
                AxesZoomPx(dX, -dY);
        }

        public void MouseUp()
        {
            mouseIsPanning = false;
            mouseIsZooming = false;
        }

        public Point GetPoint(double x, double y)
        {
            int xPx = (int)((x - axes[0]) * xAxisScale) + xAxisOffset;
            int yPx = yAxisOffset - (int)((y - axes[2]) * yAxisScale);
            return new Point(xPx, yPx);
        }

    }
}
