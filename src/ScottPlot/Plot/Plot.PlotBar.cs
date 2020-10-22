/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using ScottPlot.Drawing;
using ScottPlot.Statistics;

namespace ScottPlot
{
    public partial class Plot
    {
        public PlottableBar PlotBar(
            double[] xs,
            double[] ys,
            double[] errorY = null,
            string label = null,
            double barWidth = .8,
            double xOffset = 0,
            bool fill = true,
            Color? fillColor = null,
            double outlineWidth = 1,
            Color? outlineColor = null,
            double errorLineWidth = 1,
            double errorCapSize = .38,
            Color? errorColor = null,
            bool horizontal = false,
            bool showValues = false,
            Color? valueColor = null,
            bool autoAxis = true,
            double[] yOffsets = null,
            Color? negativeColor = null
            )
        {
            PlottableBar barPlot = new PlottableBar(xs, ys, errorY, yOffsets)
            {
                barWidth = barWidth,
                xOffset = xOffset,
                fill = fill,
                fillColor = fillColor ?? settings.GetNextColor(),
                label = label,
                errorLineWidth = (float)errorLineWidth,
                errorCapSize = errorCapSize,
                errorColor = errorColor ?? Color.Black,
                borderLineWidth = (float)outlineWidth,
                borderColor = outlineColor ?? Color.Black,
                verticalBars = !horizontal,
                showValues = showValues,
                FontColor = valueColor ?? Color.Black,
                negativeColor = negativeColor ?? Color.Black
            };
            Add(barPlot);

            if (autoAxis)
            {
                // perform a tight axis adjustment
                AxisAuto(0, 0);
                double[] tightAxisLimits = Axis();

                // now loosen it up a bit
                AxisAuto();

                // now set one of the axis edges to zero
                if (horizontal)
                {
                    if (tightAxisLimits[0] == 0)
                        Axis(x1: 0);
                    else if (tightAxisLimits[1] == 0)
                        Axis(x2: 0);
                }
                else
                {
                    if (tightAxisLimits[2] == 0)
                        Axis(y1: 0);
                    else if (tightAxisLimits[3] == 0)
                        Axis(y2: 0);
                }
            }

            return barPlot;
        }

        public PlottableBar PlotWaterfall(
            double[] xs,
            double[] ys,
            double[] errorY = null,
            string label = null,
            double barWidth = .8,
            double xOffset = 0,
            bool fill = true,
            Color? fillColor = null,
            double outlineWidth = 1,
            Color? outlineColor = null,
            double errorLineWidth = 1,
            double errorCapSize = .38,
            Color? errorColor = null,
            bool horizontal = false,
            bool showValues = false,
            Color? valueColor = null,
            bool autoAxis = true,
            Color? negativeColor = null
            )
        {
            double[] yOffsets = Enumerable.Range(0, ys.Length).Select(count => ys.Take(count).Sum()).ToArray();
            return PlotBar(
                xs,
                ys,
                errorY,
                label,
                barWidth,
                xOffset,
                fill,
                fillColor,
                outlineWidth,
                outlineColor,
                errorLineWidth,
                errorCapSize,
                errorColor,
                horizontal,
                showValues,
                valueColor,
                autoAxis,
                yOffsets,
                negativeColor
            );
        }

        /// <summary>
        /// Create a series of bar plots given a 2D dataset
        /// </summary>
        /// <param name="groupLabels">displayed as horizontal axis tick labels</param>
        /// <param name="seriesLabels">displayed in the legend</param>
        /// <param name="ys">Array of arrays (one per series) that contan one point per group</param>
        /// <returns></returns>
        public PlottableBar[] PlotBarGroups(
                string[] groupLabels,
                string[] seriesLabels,
                double[][] ys,
                double[][] yErr = null,
                double groupWidthFraction = 0.8,
                double barWidthFraction = 0.8,
                double errorCapSize = 0.38,
                bool showValues = false
            )
        {
            if (groupLabels is null || seriesLabels is null || ys is null)
                throw new ArgumentException("labels and ys cannot be null");

            if (seriesLabels.Length != ys.Length)
                throw new ArgumentException("groupLabels and ys must be the same length");

            foreach (double[] subArray in ys)
                if (subArray.Length != groupLabels.Length)
                    throw new ArgumentException("all arrays inside ys must be the same length as groupLabels");

            int seriesCount = ys.Length;
            double barWidth = groupWidthFraction / seriesCount;
            PlottableBar[] bars = new PlottableBar[seriesCount];
            bool containsNegativeY = false;
            for (int i = 0; i < seriesCount; i++)
            {
                double offset = i * barWidth;
                double[] barYs = ys[i];
                double[] barYerr = yErr?[i];
                double[] barXs = DataGen.Consecutive(barYs.Length);
                containsNegativeY |= barYs.Where(y => y < 0).Any();
                bars[i] = PlotBar(barXs, barYs, barYerr, seriesLabels[i], barWidth * barWidthFraction, offset,
                    errorCapSize: errorCapSize, showValues: showValues);
            }

            if (containsNegativeY)
                AxisAuto();

            double[] groupPositions = DataGen.Consecutive(groupLabels.Length, offset: (groupWidthFraction - barWidth) / 2);
            XTicks(groupPositions, groupLabels);

            return bars;
        }

        public PlottablePopulations PlotPopulations(Population population, string label = null)
        {
            var plottable = new PlottablePopulations(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(Population[] populations, string label = null)
        {
            var plottable = new PlottablePopulations(populations, label);
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(PopulationSeries series, string label = null)
        {
            series.color = settings.GetNextColor();
            if (label != null)
                series.seriesLabel = label;
            var plottable = new PlottablePopulations(series);
            Add(plottable);
            return plottable;
        }

        public PlottablePopulations PlotPopulations(PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.colorset.GetColor(i);

            var plottable = new PlottablePopulations(multiSeries);
            Add(plottable);
            return plottable;
        }

    }
}
