using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableHSpan : Plottable, IDraggable
    {
        public double position1;
        public double position2;
        public Brush brush;

        public PlottableHSpan(double position1, double position2, Color color, double alpha, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            brush = new SolidBrush(this.color);
            pointCount = 1;

            DragEnabled = draggable;

            SetLimits(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
        }

        public override string ToString()
        {
            return $"PlottableVSpan from X={position1} to X={position2}";
        }

        public override AxisLimits2D GetLimits()
        {
            // TODO: use real numbers (and double.NaN)
            return new AxisLimits2D();
        }

        public override void Render(Settings settings)
        {
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

        public bool DragEnabled { get; set; }

        private double dragLimitX1 = double.NegativeInfinity;
        private double dragLimitX2 = double.PositiveInfinity;
        public void SetLimits(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) dragLimitX1 = (double)x1;
            if (x2 != null) dragLimitX2 = (double)x2;
        }

        private enum Edge { Edge1, Edge2, Neither };
        Edge edgeUnderMouse = Edge.Neither;
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateX) <= snapX)
                edgeUnderMouse = Edge.Edge1;
            else if (Math.Abs(position2 - coordinateX) <= snapX)
                edgeUnderMouse = Edge.Edge2;
            else
                edgeUnderMouse = Edge.Neither;

            return (edgeUnderMouse == Edge.Neither) ? false : true;
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (DragEnabled)
            {
                if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
                if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;

                if (edgeUnderMouse == Edge.Edge1)
                    position1 = coordinateX;
                else if (edgeUnderMouse == Edge.Edge2)
                    position2 = coordinateX;
                else
                    Debug.WriteLine("DragTo() called but no side selected. Call IsUnderMouse() to select a side.");
            }
        }

        public Cursor DragCursor => Cursor.WE;
    }
}
