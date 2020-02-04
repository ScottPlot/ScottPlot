using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottables
{
    public class HSpan : AxisSpan
    {
        public override Cursor dragCursor => Cursor.WE;
        public Brush brush;

        public HSpan(double position1, double position2, Color color, double alpha, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            brush = new SolidBrush(this.color);

            isDragEnabled = draggable;

            limits.SetXY(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
        }

        public override string ToString()
        {
            return String.Format("HSpan (X1={0:0.000}, X2={0:0.000})", position1, position2);
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

            topLeft = settings.GetPixel(positionMin, settings.axes.y.min);
            lowerRight = settings.GetPixel(positionMax, settings.axes.y.max);
            if (topLeft.X < 0)
                topLeft.X = 0;
            if (lowerRight.X > settings.bmpData.Width)
                lowerRight.X = settings.bmpData.Width;

            float width = lowerRight.X - topLeft.X + 1;
            float height = topLeft.Y - lowerRight.Y + 1;
            float x = topLeft.X - 1;
            float y = lowerRight.Y - 1;

            settings.gfxData.FillRectangle(brush, x, y, width, height);
        }

        public override void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            limits.SetX(x1, x2);
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;
        public override bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateX) <= snapX)
                edgeUnderMouse = Edge.Edge1;
            else if (Math.Abs(position2 - coordinateX) <= snapX)
                edgeUnderMouse = Edge.Edge2;
            else
                edgeUnderMouse = Edge.Neither;

            return (edgeUnderMouse == Edge.Neither) ? false : true;
        }

        public override void DragTo(double coordinateX, double coordinateY)
        {
            if (isDragEnabled)
            {
                coordinateX = Math.Max(coordinateX, limits.x1);
                coordinateX = Math.Min(coordinateX, limits.x2);

                if (edgeUnderMouse == Edge.Edge1)
                    position1 = coordinateX;
                else if (edgeUnderMouse == Edge.Edge2)
                    position2 = coordinateX;
                else
                    Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }
        }
    }
}
