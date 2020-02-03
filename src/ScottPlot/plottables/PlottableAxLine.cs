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
        public Pen pen;

        public bool horizontal { get { return !vertical; } }
        private string orientation { get { return (vertical) ? "vertical" : "horizontal"; } }

        public PlottableAxLine(double position, bool vertical, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            this.position = position;
            this.vertical = vertical;
            this.color = color;
            this.label = label;
            this.lineStyle = lineStyle;
            pointCount = 1;

            pen = new Pen(color, (float)lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                DashStyle = StyleTools.DashStyle(lineStyle),
                DashPattern = StyleTools.DashPattern(lineStyle)
            };

            DragEnable(draggable);

            if (vertical)
                DragLimit(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
            else
                DragLimit(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            return $"PlottableAxLine ({orientation}) at {position}";
        }

        public override AxisLimits2D GetLimits()
        {
            if (vertical)
                return new AxisLimits2D(position, position, double.NaN, double.NaN);
            else
                return new AxisLimits2D(double.NaN, double.NaN, position, position);
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
                pt2.X -= (float).01; // fixes a System.Drawing bug
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

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (vertical)
            {
                double distanceFromMouseX = Math.Abs(position - coordinateX);
                return (distanceFromMouseX <= snapX);
            }
            else
            {
                double distanceFromMouseY = Math.Abs(position - coordinateY);
                return (distanceFromMouseY <= snapY);
            }
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (draggingEnabled)
            {
                if (vertical)
                {
                    if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
                    if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;
                    position = coordinateX;
                }
                else
                {
                    if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                    if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;
                    position = coordinateY;
                }
            }
        }

        public Cursor GetDragCursor()
        {
            return (vertical) ? Cursor.WE : Cursor.NS;
        }
    }
}
