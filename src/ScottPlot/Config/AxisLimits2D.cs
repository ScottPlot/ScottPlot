using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
    public class AxisLimits2D
    {
        public double x1 { get; private set; }
        public double x2 { get; private set; }
        public double y1 { get; private set; }
        public double y2 { get; private set; }

        //public double[] Limits { get { return new double[] { x1, x2, y1, y2 }; } }
        //public (double, double, double, double) Limits { get { return (x1, x2, y1, y2); } }

        public double xSpan { get { return x2 - x1; } }
        public double ySpan { get { return y2 - y1; } }

        public AxisLimits2D()
        {
            x1 = double.NaN;
            x2 = double.NaN;
            y1 = double.NaN;
            y2 = double.NaN;
        }

        public AxisLimits2D(AxisLimits2D source)
        {
            x1 = source.x1;
            x2 = source.x2;
            y1 = source.y1;
            y2 = source.y2;
        }

        public AxisLimits2D(double? x1, double? x2, double? y1, double? y2)
        {
            if (x1 != null) this.x1 = (double)x1;
            if (x2 != null) this.x2 = (double)x2;
            if (y1 != null) this.y1 = (double)y1;
            if (y1 != null) this.y2 = (double)y2;
        }

        public AxisLimits2D(double[] limits)
        {
            if (limits == null || limits.Length != 4)
                throw new ArgumentException();

            x1 = limits[0];
            x2 = limits[1];
            y1 = limits[2];
            y2 = limits[3];
        }

        public override string ToString()
        {
            return string.Format("x1={0:0.000}, x2={0:0.000}, y1={0:0.000}, y2={0:0.000}", x1, x2, y1, y2);
        }

        public void SetX(double x1, double x2)
        {
            this.x1 = x1;
            this.x2 = x2;
        }

        public void SetX(AxisLimits2D source)
        {
            SetX(source.x1, source.x2);
        }

        public void SetY(double y1, double y2)
        {
            this.y1 = y1;
            this.y2 = y2;
        }

        public void SetY(AxisLimits2D source)
        {
            SetY(source.y1, source.y2);
        }

        public void SetXY(double x1, double x2, double y1, double y2)
        {
            SetX(x1, x2);
            SetY(y1, y2);
        }

        public void ExpandX(double x1, double x2)
        {
            if (!double.IsNaN(x1))
            {
                if (double.IsNaN(this.x1))
                    this.x1 = x1;
                else if (x1 < this.x1)
                    this.x1 = x1;
            }

            if (!double.IsNaN(x2))
            {
                if (double.IsNaN(this.x2))
                    this.x2 = x2;
                else if (x2 > this.x2)
                    this.x2 = x2;
            }
        }

        public void ExpandX(AxisLimits2D source)
        {
            ExpandX(source.x1, source.x2);
        }

        public void ExpandY(double y1, double y2)
        {
            if (!double.IsNaN(y1))
            {
                if (double.IsNaN(this.y1))
                    this.y1 = y1;
                else if (y1 < this.y1)
                    this.y1 = y1;
            }

            if (!double.IsNaN(y2))
            {
                if (double.IsNaN(this.y2))
                    this.y2 = y2;
                else if (y2 > this.y2)
                    this.y2 = y2;
            }
        }

        public void ExpandY(AxisLimits2D source)
        {
            ExpandY(source.y1, source.y2);
        }

        public void ExpandXY(double x1, double x2, double y1, double y2)
        {
            ExpandX(x1, x2);
            ExpandY(y1, y2);
        }

        public void ExpandXY(AxisLimits2D source)
        {
            ExpandX(source.x1, source.x2);
            ExpandY(source.y1, source.y2);
        }

        public void MakeRational()
        {
            if (double.IsNaN(x1)) x1 = 0;
            if (double.IsNaN(x2)) x2 = 0;
            if (double.IsNaN(y1)) y1 = 0;
            if (double.IsNaN(y2)) y2 = 0;

            double padding = 1.5;

            if (x1 == x2)
            {
                x1 -= padding;
                x2 += padding;
            }

            if (y1 == y2)
            {
                y1 -= padding;
                y2 += padding;
            }
        }
    }
}
