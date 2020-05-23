using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottablePie : Plottable
    {
        public double[] proportions;
        public string label;
        public string[] groupNames;
        public Color[] colors;

        private Pen pen = new Pen(Color.Black, 2);
        private SolidBrush brush = new SolidBrush(Color.Black);

        public PlottablePie(double[] proportions, string label, string[] groupNames, Color[] colors)
        {
            this.proportions = proportions;
            this.label = label;
            this.groupNames = groupNames;
            this.colors = colors;
        }

        public override LegendItem[] GetLegendItems()
        {
            var items = new LegendItem[proportions.Length];
            for (int i = 0; i < proportions.Length; i++)
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
            return proportions.Length;
        }

        public override void Render(Settings settings)
        {
            double minAxisScale = Math.Min(settings.xAxisScale, settings.yAxisScale);
            AxisLimits2D limits = GetLimits();
            double centreX = (limits.x1 + limits.x2) / 2;
            double centreY = (limits.y1 + limits.y2) / 2;
            double diameter = 2;
            float diameterPixels = (float)(minAxisScale * diameter);
            RectangleF boundingRectangle = new RectangleF((float)settings.GetPixelX(centreX) - diameterPixels / 2, (float)settings.GetPixelY(centreY) - diameterPixels / 2, diameterPixels, diameterPixels);
            pen.Color = Color.Black;
            brush.Color = Color.Black;
            settings.gfxData.FillEllipse(brush, boundingRectangle);

            double start = -90;
            for (int i = 0; i < proportions.Length; i++)
            {
                double sweep = proportions[i] * 360;
                pen.Color = colors[i];
                brush.Color = colors[i];
                settings.gfxData.FillPie(brush, boundingRectangle.X, boundingRectangle.Y, boundingRectangle.Width, boundingRectangle.Height, (float)start, (float)sweep);
                start += sweep;
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePie{label} with {GetPointCount()} points";
        }
    }
}
