using System;
using System.Drawing;

namespace ScottPlot
{

    public abstract class Plottable
    {
        public int pointCount = 0;
        public string label = null;

        public abstract void Render(Settings settings, Graphics gfx);
        public abstract override string ToString();

        public void Validate()
        {
            if (pointCount == 0)
                throw new System.Exception("pointCount must be >0");
        }

        public abstract double[] GetLimits();
    }

    public class PlottableScatter : Plottable
    {
        public double[] xs;
        public double[] ys;
        public Pen pen;

        public PlottableScatter(double[] xs, double[] ys, Color? lineColor = null, float? lineWidth = null)
        {
            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            this.xs = xs;
            this.ys = ys;

            lineColor = (lineColor != null) ? lineColor : Settings.defaultPlottableLineColor;
            lineWidth = (lineWidth != null) ? lineWidth : Settings.defaultPlottableLineWidth;
            pen = new Pen((Color)lineColor, (float)lineWidth);

            pointCount = xs.Length;
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            throw new NotImplementedException();
        }

        public override void Render(Settings settings, Graphics gfx)
        {
            Point[] points = new Point[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPoint(xs[i], ys[i]);
            gfx.DrawLines(pen, points);
        }
    }
    
    public class PlottableMarker : Plottable
    {
        public double x, y;
        public float size;
        public Brush brush;

        public PlottableMarker(double x, double y, float? size = null, Color? color = null)
        {
            this.x = x;
            this.y = y;
            this.size = (size != null) ? (float)size : Settings.defaultPlottableMarkerSize;
            color = (color != null) ? color : Settings.defaultPlottableTextColor;
            brush = new SolidBrush((Color)color);

            pointCount = 1;
        }
        public override string ToString()
        {
            return $"PlottableMarker size {size} at ({x}, {y})";
        }

        public override double[] GetLimits()
        {
            throw new NotImplementedException();
        }

        public override void Render(Settings settings, Graphics gfx)
        {
            Point point = settings.GetPoint(x, y);
            gfx.FillEllipse(brush, point.X - size/2, point.Y - size/2, size, size);
        }

    }

    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public string text;
        public Brush brush;
        public Font font;

        public PlottableText(string text, double x, double y, float? fontSize = null, Color? color = null, string fontName = null, bool bold = false)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            color = (color != null) ? color : Settings.defaultPlottableTextColor;
            brush = new SolidBrush((Color)color);
            fontSize = (fontSize != null) ? fontSize : Settings.defaultPlottableTextSize;
            FontStyle fontStyle = (bold == true) ? FontStyle.Bold : FontStyle.Regular;
            font = new Font(fontName, (float)fontSize, fontStyle);

            pointCount = 1;
        }

        public override string ToString()
        {
            return $"PlottableText \"{text}\" at ({x}, {y})";
        }

        public override double[] GetLimits()
        {
            throw new NotImplementedException();
        }

        public override void Render(Settings settings, Graphics gfx)
        {
            gfx.DrawString(text, font, brush, settings.GetPoint(x, y));
        }
    }
}