/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using ScottPlot.Plottable;
using ScottPlot.Statistics;

namespace ScottPlot
{
    public partial class Plot
    {
        [Obsolete("Use AddAnnotation() and customize the object it returns")]
        public Annotation PlotAnnotation(
            string label,
            double xPixel = 10,
            double yPixel = 10,
            double fontSize = 12,
            string fontName = "Segoe UI",
            Color? fontColor = null,
            double fontAlpha = 1,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = .2,
            double lineWidth = 1,
            Color? lineColor = null,
            double lineAlpha = 1,
            bool shadow = false
            )
        {
            fontColor = fontColor ?? Color.Black;
            fillColor = fillColor ?? Color.Yellow;
            lineColor = lineColor ?? Color.Black;

            fontColor = Color.FromArgb((int)(255 * fontAlpha), fontColor.Value.R, fontColor.Value.G, fontColor.Value.B);
            fillColor = Color.FromArgb((int)(255 * fillAlpha), fillColor.Value.R, fillColor.Value.G, fillColor.Value.B);
            lineColor = Color.FromArgb((int)(255 * lineAlpha), lineColor.Value.R, lineColor.Value.G, lineColor.Value.B);

            var plottable = new Annotation()
            {
                xPixel = xPixel,
                yPixel = yPixel,
                label = label,
                FontSize = (float)fontSize,
                FontName = fontName,
                FontColor = fontColor.Value,
                Background = fill,
                BackgroundColor = fillColor.Value,
                BorderWidth = (float)lineWidth,
                BorderColor = lineColor.Value,
                Shadow = shadow
            };

            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddArrow() and customize the object it returns")]
        public ScatterPlot PlotArrow(
            double tipX,
            double tipY,
            double baseX,
            double baseY,
            double lineWidth = 5,
            float arrowheadWidth = 3,
            float arrowheadLength = 3,
            Color? color = null,
            string label = null
            )
        {
            var scatter = PlotScatter(
                                        xs: new double[] { baseX, tipX },
                                        ys: new double[] { baseY, tipY },
                                        color: color,
                                        lineWidth: lineWidth,
                                        label: label,
                                        markerSize: 0
                                    );

            scatter.ArrowheadLength = arrowheadLength;
            scatter.ArrowheadWidth = arrowheadWidth;
            return scatter;
        }

        [Obsolete("Use AddBar() and customize the object it returns")]
        public BarPlot PlotBar(
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
            Color nextColor = settings.GetNextColor();
            BarPlot barPlot = new BarPlot(xs, ys, errorY, yOffsets)
            {
                barWidth = barWidth,
                xOffset = xOffset,
                fill = fill,
                fillColor = fillColor ?? nextColor,
                label = label,
                errorLineWidth = (float)errorLineWidth,
                errorCapSize = errorCapSize,
                errorColor = errorColor ?? Color.Black,
                borderLineWidth = (float)outlineWidth,
                borderColor = outlineColor ?? Color.Black,
                verticalBars = !horizontal,
                showValues = showValues,
                FontColor = valueColor ?? Color.Black,
                negativeColor = negativeColor ?? nextColor
            };
            Add(barPlot);

            if (autoAxis)
            {
                // perform a tight axis adjustment
                AxisAuto(0, 0);
                var tightAxisLimits = GetAxisLimits();

                // now loosen it up a bit
                AxisAuto();

                // now set one of the axis edges to zero
                if (horizontal)
                {
                    if (tightAxisLimits.XMin == 0)
                        SetAxisLimits(xMin: 0);
                    else if (tightAxisLimits.XMax == 0)
                        SetAxisLimits(xMax: 0);
                }
                else
                {
                    if (tightAxisLimits.YMin == 0)
                        SetAxisLimits(yMin: 0);
                    else if (tightAxisLimits.YMax == 0)
                        SetAxisLimits(yMax: 0);
                }
            }

            return barPlot;
        }

        /// <summary>
        /// Create a series of bar plots given a 2D dataset
        /// </summary>
        /// <param name="groupLabels">displayed as horizontal axis tick labels</param>
        /// <param name="seriesLabels">displayed in the legend</param>
        /// <param name="ys">Array of arrays (one per series) that contan one point per group</param>
        /// <returns></returns>
        [Obsolete("Use AddBarGroups() and customize the object it returns")]
        public BarPlot[] PlotBarGroups(
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
            BarPlot[] bars = new BarPlot[seriesCount];
            bool containsNegativeY = false;
            for (int i = 0; i < seriesCount; i++)
            {
                double offset = i * barWidth;
                double[] barYs = ys[i];
                double[] barYerr = yErr?[i];
                double[] barXs = DataGen.Consecutive(barYs.Length);
                containsNegativeY |= barYs.Where(y => y < 0).Any();
                bars[i] = PlotBar(barXs, barYs, barYerr, seriesLabels[i], barWidth * barWidthFraction, offset, errorCapSize: errorCapSize, showValues: showValues);
            }

            if (containsNegativeY)
                AxisAuto();

            double[] groupPositions = DataGen.Consecutive(groupLabels.Length, offset: (groupWidthFraction - barWidth) / 2);
            XTicks(groupPositions, groupLabels);

            return bars;
        }

        [Obsolete("Use AddHorizontalLine() and customize the object it returns")]
        public HLine PlotHLine(
            double y,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            var hline = new HLine()
            {
                position = y,
                color = color ?? settings.GetNextColor(),
                lineWidth = (float)lineWidth,
                label = label,
                DragEnabled = draggable,
                lineStyle = lineStyle,
                DragLimitMin = dragLimitLower,
                DragLimitMax = dragLimitUpper
            };
            Add(hline);
            return hline;
        }

        [Obsolete("Use AddHorizontalSpan() and customize the object it returns")]
        public HSpan PlotHSpan(
            double x1,
            double x2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            bool dragFixedSize = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            var axisSpan = new HSpan()
            {
                position1 = x1,
                position2 = x2,
                color = color ?? GetNextColor(alpha),
                label = label,
                DragEnabled = draggable,
                DragFixedSize = dragFixedSize,
                DragLimitMin = dragLimitLower,
                DragLimitMax = dragLimitUpper,
            };
            Add(axisSpan);
            return axisSpan;
        }

        [Obsolete("Use AddPopulation() and customize the object it returns")]
        public PopulationPlot PlotPopulations(Population population, string label = null)
        {
            var plottable = new PopulationPlot(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulation() and customize the object it returns")]
        public PopulationPlot PlotPopulations(Population[] populations, string label = null)
        {
            var plottable = new PopulationPlot(populations, label);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulation() and customize the object it returns")]
        public PopulationPlot PlotPopulations(PopulationSeries series, string label = null)
        {
            series.color = settings.GetNextColor();
            if (label != null)
                series.seriesLabel = label;
            var plottable = new PopulationPlot(series);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulation() and customize the object it returns")]
        public PopulationPlot PlotPopulations(PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.PlottablePalette.GetColor(i);

            var plottable = new PopulationPlot(multiSeries);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddText() and customize the object it returns")]
        public Text PlotText(
            string text,
            double x,
            double y,
            Color? color = null,
            string fontName = null,
            double fontSize = 12,
            bool bold = false,
            string label = null,
            Alignment alignment = Alignment.MiddleLeft,
            double rotation = 0,
            bool frame = false,
            Color? frameColor = null
            )
        {
            if (!string.IsNullOrWhiteSpace(label))
                Debug.WriteLine("WARNING: the PlotText() label argument is ignored");

            Text plottableText = new Text()
            {
                text = text,
                x = x,
                y = y,
                FontColor = color ?? settings.GetNextColor(),
                FontName = fontName,
                FontSize = (float)fontSize,
                FontBold = bold,
                alignment = alignment,
                rotation = (float)rotation,
                FillBackground = frame,
                BackgroundColor = frameColor ?? Color.White
            };
            Add(plottableText);
            return plottableText;
        }

        [Obsolete("Use AddVerticalLine() and customize the object it returns")]
        public VLine PlotVLine(
            double x,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            bool draggable = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            VLine axLine = new VLine()
            {
                position = x,
                color = color ?? settings.GetNextColor(),
                lineWidth = (float)lineWidth,
                label = label,
                DragEnabled = draggable,
                lineStyle = lineStyle,
                DragLimitMin = dragLimitLower,
                DragLimitMax = dragLimitUpper
            };
            Add(axLine);
            return axLine;
        }

        [Obsolete("Use AddVerticalSpan() and customize the object it returns")]
        public VSpan PlotVSpan(
            double y1,
            double y2,
            Color? color = null,
            double alpha = .5,
            string label = null,
            bool draggable = false,
            bool dragFixedSize = false,
            double dragLimitLower = double.NegativeInfinity,
            double dragLimitUpper = double.PositiveInfinity
            )
        {
            var axisSpan = new VSpan()
            {
                position1 = y1,
                position2 = y2,
                color = color ?? GetNextColor(alpha),
                label = label,
                DragEnabled = draggable,
                DragFixedSize = dragFixedSize,
                DragLimitMin = dragLimitLower,
                DragLimitMax = dragLimitUpper
            };
            Add(axisSpan);
            return axisSpan;
        }

        [Obsolete("This method has been replaced by AddBar() and one line of Linq (see cookbook)")]
        public BarPlot PlotWaterfall(
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
    }
}
