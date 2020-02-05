using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottables
{
    public class VLine : AxisLine
    {
        public Pen pen;

        public override Cursor dragCursor => Cursor.WE;

        public VLine(double position, Color color, double lineWidth, string label,
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

            SetLimits(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
        }

        public override string ToString()
        {
            return string.Format("VLine (Y={0:0.000})", position1);
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(x1: position1, x2: position1, y1: null, y2: null);
        }

        public override void Render(DataArea dataArea)
        {
            PointF pt1, pt2;
            pt1 = dataArea.GetPixel(position1, dataArea.axisLimits.y1);
            pt2 = dataArea.GetPixel(position1, dataArea.axisLimits.y2);
            dataArea.gfxData.DrawLine(pen, pt1, pt2);
        }

        public override bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            double distanceFromMouseX = Math.Abs(position1 - coordinateX);
            return (distanceFromMouseX <= snapX);
        }

        public override void DragTo(double coordinateX, double coordinateY)
        {
            if (isDragEnabled)
            {
                coordinateX = Math.Max(coordinateX, limits.x1);
                coordinateX = Math.Min(coordinateX, limits.x2);
                position1 = coordinateX;
            }
        }

        public override void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            limits.SetX(x1: x1, x2: x2);
        }
    }
}
