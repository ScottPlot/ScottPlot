using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ScottPlot
{
    public class PlottableScatter : Plottable
    {
        public double[] xs;
        public double[] ys;
        public float markerSize;
        public Pen pen;
        public Brush brush;

        public PlottableScatter(double[] xs, double[] ys, Color color, float lineWidth = 1, float markerSize = 3)
        {
            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            this.xs = xs;
            this.ys = ys;
            this.markerSize = markerSize;
            pointCount = xs.Length;

            pen = new Pen(color, lineWidth);
            brush = new SolidBrush(color);
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            throw new NotImplementedException();
        }

        public override void Render(Settings settings)
        {
            Point[] points = new Point[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPoint(xs[i], ys[i]);
            settings.gfxData.DrawLines(pen, points);

            if (markerSize > 0)
                for (int i = 0; i < points.Length; i++)
                    settings.gfxData.FillEllipse(brush, points[i].X - markerSize/2, points[i].Y - markerSize / 2, markerSize, markerSize);
        }
    }

    public class PlottableMarker : Plottable
    {
        public double x, y;
        public float size;
        public Brush brush;

        public PlottableMarker(double x, double y, float size = 3, Color? color = null)
        {
            this.x = x;
            this.y = y;
            color = (color == null) ? Color.Magenta : (Color)color;
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

        public override void Render(Settings settings)
        {
            Point point = settings.GetPoint(x, y);
            settings.gfxData.FillEllipse(brush, point.X - size / 2, point.Y - size / 2, size, size);
        }

    }

    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public string text;
        public Brush brush;
        public Font font;

        public PlottableText(string text, double x, double y, float fontSize = 12, Color? color = null, string fontName = "Arial", bool bold = false)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            color = (color == null) ? Color.Magenta : (Color)color;
            brush = new SolidBrush((Color)color);
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

        public override void Render(Settings settings)
        {
            settings.gfxData.DrawString(text, font, brush, settings.GetPoint(x, y));
        }
    }
}
