using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottables
{
    public class VSpan : AxisSpan
    {
        public override Cursor dragCursor => Cursor.NS;
        public Brush brush;

        public VSpan(double position1, double position2, Color color, double alpha, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            brush = new SolidBrush(this.color);

            isDragEnabled = draggable;

            SetLimits(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            return String.Format("VSpan (Y1={0:0.000}, Y2={0:0.000})", position1, position2);
        }

        public override AxisLimits2D GetLimits()
        {
            // TODO: use real numbers (and double.NaN)
            return new AxisLimits2D();
        }

        public override void Render(Context renderContext)
        {
            var settings = renderContext.settings;

            PointF topLeft, lowerRight;

            double positionMin = Math.Min(position1, position2);
            double positionMax = Math.Max(position1, position2);

            topLeft = settings.GetPixel(settings.axes.x.min, positionMin);
            lowerRight = settings.GetPixel(settings.axes.x.max, positionMax);
            if (topLeft.Y > settings.bmpData.Height)
                topLeft.Y = settings.bmpData.Height;
            if (lowerRight.Y < 0)
                lowerRight.Y = 0;

            float width = lowerRight.X - topLeft.X + 1;
            float height = topLeft.Y - lowerRight.Y + 1;
            float x = topLeft.X - 1;
            float y = lowerRight.Y - 1;

            settings.gfxData.FillRectangle(brush, x, y, width, height);
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;
        public override bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateY) <= snapY)
                edgeUnderMouse = Edge.Edge1;
            else if (Math.Abs(position2 - coordinateY) <= snapY)
                edgeUnderMouse = Edge.Edge2;
            else
                edgeUnderMouse = Edge.Neither;

            return (edgeUnderMouse == Edge.Neither) ? false : true;
        }

        public override void DragTo(double coordinateX, double coordinateY)
        {
            if (isDragEnabled)
            {
                coordinateY = Math.Max(coordinateY, limits.y1);
                coordinateY = Math.Min(coordinateY, limits.y2);

                if (edgeUnderMouse == Edge.Edge1)
                    position1 = coordinateY;
                else if (edgeUnderMouse == Edge.Edge2)
                    position2 = coordinateY;
                else
                    Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }
        }

        public override void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            limits.SetY(y1, y2);
        }
    }
}
