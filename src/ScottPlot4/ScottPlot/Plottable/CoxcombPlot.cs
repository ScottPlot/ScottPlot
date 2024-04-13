using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A Pie chart where the angle of slices is constant but the radii are not.
    /// </summary>
    public class CoxcombPlot : IPlottable
    {
        private double[] _values;

        /// <summary>
        /// The data to be plotted
        /// </summary>
        public double[] Values
        {
            get => _values;
            set
            {
                _values = value;
                Normalized = Normalize(value);
            }
        }
        /// <summary>
        /// The colors of each slice
        /// </summary>
        public Color[] FillColors { get; set; }

        /// <summary>
        /// Contains options for hatched (patterned) fills for each slice
        /// </summary>
        public HatchOptions[] HatchOptions { get; set; }

        /// <summary>
        /// The color of the slice outline.
        /// </summary>
        public Color Outline { get; set; } = Color.Black;

        /// <summary>
        /// The width of the slice outline.
        /// </summary>
        public float OutlineWidth { get; set; } = 0;


        /// <summary>
        /// The color to draw the axis in
        /// </summary>
        public Color WebColor { get; set; } = Color.Gray; // TODO: avoid this name in the future (see #1948)

        /// <summary>
        /// Controls rendering style of the concentric circles (ticks) of the web
        /// </summary>
        public RadarAxis AxisType { get; set; } = RadarAxis.Circle;

        /// <summary>
        /// If true, each value will be written in text on the plot.
        /// </summary>
        public bool ShowAxisValues { get; set; } = true;

        /// <summary>
        /// Labels for each category.
        /// Length must be equal to the number of columns (categories) in the original data.
        /// </summary>
        public string[] SliceLabels { get; set; }

        /// <summary>
        /// Icons for each category.
        /// Length must be equal to the number of columns (categories) in the original data. 
        /// </summary>
        public System.Drawing.Image[] CategoryImages { get; set; }

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public string Label { get; set; }

        private double[] Normalized { get; set; }

        public CoxcombPlot(double[] values, Color[] fillColors)
        {
            Values = values;
            FillColors = fillColors;
        }

        public void ValidateData(bool deep = false) { }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            int numCategories = Normalized.Length;
            PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));
            double sweepAngle = 360f / numCategories;
            double maxRadiusPixels = Math.Min(dims.PxPerUnitX, dims.PxPerUnitY);
            double maxDiameterPixels = maxRadiusPixels * 2;


            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Pen outlinePen = GDI.Pen(Outline, OutlineWidth);

            RenderAxis(gfx, dims, bmp, lowQuality);

            double start = -90;
            for (int i = 0; i < numCategories; i++)
            {
                using var sliceFillBrush = GDI.Brush(FillColors[i], HatchOptions?[i].Color, HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None);

                double angle = (Math.PI / 180) * ((sweepAngle + 2 * start) / 2);
                float diameter = (float)(maxDiameterPixels * Normalized[i]);

                var pieX = (int)origin.X - diameter / 2;
                var pieY = (int)origin.Y - diameter / 2;

                gfx.FillPie(brush: sliceFillBrush,
                    x: pieX,
                    y: pieY,
                    width: diameter,
                    height: diameter,
                    startAngle: (float)start,
                    sweepAngle: (float)sweepAngle);

                if (OutlineWidth != 0)
                {
                    gfx.DrawPie(outlinePen,
                        x: pieX,
                        y: pieY,
                        width: diameter,
                        height: diameter,
                        startAngle: (float)start,
                        sweepAngle: (float)sweepAngle);
                }

                start += sweepAngle;
            }
        }

        private void RenderAxis(Graphics gfx, PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double[,] Norm = new double[Normalized.Length, 1];
            for (int i = 0; i < Normalized.Length; i++)
            {
                Norm[i, 0] = Normalized[i];
            }

            StarAxisTick[] Ticks = new double[] { 0.25, 0.5, 1 }
                .Select(x => new StarAxisTick(x, Values.Max()))
                .ToArray();

            StarAxis axis = new()
            {
                Ticks = Ticks,
                CategoryLabels = SliceLabels,
                AxisType = AxisType,
                WebColor = WebColor,
                ShowAxisValues = ShowAxisValues,
                CategoryImages = CategoryImages,
                Graphics = gfx,
                ShowCategoryLabels = false,
                NumberOfSpokes = Values.Length,
                ImagePlacement = ImagePlacement.Inside
            };

            axis.Render(dims, bmp, lowQuality);
        }

        private static double[] Normalize(double[] values)
        {
            double Max = values.Max();

            if (Max == 0)
            {
                return values.Select(_ => 0.0).ToArray();
            }

            return values.Select(v => v / Max).ToArray();
        }

        public LegendItem[] GetLegendItems()
        {
            if (SliceLabels is null)
                return LegendItem.None;

            return Enumerable
                .Range(0, Values.Length)
                .Select(i => new LegendItem(this)
                {
                    label = SliceLabels[i],
                    color = FillColors[i],
                    lineWidth = 10,
                    hatchStyle = HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None,
                    hatchColor = HatchOptions?[i].Color ?? Color.Black
                })
                .ToArray();
        }

        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(-1.5, 1.5, -1.5, 1.5);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableCoxcomb {label} with {Values.Length} categories";
        }
    }
}
