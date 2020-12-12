using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot.Plottable
{
    public class ScatterPlotList : IPlottable
    {
        private readonly List<double> Xs = new List<double>();
        private readonly List<double> Ys = new List<double>();
        public int Count => Xs.Count;

        public string Label;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public Color Color = Color.Black;
        public float LineWidth = 1;
        public LineStyle LineStyle = LineStyle.Solid;
        public float MarkerSize = 3;
        public MarkerShape MarkerShape = MarkerShape.filledCircle;

        public void ValidateData(bool deep = false)
        {
            if (Xs.Count != Ys.Count)
                throw new InvalidOperationException("Xs and Ys must be same length");

            if (deep)
            {
                for (int i = 0; i < Xs.Count; i++)
                {
                    if (double.IsNaN(Xs[i]) || double.IsNaN(Ys[i]))
                        throw new InvalidOperationException("Xs and Ys cannot contain NaN");

                    if (double.IsInfinity(Xs[i]) || double.IsInfinity(Ys[i]))
                        throw new InvalidOperationException("Xs and Ys cannot contain Infinity");
                }
            }
        }

        public void Clear()
        {
            Xs.Clear();
            Ys.Clear();
        }

        public void Add(double x, double y)
        {
            Xs.Add(x);
            Ys.Add(y);
        }

        public void AddRange(double[] xs, double[] ys)
        {
            if (xs is null)
                throw new ArgumentException("xs must not be null");
            if (ys is null)
                throw new ArgumentException("ys must not be null");
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Xs.AddRange(xs);
            Ys.AddRange(ys);
        }

        public AxisLimits GetAxisLimits()
        {
            if (Count == 0)
                return new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);

            double xMin = Xs[0];
            double xMax = Xs[0];
            double yMin = Ys[0];
            double yMax = Ys[0];

            for (int i = 1; i < Count; i++)
            {
                xMin = Math.Min(xMin, Xs[i]);
                xMax = Math.Max(xMax, Xs[i]);
                yMin = Math.Min(yMin, Ys[i]);
                yMax = Math.Max(yMax, Ys[i]);
            }

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF[] points = new PointF[Count];
            for (int i = 0; i < Count; i++)
                points[i] = new PointF(dims.GetPixelX(Xs[i]), dims.GetPixelY(Ys[i]));

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var linePen = GDI.Pen(Color, LineWidth, LineStyle, true))
            {
                if (LineStyle != LineStyle.None && LineWidth > 0 && Count > 1)
                {
                    for (int i = 0; i < Count; i++)
                        Console.WriteLine(points[i]);

                    gfx.DrawLines(linePen, points);
                }

                if (MarkerShape != MarkerShape.none && MarkerSize > 0 && Count > 0)
                {
                    foreach (PointF point in points)
                        MarkerTools.DrawMarker(gfx, point, MarkerShape, MarkerSize, Color);
                }
            }
        }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = Label,
                color = Color.Black
            };
            return new LegendItem[] { singleLegendItem };
        }
    }
}
