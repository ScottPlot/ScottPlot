using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableAxSpan : Plottable
    {
        public double position1;
        public double position2;
        public bool vertical;
        public string orientation;
        public Brush brush;
        public bool draggable;
        public double dragLimitLower;
        public double dragLimitUpper;

        public PlottableAxSpan(double position1, double position2, bool vertical, Color color, double alpha, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper)
        {
            this.position1 = position1;
            this.position2 = position2;
            this.vertical = vertical;
            this.color = Color.FromArgb((int)(alpha * 255), color.R, color.G, color.B);
            this.label = label;
            this.draggable = draggable;
            this.dragLimitLower = dragLimitLower;
            this.dragLimitUpper = dragLimitUpper;
            brush = new SolidBrush(this.color);
            orientation = (vertical) ? "vertical" : "horizontal";
            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableAxSpan ({orientation}) from {position1} to {position2}";
        }

        public override double[] GetLimits()
        {
            return new double[] { 0, 0, 0, 0 };
        }

        public override void Render(Settings settings)
        {
            PointF topLeft, lowerRight;

            if (vertical)
            {
                topLeft = settings.GetPixel(position1, settings.axes.y.min);
                lowerRight = settings.GetPixel(position2, settings.axes.y.max);
            }
            else
            {
                topLeft = settings.GetPixel(settings.axes.x.min, position1);
                lowerRight = settings.GetPixel(settings.axes.x.max, position2);
            }

            float width = lowerRight.X - topLeft.X + 1;
            float height = topLeft.Y - lowerRight.Y + 1;
            float x = topLeft.X - 1;
            float y = lowerRight.Y - 1;

            settings.gfxData.FillRectangle(brush, x, y, width, height);
        }
    }
}
