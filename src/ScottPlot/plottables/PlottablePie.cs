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
        bool explodedChart;
        bool showValues;
        bool showPercentages;
        bool showLabels;
        bool donut;
        double donutSize;
        bool showPercentageInDonut;
        bool drawOutline;

        private SolidBrush brush = new SolidBrush(Color.Black);
        private Pen pen = new Pen(Color.Black);

        public PlottablePie(double[] values, string[] groupNames, Color[] colors, bool explodedChart, bool showValues, bool showPercentages, bool showLabels, string label, bool donut, double donutSize, bool showPercentageInDonut, bool drawOutline)
        {
            this.values = values;
            this.label = label;
            this.groupNames = groupNames;
            this.colors = colors;
            this.explodedChart = explodedChart;
            this.showValues = showValues;
            this.showPercentages = showPercentages;
            this.showLabels = (groupNames is null) ? false : showLabels;
            this.donut = donut;
            this.donutSize = donutSize;
            this.showPercentageInDonut = showPercentageInDonut;
            this.drawOutline = drawOutline;
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

            int outlineWidth = 1;
            int sliceOutlineWidth = 0;
            if (explodedChart)
            {
                pen.Color = settings.DataBackground.Color; // TODO: will fail if data background is transparent
                outlineWidth = 20;
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

            if (donut)
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
                string sliceLabelPercentage = (showPercentages && !showPercentageInDonut) ? $"{proportions[i] * 100:f1}%" : "";
                string sliceLabelName = (showLabels) ? groupNames[i] : "";
                labelStrings[i] = $"{sliceLabelValue}\n{sliceLabelPercentage}\n{sliceLabelName}".Trim();

                brush.Color = colors[i];
                settings.gfxData.FillPie(brush, (int)(boundingRectangle.X + xOffset), (int)(boundingRectangle.Y + yOffset), boundingRectangle.Width, boundingRectangle.Height, (float)start, (float)(sweep + sweepOffset));

                if (explodedChart)
                {
                    pen.Width = sliceOutlineWidth;
                    settings.gfxData.DrawPie(pen, (int)(boundingRectangle.X + xOffset), (int)(boundingRectangle.Y + yOffset), boundingRectangle.Width, boundingRectangle.Height, (float)start, (float)(sweep + sweepOffset));
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

            pen.Width = outlineWidth;

            if (!drawOutline && !explodedChart)
            {
                pen.Color = System.Drawing.Color.Transparent;
            }

            settings.gfxData.DrawEllipse(pen, boundingRectangle.X, boundingRectangle.Y, boundingRectangle.Width, boundingRectangle.Height);

            settings.gfxData.ResetClip();

            if (showPercentageInDonut)
            {
                int maxPercentageIndex = 0;
                for (int i = 1; i < proportions.Length; i++)
                {
                    if (proportions[i] > proportions[maxPercentageIndex])
                    {
                        maxPercentageIndex = i;
                    }
                }
                int maxPercentage = (int)Math.Round(proportions[maxPercentageIndex] * 100);
                brush.Color = colors[maxPercentageIndex];
                Font donutHoleFont = new Font(fontName, 36);
                settings.gfxData.DrawString($"{maxPercentage}%", donutHoleFont, brush, settings.GetPixel(0, 0), settings.misc.sfCenterCenter);
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePie{label} with {GetPointCount()} points";
        }
    }
}
