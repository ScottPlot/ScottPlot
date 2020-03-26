using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public class PlottableBar : Plottable
    {
        public readonly DataSet[] datasets;
        public readonly string[] groupLabels;
        public readonly int groupCount;
        public readonly int barSetCount;

        public readonly System.Drawing.Color[] setColors;
        public readonly System.Drawing.Brush[] setBrushes;
        public readonly System.Drawing.Pen[] setErrorPens;
        public readonly System.Drawing.Pen[] setOutlinePens;

        public bool stacked;
        public bool horizontal;
        public double outlineWidth;
        public double errorLineWidth;
        public double errorCapSize;

        public PlottableBar(DataSet[] datasets, string[] groupLabels, bool stacked, bool horizontal,
            double outlineWidth, double errorLineWidth, double errorCapSize,
            System.Drawing.Color[] setColors = null)
        {
            this.datasets = datasets;
            this.groupLabels = groupLabels;
            this.stacked = stacked;
            this.horizontal = horizontal;

            this.errorLineWidth = errorLineWidth;
            this.outlineWidth = outlineWidth;
            this.errorCapSize = errorCapSize;

            // MUST populate barSetCount and groupCount in constructor
            barSetCount = datasets.Length;
            foreach (DataSet barSet in datasets)
                groupCount = Math.Max(groupCount, barSet.values.Length);

            if (groupLabels.Length != groupCount)
                throw new ArgumentException("groupLabels must be same number of elements as the largest barSet values");

            if (setColors is null)
            {
                this.setColors = new System.Drawing.Color[barSetCount];
                for (int i = 0; i < barSetCount; i++)
                    this.setColors[i] = new ScottPlot.Config.Colors().GetColor(i);
            }
            else
            {
                if (setColors.Length != barSetCount)
                    throw new ArgumentException("groupColors must be same number of elements as the largest barSet values");
                this.setColors = setColors;
            }

            setBrushes = new System.Drawing.Brush[barSetCount];
            setErrorPens = new System.Drawing.Pen[barSetCount];
            setOutlinePens = new System.Drawing.Pen[barSetCount];
            for (int i = 0; i < barSetCount; i++)
            {
                setBrushes[i] = new System.Drawing.SolidBrush(this.setColors[i]);
                setErrorPens[i] = new System.Drawing.Pen(System.Drawing.Color.Black, (float)errorLineWidth);
                setOutlinePens[i] = new System.Drawing.Pen(System.Drawing.Color.Black, (float)outlineWidth);
            }
        }

        private (double min, double max) GetLimitsStandard()
        {
            double minValue = datasets[0].values[0];
            double maxValue = datasets[0].values[0];

            foreach (var barSet in datasets)
            {
                for (int valueIndex = 0; valueIndex < barSet.values.Length; valueIndex++)
                {
                    if (barSet.errors is null)
                    {
                        minValue = Math.Min(minValue, barSet.values[valueIndex]);
                        maxValue = Math.Max(maxValue, barSet.values[valueIndex]);
                    }
                    else
                    {
                        minValue = Math.Min(minValue, barSet.values[valueIndex] - barSet.errors[valueIndex]);
                        maxValue = Math.Max(maxValue, barSet.values[valueIndex] + barSet.errors[valueIndex]);
                    }
                }
            }

            return (Math.Min(0, minValue), maxValue);
        }

        private (double min, double max) GetLimitsStacked()
        {
            double maxValue = double.NegativeInfinity;

            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                double groupSum = 0;
                foreach (var barSet in datasets)
                    groupSum += barSet.values[groupIndex];
                maxValue = Math.Max(maxValue, groupSum);
            }

            return (0, maxValue);
        }

        public override AxisLimits2D GetLimits()
        {
            (double minValue, double maxValue) = stacked ? GetLimitsStacked() : GetLimitsStandard();

            double interGroupSpaceFrac = 0.25;
            double barFillGroupFrac = 1 - interGroupSpaceFrac;
            double sidePadding = barFillGroupFrac / 2;

            if (horizontal)
                return new AxisLimits2D(minValue, maxValue, -sidePadding, groupCount - 1 + sidePadding);
            else
                return new AxisLimits2D(-sidePadding, groupCount - 1 + sidePadding, minValue, maxValue);
        }

        public override void Render(Settings settings)
        {
            if (horizontal)
            {
                if (stacked)
                    RenderHorizontalStacked(settings);
                else
                    RenderHorizontalGrouped(settings);
                    return;
            }
            else
            {
                if (stacked)
                    RenderVerticalStacked(settings);
                else
                    RenderVerticalGrouped(settings);
            }
        }

        double interGroupSpaceFrac = 0.25;

        public void RenderVerticalGrouped(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double barFillGroupFrac = 1 - interGroupSpaceFrac;
            double barWidthFrac = barFillGroupFrac / barSetCount;

            for (int setIndex = 0; setIndex < barSetCount; setIndex++)
            {
                // set bar style for this whole series

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
                    double barCenterPixel = (barLeftPixel + barRightPixel) / 2;
                    double barWidthPx = barRightPixel - barLeftPixel;
                    double barHeightPx = barBotPixel - barTopPixel;


                    var rect = new System.Drawing.RectangleF((float)barLeftPixel, (float)barTopPixel, (float)barWidthPx, (float)barHeightPx);
                    settings.gfxData.FillRectangle(setBrushes[setIndex], rect);
                    if (outlineWidth > 0)
                        settings.gfxData.DrawRectangle(setOutlinePens[setIndex], rect.X, rect.Y, rect.Width, rect.Height);

                    // draw the errorbar
                    if (datasets[setIndex].errors != null)
                    {
                        double valueErrorTop = value + datasets[setIndex].errors[groupIndex];
                        double valueErrorBot = value - datasets[setIndex].errors[groupIndex];
                        double errorPixelTop = settings.GetPixelY(valueErrorTop);
                        double errorPixelBot = settings.GetPixelY(valueErrorBot);
                        settings.gfxData.DrawLine(setErrorPens[setIndex], (float)barCenterPixel, (float)errorPixelTop, (float)barCenterPixel, (float)errorPixelBot);
                        if (errorCapSize > 0)
                        {
                            settings.gfxData.DrawLine(setErrorPens[setIndex], (float)(barCenterPixel - errorCapSize), (float)errorPixelTop, (float)(barCenterPixel + errorCapSize), (float)errorPixelTop);
                            settings.gfxData.DrawLine(setErrorPens[setIndex], (float)(barCenterPixel - errorCapSize), (float)errorPixelBot, (float)(barCenterPixel + errorCapSize), (float)errorPixelBot);
                        }
                    }
                }
            }
        }

        public void RenderHorizontalGrouped(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double barGroupThickness = 1 - interGroupSpaceFrac;
            double barThickness = barGroupThickness / barSetCount;

            for (int setIndex = 0; setIndex < barSetCount; setIndex++)
            {
                // set bar style for this whole series
                double thisSetOffset = setIndex * barThickness;

                for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
                {
                    // draw the bar for every group

                    // determine the width and horizontal offset of this bar
                    double fixedOffset = (barThickness / 2) * barSetCount;
                    double thisGroupOffset = groupIndex;
                    double barEdge1 = thisGroupOffset + thisSetOffset - fixedOffset;
                    double barEdge2 = barEdge1 + barThickness;
                    double barEdgePx1 = settings.GetPixelY(barEdge1);
                    double barEdgePx2 = settings.GetPixelY(barEdge2);
                    double barThicknessPx = barEdgePx1 - barEdgePx2;
                    double barCenterPx = (barEdgePx1 + barEdgePx2) / 2;

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
                    double barValuePixel = settings.GetPixelX(valueMax);
                    double barBasePixel = settings.GetPixelX(valueMin);
                    double barValuePx = barValuePixel - barBasePixel;

                    // draw the rectangle
                    var rect = new System.Drawing.RectangleF((float)barBasePixel, (float)barEdgePx2, (float)barValuePx, (float)barThicknessPx);
                    settings.gfxData.FillRectangle(setBrushes[setIndex], rect);
                    if (outlineWidth > 0)
                        settings.gfxData.DrawRectangle(setOutlinePens[setIndex], rect.X, rect.Y, rect.Width, rect.Height);

                    // draw the errorbar
                    if (datasets[setIndex].errors != null)
                    {

                        double valueErrorMax = value + datasets[setIndex].errors[groupIndex];
                        double valueErrorMin = value - datasets[setIndex].errors[groupIndex];
                        double valueErrorMaxPx = settings.GetPixelX(valueErrorMax);
                        double valueErrorMinPx = settings.GetPixelX(valueErrorMin);
                        settings.gfxData.DrawLine(setErrorPens[setIndex], (float)valueErrorMinPx, (float)barCenterPx, (float)valueErrorMaxPx, (float)barCenterPx);
                        if (errorCapSize > 0)
                        {
                            //settings.gfxData.DrawLine(setErrorPens[setIndex], (float)(barCenterPixel - errorCapSize), (float)errorPixelBot, (float)(barCenterPixel + errorCapSize), (float)errorPixelBot);
                            settings.gfxData.DrawLine(setErrorPens[setIndex], (float)valueErrorMinPx, (float)(barCenterPx - errorCapSize), (float)valueErrorMinPx, (float)(barCenterPx + errorCapSize));
                            settings.gfxData.DrawLine(setErrorPens[setIndex], (float)valueErrorMaxPx, (float)(barCenterPx - errorCapSize), (float)valueErrorMaxPx, (float)(barCenterPx + errorCapSize));
                        }
                    }
                }
            }
        }

        public void RenderVerticalStacked(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double barWidthFrac = 1 - interGroupSpaceFrac;

            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                // determine the width of the bar
                double xOffset = barWidthFrac / 2;
                double barLeft = groupIndex - xOffset;
                double barRight = barLeft + barWidthFrac;
                double barLeftPixel = settings.GetPixelX(barLeft);
                double barRightPixel = settings.GetPixelX(barRight);

                double yOffset = 0;
                for (int setIndex = 0; setIndex < barSetCount; setIndex++)
                {
                    // determine the height of the bar
                    double value = datasets[setIndex].values[groupIndex];
                    double barTopPixel = settings.GetPixelY(value + yOffset);
                    double barBotPixel = settings.GetPixelY(yOffset);
                    yOffset += value;

                    // draw the bar rectangle
                    double barWidthPx = barRightPixel - barLeftPixel;
                    double barHeightPx = barBotPixel - barTopPixel;

                    var rect = new System.Drawing.RectangleF((float)barLeftPixel, (float)barTopPixel, (float)barWidthPx, (float)barHeightPx);
                    settings.gfxData.FillRectangle(setBrushes[setIndex], rect);
                    if (outlineWidth > 0)
                        settings.gfxData.DrawRectangle(setOutlinePens[setIndex], rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public void RenderHorizontalStacked(Settings settings)
        {
            // define how wide the bar graphs and spaces should be
            double groupThicknessFrac = 1 - interGroupSpaceFrac;
            double groupOffset = groupThicknessFrac / 2;

            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                // determine the width of the bar
                double thisGroupEdge1 = groupIndex - groupOffset;
                double thisGroupEdge2 = thisGroupEdge1 + groupThicknessFrac;
                double thisGroupPx1 = settings.GetPixelY(thisGroupEdge1);
                double thisGroupPx2 = settings.GetPixelY(thisGroupEdge2);
                double thisBarThickness = thisGroupPx1 - thisGroupPx2;

                double startingValue = 0;
                for (int setIndex = 0; setIndex < barSetCount; setIndex++)
                {
                    // determine the height of the bar
                    double value = datasets[setIndex].values[groupIndex];
                    double thisSetPx2 = settings.GetPixelX(value + startingValue);
                    double thisSetPx1 = settings.GetPixelX(startingValue);
                    double thisBarValuePx = thisSetPx2 - thisSetPx1;
                    startingValue += value;

                    var rect = new System.Drawing.RectangleF((float)thisSetPx1, (float)thisGroupPx2, (float)thisBarValuePx, (float)thisBarThickness);
                    settings.gfxData.FillRectangle(setBrushes[setIndex], rect);
                    if (outlineWidth > 0)
                        settings.gfxData.DrawRectangle(setOutlinePens[setIndex], rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableBar with {groupCount} groups and {barSetCount} bars per group";
        }

        public override int GetPointCount()
        {
            return groupCount * barSetCount;
        }

        public override LegendItem[] GetLegendItems()
        {
            var items = new List<LegendItem>();

            for (int i = 0; i < barSetCount; i++)
                items.Add(new LegendItem(datasets[i].label, setColors[i], lineWidth: 10, markerShape: MarkerShape.none));

            return items.ToArray();
        }
    }
}
