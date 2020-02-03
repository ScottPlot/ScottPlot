using ScottPlot.Config;
using System;
using System.Collections.Generic;
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

            DragLimit(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            return $"PlottableHSpan (Horizontal) from {position1} to {position2}";
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

        public bool DragEnabled { get; set; }

        private double dragLimitY1 = double.NegativeInfinity;
        private double dragLimitY2 = double.PositiveInfinity;
        public void DragLimit(double? x1, double? x2, double? y1, double? y2)
        {
            if (y1 != null) dragLimitY1 = (double)y1;
            if (y2 != null) dragLimitY2 = (double)y2;
        }

        private int positionToDrag;
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateY) <= snapY || Math.Abs(position2 - coordinateY) <= snapY)
                return true;
            else
                return false;
        }

        public void StartDragFrom(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (Math.Abs(position1 - coordinateY) <= snapY)
                positionToDrag = 1;
            else if (Math.Abs(position2 - coordinateY) <= snapY)
                positionToDrag = 2;
            else
                positionToDrag = 0;
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (DragEnabled)
            {
                if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;

                if (positionToDrag == 1)
                    position1 = coordinateY;
                else if (positionToDrag == 2)
                    position2 = coordinateY;
            }
        }

        public Cursor DragCursor => Cursor.NS;
    }
}
