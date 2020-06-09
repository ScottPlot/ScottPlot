using System;
using System.Collections.Generic;
using System.Drawing;
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

            int outlineWidth = 1;
            int sliceOutlineWidth = 0;
            if (explodedChart)
            {
                pen.Color = settings.misc.dataBackgroundColor;
                outlineWidth = 20;
                sliceOutlineWidth = 1;
            }

            double minAxisScale = Math.Min(settings.xAxisScale, settings.yAxisScale);
            AxisLimits2D limits = GetLimits();
            double centreX = (limits.x1 + limits.x2) / 2;
            double centreY = (limits.y1 + limits.y2) / 2;
            double diameter = 2; // Unit circle
            float diameterPixels = (float)(minAxisScale * diameter);
            string fontName = Config.Fonts.GetSansFontName();
            float fontSize = 12;

            // record label details and draw them after slices to prevent cover-ups
            double[] labelXs = new double[values.Length];
            double[] labelYs = new double[values.Length];
            string[] labelStrings = new string[values.Length];

            RectangleF boundingRectangle = new RectangleF((float)settings.GetPixelX(centreX) - diameterPixels / 2, (float)settings.GetPixelY(centreY) - diameterPixels / 2, diameterPixels, diameterPixels);

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
                double sliceLabelR = 0.5 * minAxisScale;
                labelXs[i] = (boundingRectangle.X + diameterPixels / 2 + xOffset + Math.Cos(angle) * sliceLabelR);
                labelYs[i] = (boundingRectangle.Y + diameterPixels / 2 + yOffset + Math.Sin(angle) * sliceLabelR);
                string sliceLabelValue = (showValues) ? $"{values[i]}" : "";
                string sliceLabelPercentage = (showPercentages) ? $"{proportions[i] * 100:f1}%" : "";
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
            settings.gfxData.DrawEllipse(pen, boundingRectangle.X, boundingRectangle.Y, boundingRectangle.Width, boundingRectangle.Height);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePie{label} with {GetPointCount()} points";
        }
    }
}
