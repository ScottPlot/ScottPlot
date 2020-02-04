using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottables
{
    public class HLine : AxisLine
    {
        public Pen pen;
        public override Cursor dragCursor => Cursor.NS;

        public HLine(double position, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            position1 = position;
            this.color = color;
            this.label = label;
            this.lineStyle = lineStyle;

            pen = new Pen(color, (float)lineWidth)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round,
                DashStyle = StyleTools.DashStyle(lineStyle),
                DashPattern = StyleTools.DashPattern(lineStyle)
            };

            isDragEnabled = draggable;

            SetLimits(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            return string.Format("HLine (X={0:0.000})", position1);
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(x1: null, x2: null, y1: position1, y2: position1);
        }

        public override void Render(Settings settings)
        {
            PointF pt1, pt2;
            pt1 = settings.GetPixel(settings.axes.x.min, position1);
            pt2 = settings.GetPixel(settings.axes.x.max, position1);
            pt2.X -= (float).01; // fixes a System.Drawing bug
            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        public override bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            double distanceFromMouseY = Math.Abs(position1 - coordinateY);
            return (distanceFromMouseY <= snapY);
        }

        public override void DragTo(double coordinateX, double coordinateY)
        {
            if (isDragEnabled)
            {
                coordinateY = Math.Max(coordinateY, limits.y1);
                coordinateY = Math.Min(coordinateY, limits.y2);
                position1 = coordinateY;
            }
        }

        public override void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            limits.SetY(y1: y1, y2: y2);
        }
    }
}
