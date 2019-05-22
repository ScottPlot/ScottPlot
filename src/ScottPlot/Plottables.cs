using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ScottPlot
{

    internal class PlottableSignal : Plottable
    {
        public double[] ys;
        public double sampleRate;
        public double samplePeriod;
        public Pen pen;
        public bool antiAlias;

        public PlottableSignal(double[] ys, double sampleRate, Color color, double linewidth, bool antiAlias)
        {
            this.ys = ys;
            this.sampleRate = sampleRate;
            this.samplePeriod = 1.0 / sampleRate;
            this.antiAlias = antiAlias;
            pointCount = ys.Length;

            pen = new Pen(color, (float)linewidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };
        }

        public override string ToString()
        {
            return $"PlottableSignal with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = 0;
            limits[1] = samplePeriod * ys.Length;
            limits[2] = ys.Min();
            limits[3] = ys.Max();
            return limits;
        }

        public override void Render(Settings settings)
        {
            
            double dataSpanUnits = ys.Length * samplePeriod;
            double columnSpanUnits = settings.xAxisSpan / settings.dataSize.Width;
            double columnPointCount = (columnSpanUnits / dataSpanUnits) * ys.Length;
            double offsetUnits = settings.axis[0];
            double offsetPoints = offsetUnits / samplePeriod;

            List<Point> points = new List<Point>(settings.dataSize.Width * 2 + 1);
            for (int xPx = 0; xPx < settings.dataSize.Width; xPx++)
            {
                // determine data indexes for this pixel column
                int index1 = (int)(offsetPoints + columnPointCount * xPx);
                int index2 = (int)(offsetPoints + columnPointCount * (xPx + 1));

                // skip invalid data index values
                if ((index2 < 0) || (index1 > ys.Length - 1))
                    continue;
                if (index1 < 0)
                    index1 = 0;
                if (index2 > ys.Length - 1)
                    index2 = ys.Length - 1;

                // get the min and max value for this column
                double lowestValue = ys[index1];
                double highestValue = ys[index1];
                for (int i = index1; i < index2; i++)
                {
                    if (ys[i] < lowestValue)
                        lowestValue = ys[i];
                    if (ys[i] > highestValue)
                        highestValue = ys[i];
                }
                int yPxHigh = settings.GetPoint(0, lowestValue).Y;
                int yPxLow = settings.GetPoint(0, highestValue).Y;

                // adjust order of points to enhance anti-aliasing
                if ((points.Count < 2) || (yPxLow < points[points.Count - 1].Y))
                {
                    points.Add(new Point(xPx, yPxLow));
                    points.Add(new Point(xPx, yPxHigh));
                }
                else
                {
                    points.Add(new Point(xPx, yPxHigh));
                    points.Add(new Point(xPx, yPxLow));
                }
            }

            var originalAntiAliasingMode = settings.gfxData.SmoothingMode;
            if (antiAlias)
                settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            else
                settings.gfxData.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            if (points.Count > 0)
                settings.gfxData.DrawLines(pen, points.ToArray());

            settings.gfxData.SmoothingMode = originalAntiAliasingMode;

        }
    }


    internal class PlottableScatter : Plottable
    {
        public double[] xs;
        public double[] ys;
        public float markerSize;
        public Pen pen;
        public Brush brush;

        public PlottableScatter(double[] xs, double[] ys, Color color, float lineWidth, float markerSize)
        {
            if (xs.Length != ys.Length)
                throw new Exception("Xs and Ys must have same length");

            this.xs = xs;
            this.ys = ys;
            this.markerSize = markerSize;
            pointCount = xs.Length;

            pen = new Pen(color, lineWidth)
            {
                // this prevents sharp corners
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round,
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round
            };

            brush = new SolidBrush(color);
        }

        public override string ToString()
        {
            return $"PlottableScatter with {pointCount} points";
        }

        public override double[] GetLimits()
        {
            double[] limits = new double[4];
            limits[0] = xs.Min();
            limits[1] = xs.Max();
            limits[2] = ys.Min();
            limits[3] = ys.Max();
            return limits;
        }

        public override void Render(Settings settings)
        {
            Point[] points = new Point[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                points[i] = settings.GetPoint(xs[i], ys[i]);

            if (pen.Width > 0)
                settings.gfxData.DrawLines(pen, points);

            if (markerSize > 0)
                for (int i = 0; i < points.Length; i++)
                    settings.gfxData.FillEllipse(brush, points[i].X - markerSize / 2, points[i].Y - markerSize / 2, markerSize, markerSize);

        }
    }

    public class PlottableText : Plottable
    {
        public double x;
        public double y;
        public string text;
        public Brush brush;
        public Font font;

        public PlottableText(string text, double x, double y, Color color, string fontName = "Arial", float fontSize = 12, bool bold = false)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            brush = new SolidBrush(color);
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
            return new double[] { x, x, y, y };
        }

        public override void Render(Settings settings)
        {
            settings.gfxData.DrawString(text, font, brush, settings.GetPoint(x, y));
        }
    }
}
