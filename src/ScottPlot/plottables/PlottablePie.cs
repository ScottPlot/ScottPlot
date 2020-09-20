using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottablePie : Plottable
    {
        public double[] values;
        public string label;
        public string[] groupNames;
        public Color[] colors;
        public bool explodedChart;
        public bool showValues;
        public bool showPercentages;
        public bool showLabels;
        public double donutSize;
        public float outlineSize = 0;
        public Color outlineColor = Color.Black;
        public string centerText;
        public float centerTextSize = 36;
        public Color centerTextColor = Color.Black;

        private SolidBrush brush = new SolidBrush(Color.Black);
        private Pen pen = new Pen(Color.Black);

        public PlottablePie(double[] values, string[] groupNames, Color[] colors, bool explodedChart, bool showValues, bool showPercentages, bool showLabels, string label)
        {
            this.values = values;
            this.label = label;
            this.groupNames = groupNames;
            this.colors = colors;
            this.explodedChart = explodedChart;
            this.showValues = showValues;
            this.showPercentages = showPercentages;
            this.showLabels = (groupNames is null) ? false : showLabels;
        }

        public override LegendItem[] GetLegendItems()
        {
            if (groupNames is null)
                return null;

            var items = new LegendItem[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                items[i] = new LegendItem(groupNames[i], colors[i], lineWidth: 10);
            }
            return items;
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(-0.5, 0.5, -1, 1);
        }

        public override int GetPointCount()
        {
            return values.Length;
        }

        public override void Render(Settings settings)
        {
            double[] proportions = values.Select(x => x / values.Sum()).ToArray();

            int sliceOutlineWidth = 0;
            if (explodedChart)
            {
                pen.Color = settings.DataBackground.Color; // TODO: will fail if data background is transparent
                sliceOutlineWidth = 1;
            }

            AxisLimits2D limits = GetLimits();
            double centreX = limits.xCenter;
            double centreY = limits.yCenter;
            float diameterPixels = .9f * Math.Min(settings.dataSize.Width, settings.dataSize.Height);
            string fontName = Config.Fonts.GetSansFontName();
            float fontSize = 12;

            // record label details and draw them after slices to prevent cover-ups
            double[] labelXs = new double[values.Length];
            double[] labelYs = new double[values.Length];
            string[] labelStrings = new string[values.Length];

            RectangleF boundingRectangle = new RectangleF((float)settings.GetPixelX(centreX) - diameterPixels / 2, (float)settings.GetPixelY(centreY) - diameterPixels / 2, diameterPixels, diameterPixels);

            if (donutSize > 0)
            {
                GraphicsPath graphicsPath = new GraphicsPath();
                float donutDiameterPixels = (float)donutSize * diameterPixels;
                RectangleF donutHoleBoundingRectangle = new RectangleF((float)settings.GetPixelX(centreX) - donutDiameterPixels / 2, (float)settings.GetPixelY(centreY) - donutDiameterPixels / 2, donutDiameterPixels, donutDiameterPixels);
                graphicsPath.AddEllipse(donutHoleBoundingRectangle);
                Region excludedRegion = new Region(graphicsPath);
                settings.gfxData.ExcludeClip(excludedRegion);
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
                string sliceLabelName = (showLabels) ? groupNames[i] : "";
                labelStrings[i] = $"{sliceLabelValue}\n{sliceLabelPercentage}\n{sliceLabelName}".Trim();

                brush.Color = colors[i];
                settings.gfxData.FillPie(brush: brush,
                    x: (int)(boundingRectangle.X + xOffset),
                    y: (int)(boundingRectangle.Y + yOffset),
                    width: boundingRectangle.Width,
                    height: boundingRectangle.Height,
                    startAngle: (float)start,
                    sweepAngle: (float)(sweep + sweepOffset));

                if (explodedChart)
                {
                    pen.Color = settings.DataBackground.Color; // TODO: will fail if data background is transparent
                    pen.Width = sliceOutlineWidth;
                    settings.gfxData.DrawPie(
                        pen: pen,
                        x: (int)(boundingRectangle.X + xOffset),
                        y: (int)(boundingRectangle.Y + yOffset),
                        width: boundingRectangle.Width, boundingRectangle.Height,
                        startAngle: (float)start,
                        sweepAngle: (float)(sweep + sweepOffset));
                }
                start += sweep;
            }

            brush.Color = Color.White;
            var font = new Font(fontName, fontSize);
            for (int i = 0; i < values.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(labelStrings[i]))
                {
                    settings.gfxData.DrawString(labelStrings[i], font, brush,
                        (float)labelXs[i], (float)labelYs[i], settings.misc.sfCenterCenter);
                }
            }

            if (outlineSize > 0)
            {
                pen.Width = outlineSize;
                pen.Color = outlineColor;
                settings.gfxData.DrawEllipse(pen, boundingRectangle.X, boundingRectangle.Y, boundingRectangle.Width, boundingRectangle.Height);
            }

            settings.gfxData.ResetClip();

            if (centerText != null)
            {
                brush.Color = centerTextColor;
                Font donutHoleFont = new Font(fontName, centerTextSize);
                settings.gfxData.DrawString(centerText, donutHoleFont, brush, settings.GetPixel(0, 0), settings.misc.sfCenterCenter);
                donutHoleFont.Dispose();
            }

            if (explodedChart)
            {
                // draw a background-colored circle around the perimeter to make it look like all pieces are the same size
                pen.Width = 20;
                settings.gfxData.DrawEllipse(
                    pen: pen,
                    x: boundingRectangle.X,
                    y: boundingRectangle.Y,
                    width: boundingRectangle.Width,
                    height: boundingRectangle.Height);
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePie{label} with {GetPointCount()} points";
        }
    }
}
