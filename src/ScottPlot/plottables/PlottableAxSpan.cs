using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottableAxSpan : Plottable, IDraggable
    {
        public double position1;
        public double position2;
        public bool vertical;
        public Brush brush;

        public bool horizontal { get { return !vertical; } }
        private string orientation { get { return (vertical) ? "vertical" : "horizontal"; } }

        public PlottableAxSpan(double position1, double position2, bool vertical, Color color, double alpha, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.vertical = vertical;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            brush = new SolidBrush(this.color);
            pointCount = 1;

            DragEnable(draggable);

            if (vertical)
                DragLimit(x1: dragLimitLower, x2: dragLimitUpper, y1: double.NegativeInfinity, y2: double.PositiveInfinity);
            else
                DragLimit(x1: double.NegativeInfinity, x2: double.PositiveInfinity, y1: dragLimitLower, y2: dragLimitUpper);
        }

        public override string ToString()
        {
            return $"PlottableAxSpan ({orientation}) from {position1} to {position2}";
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

            if (vertical)
            {
                topLeft = settings.GetPixel(positionMin, settings.axes.y.min);
                lowerRight = settings.GetPixel(positionMax, settings.axes.y.max);
                if (topLeft.X < 0)
                    topLeft.X = 0;
                if (lowerRight.X > settings.bmpData.Width)
                    lowerRight.X = settings.bmpData.Width;
            }
            else
            {
                topLeft = settings.GetPixel(settings.axes.x.min, positionMin);
                lowerRight = settings.GetPixel(settings.axes.x.max, positionMax);
                if (topLeft.Y > settings.bmpData.Height)
                    topLeft.Y = settings.bmpData.Height;
                if (lowerRight.Y < 0)
                    lowerRight.Y = 0;
            }

            float width = lowerRight.X - topLeft.X + 1;
            float height = topLeft.Y - lowerRight.Y + 1;
            float x = topLeft.X - 1;
            float y = lowerRight.Y - 1;

            settings.gfxData.FillRectangle(brush, x, y, width, height);
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

        private int positionUnderMouse;
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            if (vertical)
            {
                if (Math.Abs(position1 - coordinateX) <= snapX)
                    positionUnderMouse = 1;
                else if (Math.Abs(position2 - coordinateX) <= snapX)
                    positionUnderMouse = 2;
                else
                    positionUnderMouse = 0;
            }
            else
            {
                if (Math.Abs(position1 - coordinateY) <= snapY)
                    positionUnderMouse = 1;
                else if (Math.Abs(position2 - coordinateY) <= snapY)
                    positionUnderMouse = 2;
                else
                    positionUnderMouse = 0;
            }

            return (positionUnderMouse > 0);
        }

        public void DragTo(double coordinateX, double coordinateY)
        {
            if (draggingEnabled)
            {
                if (vertical)
                {
                    if (coordinateX < dragLimitX1) coordinateX = dragLimitX1;
                    if (coordinateX > dragLimitX2) coordinateX = dragLimitX2;

                    if (positionUnderMouse == 1)
                        position1 = coordinateX;
                    else if (positionUnderMouse == 2)
                        position2 = coordinateX;
                }
                else
                {
                    if (coordinateY < dragLimitY1) coordinateY = dragLimitY1;
                    if (coordinateY > dragLimitY2) coordinateY = dragLimitY2;

                    if (positionUnderMouse == 1)
                        position1 = coordinateY;
                    else if (positionUnderMouse == 2)
                        position2 = coordinateY;
                }
            }
        }

        public Cursor GetDragCursor()
        {
            return (vertical) ? Cursor.WE : Cursor.NS;
        }
    }
}
