using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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

        private SolidBrush brush = new SolidBrush(Color.Black);
        private Pen pen = new Pen(Color.Black);

        public PlottablePie(double[] values, string label, string[] groupNames, Color[] colors, bool explodedChart, bool showValues)
        {
            this.values = values;
            this.label = label;
            this.groupNames = groupNames;
            this.colors = colors;
            this.explodedChart = explodedChart;
            this.showValues = showValues;
        }

        public override LegendItem[] GetLegendItems()
        {
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

            RectangleF boundingRectangle = new RectangleF((float)settings.GetPixelX(centreX) - diameterPixels / 2, (float)settings.GetPixelY(centreY) - diameterPixels / 2, diameterPixels, diameterPixels);
            double start = -90;
            for (int i = 0; i < proportions.Length; i++)
            {
                double sweep = proportions[i] * 360;
                double sweepOffset = explodedChart ? -1 : 0;
                double angle = (Math.PI / 180) * ((sweep + 2 * start) / 2);
                double xOffset = explodedChart ? 3 * Math.Cos(angle) : 0;
                double yOffset = explodedChart ? 3 * Math.Sin(angle) : 0;

                brush.Color = colors[i];
                settings.gfxData.FillPie(brush, (int)(boundingRectangle.X + xOffset), (int)(boundingRectangle.Y + yOffset), boundingRectangle.Width, boundingRectangle.Height, (float)start, (float)(sweep + sweepOffset));

                if (explodedChart)
                {
                    pen.Width = sliceOutlineWidth;
                    settings.gfxData.DrawPie(pen, (int)(boundingRectangle.X + xOffset), (int)(boundingRectangle.Y + yOffset), boundingRectangle.Width, boundingRectangle.Height, (float)start, (float)(sweep + sweepOffset));
                }
                start += sweep;

                if (showValues)
                {
                    brush.Color = Color.White;
                    double radius = 0.5 * minAxisScale;
                    settings.gfxData.DrawString($"{proportions[i] * 100:f1}%", new Font("Sans Serif", 12), brush,
                        (float)(boundingRectangle.X + diameterPixels / 2 + xOffset + Math.Cos(angle) * radius),
                        (float)(boundingRectangle.Y + diameterPixels / 2 + yOffset + Math.Sin(angle) * radius),
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
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
