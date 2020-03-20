using ScottPlot;
using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Experimental
{
    class PlottableBar2 : ScottPlot.Plottable
    {
        public readonly double[,] values;
        public readonly double[,] errors;
        public readonly string[] rowLabels;
        public readonly string[] colLabels;
        public int barsPerGroup { get { return values.GetLength(0); } }
        public int groupCount { get { return values.GetLength(1); } }

        public PlottableBar2(string[] rowLabels, string[] colLabels, double[,] values, double[,] errors = null)
        {
            if ((values is null) || (values.Length == 0))
                throw new ArgumentException("values array must contain data");

            if (values.GetLength(0) != rowLabels.Length)
                throw new ArgumentException("must have same number of rowLabels as rows");

            if (values.GetLength(1) != colLabels.Length)
                throw new ArgumentException("must have same number of colLabels as columns");

            this.rowLabels = rowLabels;
            this.colLabels = colLabels;
            this.values = values;
            this.errors = errors;
        }

        public override AxisLimits2D GetLimits()
        {
            double minValue = values[0, 0];
            double maxValue = values[0, 0];

            for (int colIndex = 0; colIndex < values.GetLength(1); colIndex++)
            {
                for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++)
                {
                    double value = values[rowIndex, colIndex];
                    minValue = Math.Min(minValue, value);
                    maxValue = Math.Max(maxValue, value);
                }
            }

            return new AxisLimits2D(-1, groupCount + 1, minValue, maxValue);
        }

        public override void Render(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double interGroupSpaceFrac = 0.25;
            double barFillGroupFrac = 1 - interGroupSpaceFrac;
            double barWidthFrac = barFillGroupFrac / barsPerGroup;

            for (int rowIndex = 0; rowIndex < values.GetLength(0); rowIndex++)
            {
                // set bar style for this whole series

                // TODO: set this a better way
                var barColor = new ScottPlot.Config.Colors().GetColor(rowIndex);
                var barBrush = new System.Drawing.SolidBrush(barColor);

                double barOffset = rowIndex * barWidthFrac;

                for (int colIndex = 0; colIndex < values.GetLength(1); colIndex++)
                {
                    // draw the bar for every group

                    // determine the width and horizontal offset of this bar
                    double xOffset = barWidthFrac / 2;
                    double groupOffset = colIndex;
                    double barLeft = groupOffset + barOffset - xOffset * barsPerGroup;
                    double barRight = barLeft + barWidthFrac;

                    // determine the height of this bar
                    double value = values[rowIndex, colIndex];
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
            return $"PlottableBar with {groupCount} groups and {barsPerGroup} bars per group";
        }
    }
}
