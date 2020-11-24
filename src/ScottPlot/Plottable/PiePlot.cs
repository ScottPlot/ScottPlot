﻿using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
    public class PiePlot : IRenderable, IHasLegendItems, IUsesAxes, IValidatable
    {
        public double[] values;
        public string label;
        public string[] groupNames;
        public Color[] colors;
        public Color dataBackgroundColor;
        public bool explodedChart;
        public bool showValues;
        public bool showPercentages;
        public bool showLabels;
        public double donutSize;
        public float outlineSize = 0;
        public Color outlineColor = Color.Black;
        public string centerText;
        public float sliceFontSize = 14;
        public float centerFontSize = 36;
        public Color centerTextColor = Color.Black;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public PiePlot(double[] values, string[] groupNames, Color[] colors)
        {
            this.values = values;
            this.groupNames = groupNames;
            this.colors = colors;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePie{label} with {PointCount} points";
        }

        public LegendItem[] LegendItems
        {
            get
            {
                if (groupNames is null)
                    return null;

                return Enumerable
                    .Range(0, values.Length)
                    .Select(i => new LegendItem() { label = groupNames[i], color = colors[i], lineWidth = 10 })
                    .ToArray();
            }
        }

        public AxisLimits GetAxisLimits() => new AxisLimits(-0.5, 0.5, -1, 1);

        public int PointCount { get => values.Length; }

        public string ErrorMessage(bool deepValidation = false)
        {
            try
            {
                Validate.AssertHasElements("values", values);
                Validate.AssertHasElements("groupNames", groupNames);
                Validate.AssertHasElements("colors", colors);
                Validate.AssertAllReal("values", values);
                if (values.Length != groupNames.Length || values.Length != colors.Length)
                    throw new ArgumentException("values, groupNames, and colors must have equal length");
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }

            return null;
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen backgroundPen = GDI.Pen(dataBackgroundColor))
            using (Pen outlinePen = GDI.Pen(outlineColor, outlineSize))
            using (Brush brush = GDI.Brush(Color.Black))
            using (Brush fontBrush = GDI.Brush(centerTextColor))
            using (var sliceFont = GDI.Font(null, sliceFontSize))
            using (var centerFont = GDI.Font(null, centerFontSize))
            using (StringFormat sfCenter = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
            {
                double[] proportions = values.Select(x => x / values.Sum()).ToArray();

                double centreX = 0;
                double centreY = 0;
                float diameterPixels = .9f * Math.Min(dims.DataWidth, dims.DataHeight);

                // record label details and draw them after slices to prevent cover-ups
                double[] labelXs = new double[values.Length];
                double[] labelYs = new double[values.Length];
                string[] labelStrings = new string[values.Length];

                RectangleF boundingRectangle = new RectangleF(
                    dims.GetPixelX(centreX) - diameterPixels / 2,
                    dims.GetPixelY(centreY) - diameterPixels / 2,
                    diameterPixels,
                    diameterPixels);

                if (donutSize > 0)
                {
                    GraphicsPath graphicsPath = new GraphicsPath();
                    float donutDiameterPixels = (float)donutSize * diameterPixels;
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
                for (int i = 0; i < values.Length; i++)
                {
                    // determine where the slice is to be drawn
                    double sweep = proportions[i] * 360;
                    double sweepOffset = explodedChart ? -1 : 0;
                    double angle = (Math.PI / 180) * ((sweep + 2 * start) / 2);
                    double xOffset = explodedChart ? 3 * Math.Cos(angle) : 0;
                    double yOffset = explodedChart ? 3 * Math.Sin(angle) : 0;

                    // record where and what to label the slice
                    double sliceLabelR = 0.35 * diameterPixels;
                    labelXs[i] = (boundingRectangle.X + diameterPixels / 2 + xOffset + Math.Cos(angle) * sliceLabelR);
                    labelYs[i] = (boundingRectangle.Y + diameterPixels / 2 + yOffset + Math.Sin(angle) * sliceLabelR);
                    string sliceLabelValue = (showValues) ? $"{values[i]}" : "";
                    string sliceLabelPercentage = showPercentages ? $"{proportions[i] * 100:f1}%" : "";
                    string sliceLabelName = (showLabels && groupNames != null) ? groupNames[i] : "";
                    labelStrings[i] = $"{sliceLabelValue}\n{sliceLabelPercentage}\n{sliceLabelName}".Trim();

                    ((SolidBrush)brush).Color = colors[i];
                    gfx.FillPie(brush: brush,
                        x: (int)(boundingRectangle.X + xOffset),
                        y: (int)(boundingRectangle.Y + yOffset),
                        width: boundingRectangle.Width,
                        height: boundingRectangle.Height,
                        startAngle: (float)start,
                        sweepAngle: (float)(sweep + sweepOffset));

                    if (explodedChart)
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

                ((SolidBrush)brush).Color = Color.White;
                for (int i = 0; i < values.Length; i++)
                    if (!string.IsNullOrWhiteSpace(labelStrings[i]))
                        gfx.DrawString(labelStrings[i], sliceFont, brush, (float)labelXs[i], (float)labelYs[i], sfCenter);

                if (outlineSize > 0)
                    gfx.DrawEllipse(
                        outlinePen,
                        boundingRectangle.X,
                        boundingRectangle.Y,
                        boundingRectangle.Width,
                        boundingRectangle.Height);

                gfx.ResetClip();

                if (centerText != null)
                    gfx.DrawString(centerText, centerFont, fontBrush, dims.GetPixelX(0), dims.GetPixelY(0), sfCenter);

                if (explodedChart)
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
