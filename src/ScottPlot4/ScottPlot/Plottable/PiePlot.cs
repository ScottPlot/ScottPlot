using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A pie plot displays a collection of values as a circle.
    /// Pie plots with a hollow center are donut plots.
    /// </summary>
    public class PiePlot : IPlottable
    {
        public double[] Values { get; set; }
        public string Label { get; set; }

        /// <summary>
        /// Labels to display on top of each slice or in the legend.
        /// </summary>
        public string[] SliceLabels { get; set; }

        /// <summary>
        /// If populated, this array of strings will be used for the legend.
        /// </summary>
        public string[] LegendLabels { get; set; }

        /// <summary>
        /// Defines how large the pie is relative to the pixel size of the smallest axis
        /// </summary>
        public double Size { get; set; } = 0.9;

        public Color[] SliceFillColors { get; set; }
        public Color[] SliceLabelColors { get; set; }
        public Color BackgroundColor { get; set; }
        public HatchOptions[] HatchOptions { get; set; }

        public bool Explode { get; set; }
        public bool ShowValues { get; set; }
        public bool ShowPercentages { get; set; }

        /// <summary>
        /// If enabled, <see cref="SliceLabels"/> will be displayed above each slice.
        /// </summary>
        public bool ShowLabels { get; set; }

        public double DonutSize { get; set; }
        public string DonutLabel { get; set; }
        public readonly Drawing.Font CenterFont = new();
        public readonly Drawing.Font SliceFont = new();

        public float OutlineSize { get; set; } = 0;
        public Color OutlineColor { get; set; } = Color.Black;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public PiePlot(double[] values, string[] groupNames, Color[] colors)
        {
            Values = values;
            SliceLabels = groupNames;
            SliceFillColors = colors;

            SliceFont.Size = 18;
            SliceFont.Bold = true;
            SliceFont.Color = Color.White;

            CenterFont.Size = 48;
            CenterFont.Bold = true;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottablePie{label} with {PointCount} points";
        }

        public LegendItem[] GetLegendItems()
        {
            if (SliceLabels is null)
                return LegendItem.None;

            string[] labels = SliceLabels;

            if (LegendLabels is not null)
            {
                if (LegendLabels.Length != Values.Length)
                    throw new InvalidOperationException("custom legend labels must have the same number of items as slices");
                labels = LegendLabels;
            }

            return Enumerable
                .Range(0, Values.Length)
                .Select(i => new LegendItem(this)
                {
                    label = labels[i],
                    color = SliceFillColors[i],
                    lineWidth = 10,
                    hatchStyle = HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None,
                    hatchColor = HatchOptions?[i].Color ?? Color.Black

                })
                .ToArray();
        }

        public AxisLimits GetAxisLimits()
        {
            return new AxisLimits(-0.5, 0.5, -1, 1);
        }

        public int PointCount { get => Values.Length; }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("values", Values);
            Validate.AssertHasElements("colors", SliceFillColors);
            Validate.AssertAllReal("values", Values);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen backgroundPen = GDI.Pen(BackgroundColor))
            using (Pen outlinePen = GDI.Pen(OutlineColor, OutlineSize))
            using (var sliceFont = GDI.Font(SliceFont))
            using (SolidBrush sliceFontBrush = (SolidBrush)GDI.Brush(SliceFont.Color))
            using (var centerFont = GDI.Font(CenterFont))
            using (Brush centerFontBrush = GDI.Brush(CenterFont.Color))
            using (StringFormat sfCenter = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
            {
                double[] proportions = Values.Select(x => x / Values.Sum()).ToArray();

                double centreX = 0;
                double centreY = 0;
                float diameterPixels = (float)Size * Math.Min(dims.DataWidth, dims.DataHeight);

                // record label details and draw them after slices to prevent cover-ups
                double[] labelXs = new double[Values.Length];
                double[] labelYs = new double[Values.Length];
                string[] labelStrings = new string[Values.Length];

                RectangleF boundingRectangle = new RectangleF(
                    dims.GetPixelX(centreX) - diameterPixels / 2,
                    dims.GetPixelY(centreY) - diameterPixels / 2,
                    diameterPixels,
                    diameterPixels);

                if (DonutSize > 0)
                {
                    GraphicsPath graphicsPath = new GraphicsPath();
                    float donutDiameterPixels = (float)DonutSize * diameterPixels;
                    RectangleF donutHoleBoundingRectangle = new RectangleF(
                        dims.GetPixelX(centreX) - donutDiameterPixels / 2,
                        dims.GetPixelY(centreY) - donutDiameterPixels / 2,
                        donutDiameterPixels,
                        donutDiameterPixels);
                    graphicsPath.AddEllipse(donutHoleBoundingRectangle);
                    Region excludedRegion = new Region(graphicsPath);
                    gfx.ExcludeClip(excludedRegion);
                }

                double start = -90;
                for (int i = 0; i < Values.Length; i++)
                {
                    // determine where the slice is to be drawn
                    double sweep = proportions[i] * 360;
                    double sweepOffset = Explode ? -1 : 0;
                    double angle = (Math.PI / 180) * ((sweep + 2 * start) / 2);
                    double xOffset = Explode ? 3 * Math.Cos(angle) : 0;
                    double yOffset = Explode ? 3 * Math.Sin(angle) : 0;

                    // record where and what to label the slice
                    double sliceLabelR = 0.35 * diameterPixels;
                    labelXs[i] = (boundingRectangle.X + diameterPixels / 2 + xOffset + Math.Cos(angle) * sliceLabelR);
                    labelYs[i] = (boundingRectangle.Y + diameterPixels / 2 + yOffset + Math.Sin(angle) * sliceLabelR);
                    string sliceLabelValue = (ShowValues) ? $"{Values[i]}" : "";
                    string sliceLabelPercentage = ShowPercentages ? $"{proportions[i] * 100:f1}%" : "";
                    string sliceLabelName = (ShowLabels && SliceLabels != null) ? SliceLabels[i] : "";
                    labelStrings[i] = $"{sliceLabelValue}\n{sliceLabelPercentage}\n{sliceLabelName}".Trim();

                    using var sliceFillBrush = GDI.Brush(SliceFillColors[i], HatchOptions?[i].Color, HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None);

                    Rectangle offsetRectangle = new(
                        x: (int)(boundingRectangle.X + xOffset),
                        y: (int)(boundingRectangle.Y + yOffset),
                        width: (int)boundingRectangle.Width,
                        height: (int)boundingRectangle.Height);

                    // System.Drawing cannot render extremely small shapes
                    // https://github.com/ScottPlot/ScottPlot/issues/2415
                    if (offsetRectangle.Width < 1 || offsetRectangle.Height < 1)
                        return;

                    if (sweep != 360)
                    {
                        gfx.FillPie(brush: sliceFillBrush,
                            rect: offsetRectangle,
                            startAngle: (float)start,
                            sweepAngle: (float)(sweep + sweepOffset));
                    }
                    else
                    {
                        gfx.FillEllipse(sliceFillBrush, offsetRectangle);
                    }


                    if (Explode && sweep != 360)
                    {
                        gfx.DrawPie(
                            pen: backgroundPen,
                            rect: offsetRectangle,
                            startAngle: (float)start,
                            sweepAngle: (float)(sweep + sweepOffset));
                    }
                    start += sweep;
                }

                // TODO: move length checking logic into new validation system (triaged March, 2021)
                bool useCustomLabelColors = SliceLabelColors is not null && SliceLabelColors.Length == Values.Length;

                for (int i = 0; i < Values.Length; i++)
                    if (!string.IsNullOrWhiteSpace(labelStrings[i]))
                    {
                        if (useCustomLabelColors)
                            sliceFontBrush.Color = SliceLabelColors[i];

                        gfx.DrawString(labelStrings[i], sliceFont, sliceFontBrush, (float)labelXs[i], (float)labelYs[i], sfCenter);
                    }

                if (OutlineSize > 0)
                    gfx.DrawEllipse(
                        outlinePen,
                        boundingRectangle.X,
                        boundingRectangle.Y,
                        boundingRectangle.Width,
                        boundingRectangle.Height);

                gfx.ResetClip();

                if (DonutLabel != null)
                    gfx.DrawString(DonutLabel, centerFont, centerFontBrush, dims.GetPixelX(0), dims.GetPixelY(0), sfCenter);

                if (Explode)
                {
                    // draw a background-colored circle around the perimeter to make it look like all pieces are the same size
                    backgroundPen.Width = 20;
                    gfx.DrawEllipse(
                        pen: backgroundPen,
                        x: boundingRectangle.X,
                        y: boundingRectangle.Y,
                        width: boundingRectangle.Width,
                        height: boundingRectangle.Height);
                }
            }
        }
    }
}
