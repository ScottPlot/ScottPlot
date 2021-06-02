﻿using ScottPlot.Drawing;
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
        public double[] Values;
        public string Label;
        public string[] SliceLabels;

        public Color[] SliceFillColors;
        public Color[] SliceLabelColors;
        public Color BackgroundColor;

        public bool Explode;
        public bool ShowValues;
        public bool ShowPercentages;
        public bool ShowLabels;

        public double DonutSize;
        public string DonutLabel;
        public readonly Drawing.Font CenterFont = new Drawing.Font();
        public readonly Drawing.Font SliceFont = new Drawing.Font();

        public float OutlineSize = 0;
        public Color OutlineColor = Color.Black;

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
                return null;

            return Enumerable
                .Range(0, Values.Length)
                .Select(i => new LegendItem() { label = SliceLabels[i], color = SliceFillColors[i], lineWidth = 10 })
                .ToArray();
        }

        public AxisLimits GetAxisLimits() => new AxisLimits(-0.5, 0.5, -1, 1);

        public int PointCount { get => Values.Length; }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements("values", Values);
            Validate.AssertHasElements("colors", SliceFillColors);
            Validate.AssertAllReal("values", Values);
            // TODO: ensure the length of colors and group names matches expected length
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen backgroundPen = GDI.Pen(BackgroundColor))
            using (Pen outlinePen = GDI.Pen(OutlineColor, OutlineSize))
            using (Brush sliceFillBrush = GDI.Brush(Color.Black))
            using (var sliceFont = GDI.Font(SliceFont))
            using (SolidBrush sliceFontBrush = (SolidBrush)GDI.Brush(SliceFont.Color))
            using (var centerFont = GDI.Font(CenterFont))
            using (Brush centerFontBrush = GDI.Brush(CenterFont.Color))
            using (StringFormat sfCenter = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
            {
                double[] proportions = Values.Select(x => x / Values.Sum()).ToArray();

                double centreX = 0;
                double centreY = 0;
                float diameterPixels = .9f * Math.Min(dims.DataWidth, dims.DataHeight);

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

                    ((SolidBrush)sliceFillBrush).Color = SliceFillColors[i];
                    gfx.FillPie(brush: sliceFillBrush,
                        x: (int)(boundingRectangle.X + xOffset),
                        y: (int)(boundingRectangle.Y + yOffset),
                        width: boundingRectangle.Width,
                        height: boundingRectangle.Height,
                        startAngle: (float)start,
                        sweepAngle: (float)(sweep + sweepOffset));

                    if (Explode)
                    {
                        gfx.DrawPie(
                            pen: backgroundPen,
                            x: (int)(boundingRectangle.X + xOffset),
                            y: (int)(boundingRectangle.Y + yOffset),
                            width: boundingRectangle.Width,
                            height: boundingRectangle.Height,
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
