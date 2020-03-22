using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.plottables
{
    class PlottableBar3 : Plottable, ILegend
    {
        public readonly DataSet[] datasets;
        public readonly string[] groupLabels;
        public readonly int groupCount = 0;
        public readonly int barSetCount;

        public PlottableBar3(DataSet[] datasets, string[] groupLabels)
        {
            this.datasets = datasets;
            this.groupLabels = groupLabels;

            // MUST populate barSetCount and groupCount in constructor
            barSetCount = datasets.Length;
            foreach (DataSet barSet in datasets)
                groupCount = Math.Max(groupCount, barSet.values.Length);

            if (groupLabels.Length != groupCount)
                throw new ArgumentException("groupLabels must be same number of elements as the largest barSet values");
        }

        public LegendItem[] GetLegendItems()
        {
            var items = new List<LegendItem>();
            for (int i = 0; i < groupLabels.Length; i++)
                items.Add(new LegendItem(groupLabels[i], System.Drawing.Color.Magenta, LineStyle.Solid));
            return items.ToArray();
        }

        public override AxisLimits2D GetLimits()
        {
            double minValue = datasets[0].values[0];
            double maxValue = datasets[0].values[0];

            foreach (var barSet in datasets)
            {
                foreach (var value in barSet.values)
                {
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);
                }
            }

            // define how wide the bar graphs and spaces should be
            double interGroupSpaceFrac = 0.25;
            double barFillGroupFrac = 1 - interGroupSpaceFrac;
            double sidePadding = barFillGroupFrac / 2;

            return new AxisLimits2D(-sidePadding, groupCount - 1 + sidePadding, minValue, maxValue);
        }

        public override void Render(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double interGroupSpaceFrac = 0.25;
            double barFillGroupFrac = 1 - interGroupSpaceFrac;
            double barWidthFrac = barFillGroupFrac / barSetCount;

            for (int setIndex = 0; setIndex < barSetCount; setIndex++)
            {
                // set bar style for this whole series

                // TODO: set this a better way
                var barColor = new ScottPlot.Config.Colors().GetColor(setIndex);
                var barBrush = new System.Drawing.SolidBrush(barColor);

                double barOffset = setIndex * barWidthFrac;

                for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
                {
                    // draw the bar for every group

                    // determine the width and horizontal offset of this bar
                    double xOffset = barWidthFrac / 2;
                    double groupOffset = groupIndex;
                    double barLeft = groupOffset + barOffset - xOffset * barSetCount;
                    double barRight = barLeft + barWidthFrac;

                    // determine the height of this bar
                    double value = datasets[setIndex].values[groupIndex];
                    double valueMax, valueMin;
                    if (value > 0)
                    {
                        valueMax = value;
                        valueMin = 0;
                    }
                    else
                    {
                        valueMax = 0;
                        valueMin = value;
                    }

                    // convert coordinates to pixels and draw the bar
                    double barTopPixel = settings.GetPixelY(valueMax);
                    double barBotPixel = settings.GetPixelY(valueMin);
                    double barLeftPixel = settings.GetPixelX(barLeft);
                    double barRightPixel = settings.GetPixelX(barRight);
                    double barWidthPx = barRightPixel - barLeftPixel;
                    double barHeightPx = barBotPixel - barTopPixel;

                    settings.gfxData.FillRectangle(barBrush, (float)barLeftPixel, (float)barTopPixel, (float)barWidthPx, (float)barHeightPx);
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableBar with {groupCount} groups and {barSetCount} bars per group";
        }
    }
}
