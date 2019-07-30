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
            bool draggable, double dragLimitLower, double dragLimitUpper, PenType penType)
        {
            this.position = position;
            this.vertical = vertical;
            this.color = color;
            this.label = label;
            this.draggable = draggable;
            this.dragLimitLower = dragLimitLower;
            this.dragLimitUpper = dragLimitUpper;
            orientation = (vertical) ? "vertical" : "horizontal";
            pen = new Pen(color, (float)lineWidth);
            pointCount = 1;

            switch (penType)
            {
                case PenType.Solid:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case PenType.Dash:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    pen.DashPattern = new float[] { 8.0F, 4.0F };
                    break;
                case PenType.DashDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case PenType.DashDotDot:
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    pen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F };
                    break;
                case PenType.Dot:
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
                pt1 = settings.GetPixel(position, settings.axis[2]);
                pt2 = settings.GetPixel(position, settings.axis[3]);
            }
            else
            {
                pt1 = settings.GetPixel(settings.axis[0], position);
                pt2 = settings.GetPixel(settings.axis[1], position);
            }

            settings.gfxData.DrawLine(pen, pt1, pt2);
        }

        public override void SaveCSV(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
