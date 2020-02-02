using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableAxLine : Plottable, IDraggable
    {
        public double position;
        public bool vertical;
        public string orientation;
        public Pen pen;

        public bool horizontal { get { return !vertical; } }

        public PlottableAxLine(double position, bool vertical, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            this.position = position;
            this.vertical = vertical;
            this.color = color;
            this.label = label;
            this.lineStyle = lineStyle;
            orientation = (vertical) ? "vertical" : "horizontal";
            pen = new Pen(color, (float)lineWidth);
            pointCount = 1;

            DragEnable(draggable);

            if (vertical)
                DragLimit(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
            else
                DragLimit(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);

            switch (lineStyle)
            {
                case LineStyle.Solid:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case LineStyle.Dash:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    pen.DashPattern = new float[] { 8.0F, 4.0F };
                    break;
                case LineStyle.DashDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case LineStyle.DashDotDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case LineStyle.Dot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    pen.DashPattern = new float[] { 2.0F, 4.0F };
                    break;

            }
        }

        public override string ToString()
        {
            return $"PlottableAxLine ({orientation}) at {position}";
        }

        public override double[] GetLimits()
        {
            return new double[] { 0, 0, 0, 0 };
        }

        public override void Render(Settings settings)
        {
            PointF pt1, pt2;

            if (vertical)
            {
                pt1 = settings.GetPixel(position, settings.axes.y.min);
                pt2 = settings.GetPixel(position, settings.axes.y.max);
            }
            else
            {
                pt1 = settings.GetPixel(settings.axes.x.min, position);
                pt2 = settings.GetPixel(settings.axes.x.max, position);
            }

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        private bool draggingEnabled;
        public void DragEnable(bool enable)
        {
            draggingEnabled = enable;
        }

        private double dragLimitX1 = double.NegativeInfinity;
        private double dragLimitX2 = double.PositiveInfinity;
        private double dragLimitY1 = double.NegativeInfinity;
        private double dragLimitY2 = double.PositiveInfinity;
        public void DragLimit(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) dragLimitX1 = (double)x1;
            if (x2 != null) dragLimitX2 = (double)x2;
            if (y1 != null) dragLimitY1 = (double)y1;
            if (y2 != null) dragLimitY2 = (double)y2;
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapDistanceX, double snapDistanceY)
        {
            if (vertical)
            {
                double distanceFromMouseX = Math.Abs(position - coordinateX);
                return (distanceFromMouseX <= snapDistanceX);
            }
            else
            {
                double distanceFromMouseY = Math.Abs(position - coordinateY);
                return (distanceFromMouseY <= snapDistanceY);
            }
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            position = vertical ? coordinateX : coordinateY;
        }

        public Cursor GetDragCursor()
        {
            return (vertical) ? Cursor.WE : Cursor.NS;
        }
    }
}
