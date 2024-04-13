using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A polygon is a collection of X/Y points that are all connected to form a closed shape.
    /// Polygons can be optionally filled with a color or a gradient.
    /// </summary>
    public class Polygon : IPlottable, IHasColor
    {
        // data
        public double[] Xs { get; set; }
        public double[] Ys { get; set; }

        // configuration
        public string Label { get; set; }
        public double LineWidth { get; set; } = 1;
        public Color LineColor { get; set; } = Color.Black;
        public bool Fill { get; set; } = true;
        public Color FillColor { get; set; } = Color.Gray;
        public Color Color { get => FillColor; set { FillColor = value; } }
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public Color HatchColor { get; set; } = Color.Transparent;
        public HatchStyle HatchStyle { get; set; } = HatchStyle.None;

        public Polygon(double[] xs, double[] ys)
        {
            Xs = xs;
            Ys = ys;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottablePolygon{label} with {PointCount} points";
        }

        public int PointCount { get => Xs.Length; }

        public AxisLimits GetAxisLimits()
        {
            double xMin = Xs[0];
            double xMax = Xs[0];
            double yMin = Ys[0];
            double yMax = Ys[0];

            for (int i = 1; i < Xs.Length; i++)
            {
                xMin = Math.Min(xMin, Xs[i]);
                xMax = Math.Max(xMax, Xs[i]);
                yMin = Math.Min(yMin, Ys[i]);
                yMax = Math.Max(yMax, Ys[i]);
            }

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Fill ? FillColor : LineColor,
                lineWidth = Fill ? 10 : LineWidth,
                markerShape = MarkerShape.none,
                hatchColor = HatchColor,
                hatchStyle = HatchStyle
            };
            return LegendItem.Single(singleItem);
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("xs", Xs);
            Validate.AssertHasElements("ys", Ys);
            Validate.AssertEqualLength("xs and ys", Xs, Ys);

            if (Xs.Length < 3)
                throw new InvalidOperationException("polygons must contain at least 3 points");

            if (deep)
            {
                Validate.AssertAllReal("xs", Xs);
                Validate.AssertAllReal("ys", Ys);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF[] points = new PointF[Xs.Length];
            for (int i = 0; i < Xs.Length; i++)
                points[i] = new PointF(dims.GetPixelX(Xs[i]), dims.GetPixelY(Ys[i]));

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Brush fillBrush = GDI.Brush(FillColor, HatchColor, HatchStyle))
            using (Pen outlinePen = GDI.Pen(LineColor, (float)LineWidth))
            {
                if (Fill)
                    gfx.FillPolygon(fillBrush, points);

                if (LineWidth > 0)
                    gfx.DrawPolygon(outlinePen, points);
            }
        }
    }
}
