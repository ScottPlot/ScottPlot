using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Data;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Bar plots display a series of bars. 
    /// Positions are defined by Xs.
    /// Heights are defined by Ys (relative to BaseValue and YOffsets).
    /// </summary>
    public class BarPlot : BarPlotBase, IPlottable
    {
        // customization
        public string Label;
        public Color FillColor = Color.Green;
        public Color FillColorNegative = Color.Red;
        public Color FillColorHatch = Color.Blue;
        public HatchStyle HatchStyle = HatchStyle.None;
        public float BorderLineWidth = 1;

        public BarPlot(double[] xs, double[] ys, double[] yErr, double[] yOffsets) : base()
        {
            if (ys is null || ys.Length == 0)
                throw new InvalidOperationException("ys must be an array that contains elements");

            Ys = ys;
            Xs = xs ?? DataGen.Consecutive(ys.Length);
            YErrors = yErr ?? DataGen.Zeros(ys.Length);
            YOffsets = yOffsets ?? DataGen.Zeros(ys.Length);
        }

        protected override void RenderBarFromRect(RectangleF rect, bool negative, Graphics gfx)
        {
            using (var outlinePen = new Pen(BorderColor, BorderLineWidth))
            using (var fillBrush = GDI.Brush(negative ? FillColorNegative : FillColor, FillColorHatch, HatchStyle))
            {
                gfx.FillRectangle(fillBrush, rect.X, rect.Y, rect.Width, rect.Height);
                if (BorderLineWidth > 0)
                {
                    gfx.DrawRectangle(outlinePen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableBar{label} with {PointCount} points";
        }

        public int PointCount { get => Ys is null ? 0 : Ys.Length; }

        public override LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem()
            {
                label = Label,
                color = FillColor,
                lineWidth = 10,
                markerShape = MarkerShape.none,
                hatchColor = FillColorHatch,
                hatchStyle = HatchStyle,
                borderColor = BorderColor,
                borderWith = BorderLineWidth
            };
            return new LegendItem[] { singleItem };
        }
    }
}
