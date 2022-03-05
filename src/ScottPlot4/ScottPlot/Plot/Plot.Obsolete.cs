/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using ScottPlot.Plottable;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        [Obsolete("Use AddHeatmapCoordinated (note capitalization)")]
        [EditorBrowsable(EditorBrowsableState.Never)] // Prevents suggestions in Intellisense for downstream users. Still shows up while editing this assembly.
        public CoordinatedHeatmap AddHeatMapCoordinated(double?[,] intensities, double? xMin = null, double? xMax = null, double? yMin = null, double? yMax = null, Drawing.Colormap colormap = null)
        {
            return AddHeatmapCoordinated(intensities, xMin, xMax, yMin, yMax, colormap);
        }

        [Obsolete("Use AddHeatmapCoordinated (note capitalization)")]
        [EditorBrowsable(EditorBrowsableState.Never)] // Prevents suggestions in Intellisense for downstream users. Still shows up while editing this assembly.
        public CoordinatedHeatmap AddHeatMapCoordinated(double[,] intensities, double? xMin = null, double? xMax = null, double? yMin = null, double? yMax = null, Drawing.Colormap colormap = null)
        {
            return AddHeatmapCoordinated(intensities, xMin, xMax, yMin, yMax, colormap);
        }


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
            fontColor ??= Color.Black;
            fillColor ??= Color.Yellow;
            lineColor ??= Color.Black;

            fontColor = Color.FromArgb((int)(255 * fontAlpha), fontColor.Value.R, fontColor.Value.G, fontColor.Value.B);
            fillColor = Color.FromArgb((int)(255 * fillAlpha), fillColor.Value.R, fillColor.Value.G, fillColor.Value.B);
            lineColor = Color.FromArgb((int)(255 * lineAlpha), lineColor.Value.R, lineColor.Value.G, lineColor.Value.B);

            var plottable = new Annotation()
            {
                X = xPixel,
                Y = yPixel,
                Label = label,
                Background = fill,
                BackgroundColor = fillColor.Value,
                BorderWidth = (float)lineWidth,
                BorderColor = lineColor.Value,
                Shadow = shadow
            };

            plottable.Font.Size = (float)fontSize;
            plottable.Font.Name = fontName;
            plottable.Font.Color = fontColor.Value;

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
                BarWidth = barWidth,
                XOffset = xOffset,
                FillColor = fillColor ?? nextColor,
                Label = label,
                ErrorLineWidth = (float)errorLineWidth,
                ErrorCapSize = errorCapSize,
                ErrorColor = errorColor ?? Color.Black,
                BorderLineWidth = (float)outlineWidth,
                BorderColor = outlineColor ?? Color.Black,
                VerticalOrientation = !horizontal,
                ShowValuesAboveBars = showValues,
                FillColorNegative = negativeColor ?? nextColor
            };
            barPlot.Font.Color = valueColor ?? Color.Black;
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

        [Obsolete("Use AddImage() and customize the object it returns.")]
        public Plottable.Image PlotBitmap(
           Bitmap bitmap,
           double x,
           double y,
           string label = null,
           Alignment alignment = Alignment.MiddleLeft,
           double rotation = 0,
           Color? frameColor = null,
           int frameSize = 0
           )
        {
            Plottable.Image plottableImage = new Plottable.Image()
            {
                Bitmap = bitmap,
                X = x,
                Y = y,
                Label = label,
                Alignment = alignment,
                Rotation = rotation,
                BorderColor = frameColor ?? Color.White,
                BorderSize = frameSize
            };

            settings.Plottables.Add(plottableImage);
            return plottableImage;
        }

        [Obsolete("use AddCandlesticks() and customize the object it returns")]
        public FinancePlot PlotCandlestick(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            FinancePlot plottable = new FinancePlot(ohlcs)
            {
                Candle = true,
                Sequential = sequential,
                ColorUp = colorUp ?? ColorTranslator.FromHtml("#26a69a"),
                ColorDown = colorDown ?? ColorTranslator.FromHtml("#ef5350")
            };
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddScatter() and customize it for no line, no marker, and errorbars as desired")]
        public void PlotErrorBars(
            double[] xs,
            double[] ys,
            double[] xPositiveError = null,
            double[] xNegativeError = null,
            double[] yPositiveError = null,
            double[] yNegativeError = null,
            Color? color = null,
            double lineWidth = 1,
            double capWidth = 3,
            string label = null
            ) => throw new NotImplementedException();

        [Obsolete("Use AddFill() and customize the object it returns")]
        public Polygon PlotFill(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);

            return PlotPolygon(xs2, ys2, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        [Obsolete("Use AddFill() and customize the object it returns")]
        public Polygon PlotFill(
            double[] xs1,
            double[] ys1,
            double[] xs2,
            double[] ys2,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if ((xs1.Length != ys1.Length) || (xs2.Length != ys2.Length))
                throw new ArgumentException("xs and ys for each dataset must have the same length");

            int pointCount = xs1.Length + xs2.Length;
            double[] bothX = new double[pointCount];
            double[] bothY = new double[pointCount];

            // copy the first dataset as-is
            Array.Copy(xs1, 0, bothX, 0, xs1.Length);
            Array.Copy(ys1, 0, bothY, 0, ys1.Length);

            // copy the second dataset in reverse order
            for (int i = 0; i < xs2.Length; i++)
            {
                bothX[xs1.Length + i] = xs2[xs2.Length - 1 - i];
                bothY[ys1.Length + i] = ys2[ys2.Length - 1 - i];
            }

            return PlotPolygon(bothX, bothY, label, lineWidth, lineColor, fill, fillColor, fillAlpha);
        }

        [Obsolete("Use AddFill() and customize the object it returns")]
        public (Polygon, Polygon) PlotFillAboveBelow(
            double[] xs,
            double[] ys,
            string labelAbove = null,
            string labelBelow = null,
            double lineWidth = 1,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColorAbove = null,
            Color? fillColorBelow = null,
            double fillAlpha = 1,
            double baseline = 0
            )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, cloneEdges: true);
            double[] ys2 = Tools.Pad(ys, padWithLeft: baseline, padWithRight: baseline);

            double[] ys2below = new double[ys2.Length];
            double[] ys2above = new double[ys2.Length];
            for (int i = 0; i < ys2.Length; i++)
            {
                if (ys2[i] < baseline)
                {
                    ys2below[i] = ys2[i];
                    ys2above[i] = baseline;
                }
                else
                {
                    ys2above[i] = ys2[i];
                    ys2below[i] = baseline;
                }
            }

            if (fillColorAbove is null)
                fillColorAbove = Color.Green;
            if (fillColorBelow is null)
                fillColorBelow = Color.Red;
            if (lineColor is null)
                lineColor = Color.Black;

            var polyAbove = PlotPolygon(xs2, ys2above, labelAbove, lineWidth, lineColor, fill, fillColorAbove, fillAlpha);
            var polyBelow = PlotPolygon(xs2, ys2below, labelBelow, lineWidth, lineColor, fill, fillColorBelow, fillAlpha);

            return (polyBelow, polyAbove);
        }

        [Obsolete("Use AddFill() and customize the object it returns")]
        public (Polygon, Polygon) PlotFillRightLeft(
           double[] xs,
           double[] ys,
           string labelRight = null,
           string labelLeft = null,
           double lineWidth = 1,
           Color? lineColor = null,
           bool fill = true,
           Color? fillColorRight = null,
           Color? fillColorLeft = null,
           double fillAlpha = 1,
           double baseline = 0
           )
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must all have the same length");

            double[] xs2 = Tools.Pad(xs, padWithLeft: baseline, padWithRight: baseline);
            double[] ys2 = Tools.Pad(ys, cloneEdges: true);

            double[] xs2below = new double[xs2.Length];
            double[] xs2above = new double[xs2.Length];

            for (int i = 0; i < xs2.Length; i++)
            {
                if (xs2[i] < baseline)
                {
                    xs2below[i] = xs2[i];
                    xs2above[i] = baseline;
                }
                else
                {
                    xs2above[i] = xs2[i];
                    xs2below[i] = baseline;
                }
            }

            if (fillColorRight is null)
                fillColorRight = Color.Green;
            if (fillColorLeft is null)
                fillColorLeft = Color.Red;
            if (lineColor is null)
                lineColor = Color.Black;

            var polyRight = PlotPolygon(xs2above, ys2, labelRight, lineWidth, lineColor, fill, fillColorRight, fillAlpha);
            var polyLeft = PlotPolygon(xs2below, ys2, labelLeft, lineWidth, lineColor, fill, fillColorLeft, fillAlpha);

            return (polyLeft, polyRight);
        }

        [Obsolete("Use AddFunction() and customize the object it returns")]
        public FunctionPlot PlotFunction(
            Func<double, double?> function,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 0,
            string label = "f(x)",
            MarkerShape markerShape = MarkerShape.none,
            LineStyle lineStyle = LineStyle.Solid
        )
        {
            if (markerShape != MarkerShape.none || markerSize != 0)
                throw new ArgumentException("function plots do not use markers");

            FunctionPlot functionPlot = new FunctionPlot(function)
            {
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                LineStyle = lineStyle,
                Label = label
            };

            Add(functionPlot);
            return functionPlot;
        }

        [Obsolete("Use AddHeatmap() and customize the object it returns")]
        public Heatmap PlotHeatmap(
            double[,] intensities,
            Drawing.Colormap colormap = null,
            string label = null,
            double[] axisOffsets = null,
            double[] axisMultipliers = null,
            double? scaleMin = null,
            double? scaleMax = null,
            double? transparencyThreshold = null,
            Bitmap backgroundImage = null,
            bool displayImageAbove = false,
            bool drawAxisLabels = true
            )
        {
            double?[,] tmp = new double?[intensities.GetLength(0), intensities.GetLength(1)];
            for (int i = 0; i < intensities.GetLength(0); i++)
            {
                for (int j = 0; j < intensities.GetLength(1); j++)
                {
                    tmp[i, j] = intensities[i, j];
                }
            }

            return PlotHeatmap(tmp, colormap, label, axisOffsets, axisMultipliers, scaleMin, scaleMax, transparencyThreshold, backgroundImage, displayImageAbove, drawAxisLabels);
        }

        [Obsolete("Create this plottable manually with new, then Add() it to the plot.")]
        public Heatmap PlotHeatmap(
            double?[,] intensities,
            Drawing.Colormap colormap = null,
            string label = null,
            double[] axisOffsets = null,
            double[] axisMultipliers = null,
            double? scaleMin = null,
            double? scaleMax = null,
            double? transparencyThreshold = null,
            Bitmap backgroundImage = null,
            bool displayImageAbove = false,
            bool drawAxisLabels = true
            )
        {
            Heatmap heatmap = new Heatmap()
            {
                Label = label,
                TransparencyThreshold = transparencyThreshold,
                BackgroundImage = backgroundImage,
                DisplayImageAbove = displayImageAbove,
                ShowAxisLabels = drawAxisLabels,
            };
            heatmap.Update(intensities, colormap ?? Drawing.Colormap.Viridis, scaleMin, scaleMax);

            Add(heatmap);
            Layout(top: 180);

            return heatmap;
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
                Y = y,
                Color = color ?? settings.GetNextColor(),
                LineWidth = (float)lineWidth,
                Label = label,
                DragEnabled = draggable,
                LineStyle = lineStyle,
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
                X1 = x1,
                X2 = x2,
                Color = color ?? GetNextColor(alpha),
                Label = label,
                DragEnabled = draggable,
                DragFixedSize = dragFixedSize,
                DragLimitMin = dragLimitLower,
                DragLimitMax = dragLimitUpper,
            };
            Add(axisSpan);
            return axisSpan;
        }

        [Obsolete("use AddLine() and customize the object it returns")]
        public ScatterPlot PlotLine(
            double x1,
            double y1,
            double x2,
            double y2,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            return PlotScatter(
                xs: new double[] { x1, x2 },
                ys: new double[] { y1, y2 },
                color: color,
                lineWidth: lineWidth,
                label: label,
                lineStyle: lineStyle,
                markerSize: 0
                );
        }

        [Obsolete("use AddLine() and customize the object it returns")]
        public ScatterPlot PlotLine(
            double slope,
            double offset,
            (double x1, double x2) xLimits,
            Color? color = null,
            double lineWidth = 1,
            string label = null,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            double y1 = xLimits.x1 * slope + offset;
            double y2 = xLimits.x2 * slope + offset;
            return PlotScatter(
                xs: new double[] { xLimits.x1, xLimits.x2 },
                ys: new double[] { y1, y2 },
                color: color,
                lineWidth: lineWidth,
                label: label,
                lineStyle: lineStyle,
                markerSize: 0
                );
        }

        [Obsolete("use AddOHLC() and customize the object it returns")]
        public FinancePlot PlotOHLC(
            OHLC[] ohlcs,
            Color? colorUp = null,
            Color? colorDown = null,
            bool autoWidth = true,
            bool sequential = false
            )
        {
            FinancePlot plottable = new FinancePlot(ohlcs)
            {
                Candle = false,
                Sequential = sequential,
                ColorUp = colorUp ?? ColorTranslator.FromHtml("#26a69a"),
                ColorDown = colorDown ?? ColorTranslator.FromHtml("#ef5350")
            };
            Add(plottable);
            return plottable;
        }

        [Obsolete("use AddPie() and customize the object it returns")]
        public PiePlot PlotPie(
            double[] values,
            string[] sliceLabels = null,
            Color[] colors = null,
            bool explodedChart = false,
            bool showValues = false,
            bool showPercentages = false,
            bool showLabels = true,
            string label = null
            )
        {
            colors ??= Enumerable.Range(0, values.Length).Select(i => settings.PlottablePalette.GetColor(i)).ToArray();

            PiePlot pie = new PiePlot(values, sliceLabels, colors)
            {
                Explode = explodedChart,
                ShowValues = showValues,
                ShowPercentages = showPercentages,
                ShowLabels = showLabels,
                Label = label
            };

            Add(pie);
            return pie;
        }

        [Obsolete("Use AddPoint() and customize the object it returns")]
        public ScatterPlot PlotPoint(double x, double y, Color? color = null, double markerSize = 5, string label = null,
            double? errorX = null, double? errorY = null, double errorLineWidth = 1, double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle, LineStyle lineStyle = LineStyle.Solid)
            => throw new NotImplementedException();

        [Obsolete("Use AddScatter() and customize the object it returns")]
        public ScatterPlot PlotScatter(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            double[] errorX = null,
            double[] errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            var scatterPlot = new ScatterPlot(xs, ys, errorX, errorY)
            {
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                ErrorLineWidth = (float)errorLineWidth,
                ErrorCapSize = (float)errorCapSize,
                StepDisplay = false,
                MarkerShape = markerShape,
                LineStyle = lineStyle
            };

            Add(scatterPlot);
            return scatterPlot;
        }
        [Obsolete("AddScatter() then AddPoint() and move the point around")]
        public ScatterPlotHighlight PlotScatterHighlight(
           double[] xs,
           double[] ys,
           Color? color = null,
           double lineWidth = 1,
           double markerSize = 5,
           string label = null,
           double[] errorX = null,
           double[] errorY = null,
           double errorLineWidth = 1,
           double errorCapSize = 3,
           MarkerShape markerShape = MarkerShape.filledCircle,
           LineStyle lineStyle = LineStyle.Solid,
           MarkerShape highlightedShape = MarkerShape.openCircle,
           Color? highlightedColor = null,
           double? highlightedMarkerSize = null
           )
        {
            if (color is null)
                color = settings.GetNextColor();

            if (highlightedColor is null)
                highlightedColor = Color.Red;

            if (highlightedMarkerSize is null)
                highlightedMarkerSize = 2 * markerSize;

            var scatterPlot = new ScatterPlotHighlight(xs, ys, errorX, errorY)
            {
                Color = (Color)color,
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                ErrorLineWidth = (float)errorLineWidth,
                ErrorCapSize = (float)errorCapSize,
                StepDisplay = false,
                MarkerShape = markerShape,
                LineStyle = lineStyle,
                highlightedShape = highlightedShape,
                highlightedColor = highlightedColor.Value,
                highlightedMarkerSize = (float)highlightedMarkerSize.Value
            };

            Add(scatterPlot);
            return scatterPlot;
        }

        [Obsolete("Use AddSignal() and customize the object it returns")]
        public SignalPlot PlotSignal(
            double[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            double yOffset = 0,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            SignalPlot signal = new SignalPlot()
            {
                Ys = ys,
                SampleRate = sampleRate,
                OffsetX = xOffset,
                OffsetY = yOffset,
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                DensityColors = colorByDensity,
                MinRenderIndex = minRenderIndex ?? 0,
                MaxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                LineStyle = lineStyle,
                UseParallel = useParallel,
            };

            Add(signal);
            return signal;
        }

        [Obsolete("Use AddSignalConst() and customize the object it returns")]
        public SignalPlotConst<T> PlotSignalConst<T>(
            T[] ys,
            double sampleRate = 1,
            double xOffset = 0,
            T yOffset = default(T),
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            Color[] colorByDensity = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where T : struct, IComparable
        {
            SignalPlotConst<T> signal = new SignalPlotConst<T>()
            {
                Ys = ys,
                SampleRate = sampleRate,
                OffsetX = xOffset,
                OffsetY = yOffset,
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                DensityColors = colorByDensity,
                MinRenderIndex = minRenderIndex ?? 0,
                MaxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                LineStyle = lineStyle,
                UseParallel = useParallel
            };

            Add(signal);
            return signal;
        }

        [Obsolete("Use AddSignalXY() and customize the object it returns")]
        public SignalPlotXY PlotSignalXY(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            )
        {
            SignalPlotXY signal = new SignalPlotXY()
            {
                Xs = xs,
                Ys = ys,
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                MinRenderIndex = minRenderIndex ?? 0,
                MaxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                LineStyle = lineStyle,
                UseParallel = useParallel
            };

            Add(signal);
            return signal;
        }

        [Obsolete("Use AddSignalXYConst() and customize the object it returns")]
        public SignalPlotXYConst<TX, TY> PlotSignalXYConst<TX, TY>(
            TX[] xs,
            TY[] ys,
            Color? color = null,
            double lineWidth = 1,
            double markerSize = 5,
            string label = null,
            int? minRenderIndex = null,
            int? maxRenderIndex = null,
            LineStyle lineStyle = LineStyle.Solid,
            bool useParallel = true
            ) where TX : struct, IComparable where TY : struct, IComparable

        {
            SignalPlotXYConst<TX, TY> signal = new SignalPlotXYConst<TX, TY>()
            {
                Xs = xs,
                Ys = ys,
                Color = color ?? settings.GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = (float)markerSize,
                Label = label,
                MinRenderIndex = minRenderIndex ?? 0,
                MaxRenderIndex = maxRenderIndex ?? ys.Length - 1,
                LineStyle = lineStyle,
                UseParallel = useParallel
            };

            Add(signal);
            return signal;
        }

        [Obsolete("Use AddScatterStep() and customize the object it returns")]
        public ScatterPlot PlotStep(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            ScatterPlot stepPlot = new ScatterPlot(xs, ys)
            {
                Color = (Color)color,
                LineWidth = lineWidth,
                MarkerSize = 0,
                Label = label,
                ErrorLineWidth = 0,
                ErrorCapSize = 0,
                StepDisplay = true,
                MarkerShape = MarkerShape.none,
                LineStyle = LineStyle.Solid
            };

            Add(stepPlot);
            return stepPlot;
        }

        [Obsolete("Use AddPolygon() and customize the object it returns")]
        public Polygon PlotPolygon(
            double[] xs,
            double[] ys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            var plottable = new Polygon(xs, ys)
            {
                Label = label,
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black,
                Fill = fill,
                FillColor = fillColor ?? GetNextColor(fillAlpha),
            };

            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPolygons() and customize the object it returns")]
        public Polygons PlotPolygons(
            List<List<(double x, double y)>> polys,
            string label = null,
            double lineWidth = 0,
            Color? lineColor = null,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = 1
            )
        {
            var plottable = new Polygons(polys)
            {
                Label = label,
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black,
                Fill = fill,
                FillColor = Color.FromArgb((byte)(255 * fillAlpha), fillColor ?? GetNextColor()),
            };

            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulation() and customize the object it returns")]
        public PopulationPlot PlotPopulations(Population population, string label = null)
        {
            var plottable = new PopulationPlot(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulations() and customize the object it returns")]
        public PopulationPlot PlotPopulations(Population[] populations, string label = null)
        {
            var plottable = new PopulationPlot(populations, label);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulations() and customize the object it returns")]
        public PopulationPlot PlotPopulations(PopulationSeries series, string label = null)
        {
            series.color = settings.GetNextColor();
            if (label != null)
                series.seriesLabel = label;
            var plottable = new PopulationPlot(series);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddPopulations() and customize the object it returns")]
        public PopulationPlot PlotPopulations(PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.PlottablePalette.GetColor(i);

            var plottable = new PopulationPlot(multiSeries);
            Add(plottable);
            return plottable;
        }

        [Obsolete("Use AddRader() and customize the object it returns")]
        public RadarPlot PlotRadar(
            double[,] values,
            string[] categoryNames = null,
            string[] groupNames = null,
            Color[] fillColors = null,
            double fillAlpha = .4,
            Color? webColor = null,
            bool independentAxes = false,
            double[] maxValues = null
            )
        {
            Color[] colors = fillColors ?? Enumerable.Range(0, values.Length).Select(i => settings.PlottablePalette.GetColor(i)).ToArray();
            Color[] colorsAlpha = colors.Select(x => Color.FromArgb((byte)(255 * fillAlpha), x)).ToArray();

            var plottable = new RadarPlot(values, colors, fillColors ?? colorsAlpha, independentAxes, maxValues)
            {
                CategoryLabels = categoryNames,
                GroupLabels = groupNames,
                WebColor = webColor ?? Color.Gray
            };
            Add(plottable);

            return plottable;
        }

        [Obsolete("use AddScalebar() and customize the object it returns")]
        public ScaleBar PlotScaleBar(
            double sizeX,
            double sizeY,
            string labelX = null,
            string labelY = null,
            double thickness = 2,
            double fontSize = 12,
            Color? color = null,
            double padPx = 10
            )
        {
            var scalebar = new ScaleBar()
            {
                Width = sizeX,
                Height = sizeY,
                HorizontalLabel = labelX,
                VerticalLabel = labelY,
                LineWidth = (float)thickness,
                FontSize = (float)fontSize,
                FontColor = color ?? Color.Black,
                LineColor = color ?? Color.Black,
                Padding = (float)padPx
            };
            Add(scalebar);
            return scalebar;
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
                Label = text,
                X = x,
                Y = y,
                Color = color ?? settings.GetNextColor(),
                FontName = fontName,
                FontSize = (float)fontSize,
                FontBold = bold,
                Alignment = alignment,
                Rotation = (float)rotation,
                BackgroundFill = frame,
                BackgroundColor = frameColor ?? Color.White
            };
            Add(plottableText);
            return plottableText;
        }

        [Obsolete("Create a VectorField manually then call Add()")]
        public VectorField PlotVectorField(
            Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Drawing.Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            var vectorField = new VectorField(vectors, xs, ys,
                colormap, scaleFactor, color ?? settings.GetNextColor())
            { Label = label };

            Add(vectorField);
            return vectorField;
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
                X = x,
                Color = color ?? settings.GetNextColor(),
                LineWidth = (float)lineWidth,
                Label = label,
                DragEnabled = draggable,
                LineStyle = lineStyle,
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
                Y1 = y1,
                Y2 = y2,
                Color = color ?? GetNextColor(alpha),
                Label = label,
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
