using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableAxLine : Plottable
    {
        public double position;
        public bool vertical;
        public string orientation;
        public Pen pen;
        public bool draggable;
        public double dragLimitLower;
        public double dragLimitUpper;

        public PlottableAxLine(double position, bool vertical, Color color, double lineWidth, string label,
            bool draggable, double dragLimitLower, double dragLimitUpper, LineStyle lineStyle)
        {
            this.position = position;
            this.vertical = vertical;
            this.color = color;
            this.label = label;
            this.draggable = draggable;
            this.dragLimitLower = dragLimitLower;
            this.dragLimitUpper = dragLimitUpper;
            this.lineStyle = lineStyle;
            orientation = (vertical) ? "vertical" : "horizontal";
            pen = new Pen(color, (float)lineWidth);
            pointCount = 1;

            switch (lineStyle)
            {
                case LineStyle.Solid:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case LineStyle.Dash:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    pen.DashPattern = new float[] { 8.0F, 4.0F };
                    break;
                case LineStyle.DashDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case LineStyle.DashDotDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case LineStyle.Dot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    pen.DashPattern = new float[] { 2.0F, 4.0F };
                    break;

            }
        }

        public override string ToString()
        {
            return $"PlottableAxLine ({orientation}) at {position}";
        }

        public override double[] GetLimits()
        {
            return new double[] { 0, 0, 0, 0 };
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
            }

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        public override void SaveCSV(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
