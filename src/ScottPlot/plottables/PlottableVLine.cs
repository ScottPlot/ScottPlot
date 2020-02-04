using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableVLine : Plottable, IDraggable
    {
        public double position;
        public Pen pen;

        public PlottableVLine(double position, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            this.position = position;
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

            DragEnabled = draggable;

            SetLimits(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
        }

        public override string ToString()
        {
            return $"PlottableVLine (Vertical) at {position}";
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(position, position, double.NaN, double.NaN);
        }

        public override void Render(Settings settings)
        {
            PointF pt1, pt2;

            pt1 = settings.GetPixel(position, settings.axes.y.min);
            pt2 = settings.GetPixel(position, settings.axes.y.max);

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        public bool DragEnabled { get; set; }

        private double dragLimitX1 = double.NegativeInfinity;
        private double dragLimitX2 = double.PositiveInfinity;
        public void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) dragLimitX1 = (double)x1;
            if (x2 != null) dragLimitX2 = (double)x2;
        }

        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            double distanceFromMouseX = Math.Abs(position - coordinateX);
            return (distanceFromMouseX <= snapX);
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (DragEnabled)
            {
                if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
                if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;
                position = coordinateX;
            }
        }

        public Cursor DragCursor => Cursor.WE;
    }
}
