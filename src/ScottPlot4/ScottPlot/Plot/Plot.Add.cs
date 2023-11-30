/* This file contains helper methods for creating plottables, customizing them based on optional arguments, 
 * adding them to the plot, then returning them for additional customization all with a single method call.
 * 
 * Plottable-creating helper methods try to obey these rules:
 * 
 *   1. Only the most common plot types get helper methods.
 *      Uncommon or experimental plottables can be created by the user and added with Add().
 *   
 *   2. Only the most common styling options are configurable with optional arguments.
 *      This is subjective, but guided by what is in the cookbook and often seen in the wild.
 *      Plottables are always returned by helper methods, so users can customize them extensively as desired.
 *   
 */

using ScottPlot.Drawing;
using ScottPlot.Plottable;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Display text in the data area at a pixel location (not a X/Y coordinates)
        /// </summary>
        [Obsolete("This overload is deprecated.")]
        public Annotation AddAnnotation(string label, double x, double y)
        {
            var plottable = new Annotation() { Label = label };

            // recreate old X/Y behavior using the new alignment property
            if (x >= 0 && y >= 0)
                plottable.Alignment = Alignment.UpperLeft;
            else if (x < 0 && y >= 0)
                plottable.Alignment = Alignment.UpperRight;
            else if (x >= 0 && y < 0)
                plottable.Alignment = Alignment.LowerLeft;
            else if (x < 0 && y < 0)
                plottable.Alignment = Alignment.LowerRight;

            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Display text in the data area at a pixel location (not a X/Y coordinates)
        /// </summary>
        public Annotation AddAnnotation(string label, Alignment alignment = Alignment.UpperLeft)
        {
            Annotation plottable = new() { Label = label, Alignment = alignment };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Display an arrow pointing to a spot in coordinate space
        /// </summary>
        public ArrowCoordinated AddArrow(double xTip, double yTip, double xBase, double yBase, float lineWidth = 5, Color? color = null)
        {
            /*
            double[] xs = { xBase, xTip };
            double[] ys = { yBase, yTip };
            var plottable = new ScatterPlot(xs, ys)
            {
                LineWidth = lineWidth,
                MarkerSize = 0,
                Color = color ?? GetNextColor(),
                ArrowheadLength = 3,
                ArrowheadWidth = 3
            };
            */
            var plottable = new ArrowCoordinated(xBase, yBase, xTip, yTip)
            {
                LineWidth = lineWidth,
                Color = color ?? GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bracket to highlight a range between two points in coordinate space with an optional label.
        /// </summary>
        public Bracket AddBracket(double x1, double y1, double x2, double y2, string label = null)
        {
            Bracket plottable = new(x1, y1, x2, y2)
            {
                Label = label,
            };

            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bracket to highlight a range between two points in coordinate space with an optional label.
        /// </summary>
        public Bracket AddBracket(Coordinate point1, Coordinate point2, string label = null)
        {
            return AddBracket(point1.X, point1.Y, point2.X, point2.Y, label);
        }

        /// <summary>
        /// Add a Cleveland Dot plot for the given values. Cleveland Dots will be placed at X positions 0, 1, 2, etc.
        /// </summary>
        public ClevelandDotPlot AddClevelandDot(double[] ys1, double[] ys2)
        {
            double[] xs = DataGen.Consecutive(ys1.Length);
            var plottable = new ClevelandDotPlot(xs, ys1, ys2);
            Color color = GetNextColor();
            plottable.Point1Color = color;
            plottable.Point2Color = color;
            plottable.StemColor = color;
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a Cleveland Dot plot for the given values using defined dot positions.
        /// </summary>
        public ClevelandDotPlot AddClevelandDot(double[] ys1, double[] ys2, double[] positions)
        {
            var plottable = new ClevelandDotPlot(positions, ys1, ys2);
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a data logging scatter plot designed for growing collections of X/Y points.
        /// </summary>
        public DataLogger AddDataLogger(Color? color = null, float lineWidth = 1, string label = null)
        {
            DataLogger dl = new(this)
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                Label = label
            };
            Add(dl);
            return dl;
        }

        /// <summary>
        /// Add a data streamer to display a fixed number of evenly-spaced Y values
        /// </summary>
        public DataStreamer AddDataStreamer(double[] values, Color? color = null, float lineWidth = 1, string label = null)
        {
            DataStreamer ds = new(this, values)
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                Label = label
            };
            Add(ds);
            return ds;
        }

        /// <summary>
        /// Add a data streamer to display a fixed number of evenly-spaced Y values
        /// </summary>
        public DataStreamer AddDataStreamer(int length)
        {
            double[] values = new double[length];
            return AddDataStreamer(values);
        }

        /// <summary>
        /// Add a Lollipop plot for the given values. Lollipops will be placed at X positions 0, 1, 2, etc.
        /// </summary>
        public LollipopPlot AddLollipop(double[] values, Color? color = null)
        {
            double[] xs = DataGen.Consecutive(values.Length);
            var plottable = new LollipopPlot(xs, values)
            {
                LollipopColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a lollipop plot for the given values using defined lollipop positions
        /// </summary>
        public LollipopPlot AddLollipop(double[] values, double[] positions, Color? color = null)
        {
            var plottable = new LollipopPlot(positions, values)
            {
                LollipopColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a single bar to the plot.
        /// </summary>
        public BarPlot AddBar(double x, double y, double yError = 0, Color? color = null, double width = 0.8)
        {
            double[] xs = { x };
            double[] ys = { y };
            double[] yErrs = { yError };

            BarPlot bar = new(xs, ys, yErrs, null)
            {
                FillColor = color ?? GetNextColor(),
                BarWidth = width,
            };

            Add(bar);
            return bar;
        }

        /// <summary>
        /// Add a bar plot for the given values. Bars will be placed at X positions 0, 1, 2, etc.
        /// </summary>
        public BarPlot AddBar(double[] values, Color? color = null)
        {
            double[] xs = DataGen.Consecutive(values.Length);
            return AddBar(values, xs, color);
        }

        /// <summary>
        /// Add a bar plot for the given values using defined bar positions
        /// </summary>
        public BarPlot AddBar(double[] values, double[] positions, Color? color = null)
        {
            var plottable = new BarPlot(positions, values, null, null)
            {
                FillColor = color ?? GetNextColor()
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a bar plot (values +/- errors) using defined positions
        /// </summary>
        public BarPlot AddBar(double[] values, double[] errors, double[] positions, Color? color = null)
        {
            var plottable = new BarPlot(positions, values, errors, null)
            {
                FillColor = color ?? GetNextColor(),
                FillColorNegative = color ?? GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a series of bar plots and customize the ticks and legend
        /// </summary>
        public BarPlot[] AddBarGroups(string[] groupLabels, string[] seriesLabels, double[][] ys, double[][] yErr)
        {
            if (groupLabels is null || seriesLabels is null || ys is null)
                throw new ArgumentException("labels and ys cannot be null");

            if (seriesLabels.Length != ys.Length)
                throw new ArgumentException("groupLabels and ys must be the same length");

            foreach (double[] subArray in ys)
                if (subArray.Length != groupLabels.Length)
                    throw new ArgumentException("all arrays inside ys must be the same length as groupLabels");

            double groupWidthFraction = 0.8;
            double barWidthFraction = 0.8;
            double errorCapSize = 0.38;

            int seriesCount = ys.Length;
            double barWidth = groupWidthFraction / seriesCount;
            BarPlot[] bars = new BarPlot[seriesCount];
            bool containsNegativeY = false;
            for (int i = 0; i < seriesCount; i++)
            {
                double[] barYs = ys[i];
                double[] barYerr = yErr?[i];
                double[] barXs = DataGen.Consecutive(barYs.Length);
                containsNegativeY |= barYs.Where(y => y < 0).Any();
                var bar = new BarPlot(barXs, barYs, barYerr, null)
                {
                    Label = seriesLabels[i],
                    BarWidth = barWidth * barWidthFraction,
                    PositionOffset = i * barWidth,
                    ErrorCapSize = errorCapSize,
                    FillColor = GetNextColor()
                };
                bars[i] = bar;
                Add(bar);
            }

            if (containsNegativeY)
                AxisAuto();

            double[] groupPositions = DataGen.Consecutive(groupLabels.Length, offset: (groupWidthFraction - barWidth) / 2);
            XTicks(groupPositions, groupLabels);

            return bars;
        }

        /// <summary>
        /// Create an empty BarSeries, add it to the plot, and return it. Use its Add() method to add bars.
        /// </summary>
        public BarSeries AddBarSeries()
        {
            BarSeries barSeries = new();
            Add(barSeries);
            return barSeries;
        }

        /// <summary>
        /// Create a BarSeries filled with the given bars, add it to the plot, and return it.
        /// </summary>
        public BarSeries AddBarSeries(List<Bar> bars)
        {
            BarSeries barSeries = new(bars);
            Add(barSeries);
            return barSeries;
        }

        /// <summary>
        /// Create a BarSeries filled with the given bars, add it to the plot, and return it.
        /// </summary>
        public BarSeries AddBarSeries(Bar[] bars)
        {
            BarSeries barSeries = new(bars.ToList());
            Add(barSeries);
            return barSeries;
        }

        /// <summary>
        /// Add a binned histogram that displays counts of each cell as a heatmap
        /// </summary>
        public BinnedHistogram AddBinnedHistogram(int columns, int rows, bool addColorbar = true)
        {
            BinnedHistogram hist2d = new(columns, rows);
            Add(hist2d);

            if (addColorbar)
            {
                var cbar = AddColorbar(hist2d.Colormap);
                hist2d.Colorbar = cbar;
            }

            return hist2d;
        }

        /// <summary>
        /// Add an empty bubble plot. Call it's Add() method to add bubbles with custom position and styling.
        /// </summary>
        public BubblePlot AddBubblePlot()
        {
            BubblePlot bubblePlot = new();
            Add(bubblePlot);
            return bubblePlot;
        }

        /// <summary>
        /// Add a bubble plot with multiple bubbles at the given positions all styled the same.
        /// Call the Add() method to add bubbles manually, allowing further customization of size and style.
        /// </summary>
        public BubblePlot AddBubblePlot(double[] xs, double[] ys, double radius = 10, Color? fillColor = null, double edgeWidth = 1, Color? edgeColor = null)
        {
            BubblePlot bubblePlot = new();
            bubblePlot.Add(xs, ys, radius, fillColor ?? GetNextColor(), edgeWidth, edgeColor ?? Color.Black);
            Add(bubblePlot);
            return bubblePlot;
        }

        /// <summary>
        /// Add a circle to the plot
        /// </summary>
        public Ellipse AddCircle(double x, double y, double radius, Color? color = null, float lineWidth = 2, LineStyle lineStyle = LineStyle.Solid)
        {
            return AddEllipse(x, y, radius, radius, color, lineWidth, lineStyle);
        }

        /// <summary>
        /// Add candlesticks to the chart from OHLC (open, high, low, close) data
        /// </summary>
        public FinancePlot AddCandlesticks(IOHLC[] ohlcs)
        {
            FinancePlot plottable = new(ohlcs)
            {
                Candle = true,
                ColorUp = ColorTranslator.FromHtml("#26a69a"),
                ColorDown = ColorTranslator.FromHtml("#ef5350"),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a colorbar to display a colormap beside the data area
        /// </summary>
        /// <param name="colormap">Colormap to display in this colorbar</param>
        /// <param name="space">The size of the right axis will be set to this number of pixels to make room for the colorbar</param>
        /// <param name="rightSide">If false the colorbar will be displayed on the left edge of the plot.</param>
        /// <returns>the colorbar that was just created</returns>
        public Colorbar AddColorbar(Drawing.Colormap colormap = null, int space = 100, bool rightSide = true)
        {
            var cb = new Colorbar(colormap);

            if (rightSide)
            {
                cb.Edge = Renderable.Edge.Right;
                YAxis2.SetSizeLimit(min: space);
            }
            else
            {
                cb.Edge = Renderable.Edge.Left;
                YAxis.SetSizeLimit(min: space);
            }

            Add(cb);
            return cb;
        }

        /// <summary>
        /// Add a colorbar initialized with settings from a heatmap
        /// </summary>
        /// <param name="heatmap">A heatmap-containing plottable to connect with this colorbar</param>
        /// <param name="space">The size of the right axis will be set to this number of pixels to make room for the colorbar</param>
        /// <returns>the colorbar that was just created</returns>
        public Colorbar AddColorbar(IHasColormap heatmap, int space = 100)
        {
            var cb = new Colorbar(heatmap);
            Add(cb);
            YAxis2.SetSizeLimit(min: space);
            return cb;
        }

        /// <summary>
        /// Add a crosshair to the plot
        /// </summary>
        /// <param name="x">position of vertical line (axis units)</param>
        /// <param name="y">position of horizontal line (axis units)</param>
        /// <returns>the crosshair that was just created</returns>
        public Crosshair AddCrosshair(double x, double y)
        {
            Crosshair ch = new() { X = x, Y = y };
            Add(ch);
            return ch;
        }


        /// <summary>
        /// Add an ellipse to the plot
        /// </summary>
        public Ellipse AddEllipse(double x, double y, double xRadius, double yRadius, Color? color = null, float lineWidth = 2, LineStyle lineStyle = LineStyle.Solid)
        {
            Color c = color ?? GetNextColor();
            Ellipse plottable = new(x, y, xRadius, yRadius)
            {
                BorderColor = c,
                BorderLineWidth = lineWidth,
                BorderLineStyle = lineStyle,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values and a baseline.
        /// </summary>
        public Polygon AddFill(double[] xs, double[] ys, double baseline = 0, Color? color = null,
            double lineWidth = 0, Color? lineColor = null)
        {
            var plottable = new Polygon(
                xs: Tools.Pad(xs, cloneEdges: true),
                ys: Tools.Pad(ys, 1, baseline, baseline))
            {
                Fill = true,
                FillColor = color ?? GetNextColor(.5),
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between two Y curves that share the same X positions.
        /// </summary>
        public Polygon AddFill(double[] xs, double[] ys1, double[] ys2, Color? color = null,
            double lineWidth = 0, Color? lineColor = null)
        {
            double[] polyXs = new double[] { xs[0] }.Concat(xs.Concat(xs.Reverse())).ToArray();
            double[] polyYs = new double[] { ys1[0] }.Concat(ys1.Concat(ys2.Reverse())).ToArray();

            var plottable = new Polygon(polyXs, polyYs)
            {
                Fill = true,
                FillColor = color ?? GetNextColor(.5),
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };

            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values of two curves.
        /// </summary>
        public Polygon AddFill(double[] xs1, double[] ys1, double[] xs2, double[] ys2, Color? color = null,
            double lineWidth = 0, Color? lineColor = null)
        {
            // combine xs and ys to make one big curve
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

            var plottable = new Polygon(bothX, bothY)
            {
                Fill = true,
                FillColor = color ?? GetNextColor(.5),
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area above and below a Y curve
        /// </summary>
        public Polygon AddFillError(double[] xs, double[] ys, double[] yError, Color? color = null,
            double lineWidth = 0, Color? lineColor = null)
        {
            double[] polyXs = xs.Concat(xs.Reverse()).ToArray();

            double[] ysAbove = Enumerable.Range(0, ys.Length).Select(i => ys[i] + yError[i]).ToArray();
            double[] ysBelow = Enumerable.Range(0, ys.Length).Select(i => ys[i] - yError[i]).ToArray();

            double[] polyYs = ysBelow.Concat(ysAbove.Reverse()).ToArray();

            var plottable = new Polygon(polyXs, polyYs)
            {
                Fill = true,
                FillColor = color ?? GetNextColor(.5),
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };

            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Create a polygon to fill the area between Y values and a baseline
        /// that uses two different colors for area above and area below the baseline.
        /// </summary>
        public (Polygon polyAbove, Polygon polyBelow) AddFillAboveAndBelow(double[] xs, double[] ys, double baseline = 0, Color? colorAbove = null, Color? colorBelow = null,
            double lineWidth = 0, Color? lineColor = null)
        {
            var (xs2, ysAbove, ysBelow) = Drawing.Tools.PolyAboveAndBelow(xs, ys, baseline);

            var polyAbove = new Polygon(xs2, ysAbove)
            {
                FillColor = colorAbove ?? Color.Green,
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };
            var polyBelow = new Polygon(xs2, ysBelow)
            {
                FillColor = colorBelow ?? Color.Red,
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black
            };
            Add(polyAbove);
            Add(polyBelow);

            return (polyAbove, polyBelow);
        }

        /// <summary>
        /// Add a line plot that uses a function (rather than X/Y points) to place the curve
        /// </summary>
        public FunctionPlot AddFunction(Func<double, double?> function, Color? color = null, double lineWidth = 1, LineStyle lineStyle = LineStyle.Solid)
        {
            Color color2 = color ?? settings.GetNextColor();
            FunctionPlot plottable = new(function)
            {
                Color = color2,
                LineWidth = lineWidth,
                LineStyle = lineStyle,
                FillColor = Color.FromArgb(50, color2),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a heatmap to the plot automatically-sized so each cell is 1x1.
        /// </summary>
        /// <param name="intensities">2D array of intensities. 
        /// WARNING: Rendering artifacts may appear for arrays larger than Bitmap can support (~10M total values).</param>
        /// <param name="colormap"></param>
        /// <param name="lockScales">If true, <see cref="AxisScaleLock"/> will be called to ensure heatmap cells will be square.</param>
        /// <returns>
        /// Returns the heatmap that was added to the plot.
        /// Act on its public fields and methods to customize it or update its data.
        /// </returns>
        public Heatmap AddHeatmap(double?[,] intensities, Drawing.Colormap colormap = null, bool? lockScales = true)
        {
            var plottable = new Heatmap();
            plottable.Update(intensities, colormap);
            Add(plottable);

            if (lockScales.HasValue && lockScales.Value == true)
                AxisScaleLock(true);

            if (lockScales is null && plottable.IsDefaultSizeAndLocation)
                AxisScaleLock(true);

            return plottable;
        }

        /// <summary>
        /// Add a heatmap to the plot automatically-sized so each cell is 1x1.
        /// </summary>
        /// <param name="intensities">2D array of intensities. 
        /// WARNING: Rendering artifacts may appear for arrays larger than Bitmap can support (~10M total values).</param>
        /// <param name="colormap"></param>
        /// <param name="lockScales">If true, <see cref="AxisScaleLock"/> will be called to ensure heatmap cells will be square.</param>
        /// <returns>
        /// Returns the heatmap that was added to the plot.
        /// Act on its public fields and methods to customize it or update its data.
        /// </returns>
        public Heatmap AddHeatmap(double[,] intensities, Drawing.Colormap colormap = null, bool? lockScales = null)
        {
            var plottable = new Heatmap();
            plottable.Update(intensities, colormap);
            Add(plottable);

            if (lockScales.HasValue && lockScales.Value == true)
                AxisScaleLock(true);

            if (lockScales is null && plottable.IsDefaultSizeAndLocation)
                AxisScaleLock(true);

            return plottable;
        }

        /// <summary>
        /// Add a single-color heatmap where opacity is defined by a 2D array.
        /// </summary>
        /// <param name="color">Single color used for all cells</param>
        /// <param name="opacity">Opacities (ranging 0-1) for all cells</param>
        /// <param name="lockScales">If true, <see cref="AxisScaleLock"/> will be called to ensure heatmap cells will be square</param>
        /// <returns></returns>
        public Heatmap AddHeatmap(Color color, double?[,] opacity, bool? lockScales = true)
        {
            var plottable = new Heatmap();
            plottable.Update(color, opacity);
            Add(plottable);

            if (lockScales.HasValue && lockScales.Value == true)
                AxisScaleLock(true);

            if (lockScales is null && plottable.IsDefaultSizeAndLocation)
                AxisScaleLock(true);

            return plottable;
        }

        /// <summary>
        /// Add a single-color heatmap where opacity is defined by a 2D array.
        /// </summary>
        /// <param name="color">Single color used for all cells</param>
        /// <param name="opacity">Opacities (ranging 0-1) for all cells</param>
        /// <param name="lockScales">If true, <see cref="AxisScaleLock"/> will be called to ensure heatmap cells will be square</param>
        /// <returns></returns>
        public Heatmap AddHeatmap(Color color, double[,] opacity, bool? lockScales = true)
        {
            var plottable = new Heatmap();
            plottable.Update(color, opacity);
            Add(plottable);

            if (lockScales.HasValue && lockScales.Value == true)
                AxisScaleLock(true);

            if (lockScales is null && plottable.IsDefaultSizeAndLocation)
                AxisScaleLock(true);

            return plottable;
        }

        /// <summary>
        /// Add heatmap to the plot stretched to fit the given dimensions.
        /// Unlike the regular heatmap which gives each cell a size of 1x1 and starts at the axis origin, 
        /// this heatmap stretches the array so that it covers the defined X and Y spans.
        /// </summary>
        /// <param name="intensities">2D array of intensities. 
        /// WARNING: Rendering artifacts may appear for arrays larger than Bitmap can support (~10M total values).</param>
        /// <param name="xMin">position of the left edge of the far left column</param>
        /// <param name="xMax">position of the left edge of the far right column</param>
        /// <param name="yMin">position of the upper edge of the bottom row</param>
        /// <param name="yMax">position of the upper edge of the top row</param>
        /// <param name="colormap"></param>
        /// <returns>
        /// Returns the heatmap that was added to the plot.
        /// Act on its public fields and methods to customize it or update its data.
        /// </returns>
        [Obsolete("This plot type has been deprecated. (min/max functionality now exists in Heatmap)")]
        public CoordinatedHeatmap AddHeatmapCoordinated(double?[,] intensities, double? xMin = null, double? xMax = null, double? yMin = null, double? yMax = null, Drawing.Colormap colormap = null)
        {
            var plottable = new CoordinatedHeatmap();

            // Solve all possible null combinations, if the boundaries are only partially provided use Step = 1;
            if (xMin == null && xMax == null)
            {
                plottable.XMin = 0;
                plottable.XMax = 0 + intensities.GetLength(0);
            }
            else if (xMin == null)
            {
                plottable.XMax = xMax.Value;
                plottable.XMin = xMax.Value - intensities.GetLength(0);
            }
            else if (xMax == null)
            {
                plottable.XMin = xMin.Value;
                plottable.XMax = xMin.Value + intensities.GetLength(0);
            }
            else
            {
                plottable.XMin = xMin.Value;
                plottable.XMax = xMax.Value;
            }

            if (yMin == null && yMax == null)
            {
                plottable.YMin = 0;
                plottable.YMax = 0 + intensities.GetLength(1);
            }
            else if (yMin == null)
            {
                plottable.YMax = yMax.Value;
                plottable.YMin = yMax.Value - intensities.GetLength(1);
            }
            else if (yMax == null)
            {
                plottable.YMin = yMin.Value;
                plottable.YMax = yMin.Value + intensities.GetLength(1);
            }
            else
            {
                plottable.YMin = yMin.Value;
                plottable.YMax = yMax.Value;
            }

            plottable.Update(intensities, colormap);
            Add(plottable);

            return plottable;
        }

        /// <summary>
        /// Add heatmap to the plot stretched to fit the given dimensions.
        /// Unlike the regular heatmap which gives each cell a size of 1x1 and starts at the axis origin, 
        /// this heatmap stretches the array so that it covers the defined X and Y spans.
        /// </summary>
        /// <param name="intensities">2D array of intensities. 
        /// WARNING: Rendering artifacts may appear for arrays larger than Bitmap can support (~10M total values).</param>
        /// <param name="xMin">position of the left edge of the far left column</param>
        /// <param name="xMax">position of the left edge of the far right column</param>
        /// <param name="yMin">position of the upper edge of the bottom row</param>
        /// <param name="yMax">position of the upper edge of the top row</param>
        /// <param name="colormap"></param>
        /// <returns>
        /// Returns the heatmap that was added to the plot.
        /// Act on its public fields and methods to customize it or update its data.
        /// </returns>
        [Obsolete("This plot type has been deprecated. Use a regular heatmap and modify its Offset and CellSize fields.")]
        public CoordinatedHeatmap AddHeatmapCoordinated(double[,] intensities, double? xMin = null, double? xMax = null, double? yMin = null, double? yMax = null, Drawing.Colormap colormap = null)
        {
            var plottable = new CoordinatedHeatmap();

            // Solve all possible null combinations, if the boundaries are only partially provided use Step = 1;
            if (xMin == null && xMax == null)
            {
                plottable.XMin = 0;
                plottable.XMax = 0 + intensities.GetLength(0);
            }
            else if (xMin == null)
            {
                plottable.XMax = xMax.Value;
                plottable.XMin = xMax.Value - intensities.GetLength(0);
            }
            else if (xMax == null)
            {
                plottable.XMin = xMin.Value;
                plottable.XMax = xMin.Value + intensities.GetLength(0);
            }
            else
            {
                plottable.XMin = xMin.Value;
                plottable.XMax = xMax.Value;
            }

            if (yMin == null && yMax == null)
            {
                plottable.YMin = 0;
                plottable.YMax = 0 + intensities.GetLength(1);
            }
            else if (yMin == null)
            {
                plottable.YMax = yMax.Value;
                plottable.YMin = yMax.Value - intensities.GetLength(1);
            }
            else if (yMax == null)
            {
                plottable.YMin = yMin.Value;
                plottable.YMax = yMin.Value + intensities.GetLength(1);
            }
            else
            {
                plottable.YMin = yMin.Value;
                plottable.YMax = yMax.Value;
            }

            plottable.Update(intensities, colormap);
            Add(plottable);

            return plottable;
        }

        /// <summary>
        /// Add a horizontal axis line at a specific Y position
        /// </summary>
        public HLine AddHorizontalLine(double y, Color? color = null, float width = 1, LineStyle style = LineStyle.Solid, string label = null)
        {
            HLine plottable = new HLine()
            {
                Y = y,
                Color = color ?? settings.GetNextColor(),
                LineWidth = width,
                LineStyle = style,
                Label = label,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a horizontal span (shades the region between two X positions)
        /// </summary>
        public HSpan AddHorizontalSpan(double xMin, double xMax, Color? color = null, string label = null)
        {
            var plottable = new HSpan()
            {
                X1 = xMin,
                X2 = xMax,
                Color = color ?? GetNextColor(.5),
                Label = label,
            };
            Add(plottable);
            return plottable;
        }
        /// <summary>
        /// Display an image at a specific coordinate.
        /// The <paramref name="anchor"/> defines which part of the image is placed at that coordinate.
        /// By default the image is shown at its original size (in pixel units), but this can be modified with <paramref name="scale"/>.
        /// </summary>
        /// <param name="bitmap">Image to display</param>
        /// <param name="x">horizontal position of the image anchor (axis units)</param>
        /// <param name="y">vertical position of the image anchor (axis units)</param>
        /// <param name="rotation">rotation in degrees</param>
        /// <param name="scale">scale (1.0 = original scale, 2.0 = double size)</param>
        /// <param name="anchor">definces which part of the image is placed at the given X and Y coordinates</param>
        /// <returns></returns>
        public Plottable.Image AddImage(Bitmap bitmap, double x, double y, double rotation = 0, double scale = 1, Alignment anchor = Alignment.UpperLeft)
        {
            Plottable.Image plottable = new()
            {
                Bitmap = bitmap,
                X = x,
                Y = y,
                Rotation = rotation,
                Scale = scale,
                Alignment = anchor,
            };

            settings.Plottables.Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a line (a scatter plot with two points) to the plot
        /// </summary>
        public ScatterPlot AddLine(double x1, double y1, double x2, double y2, Color? color = null, float lineWidth = 1)
        {
            return AddScatter(new double[] { x1, x2 }, new double[] { y1, y2 }, color, lineWidth, 0);
        }

        /// <summary>
        /// Add a line (a scatter plot with two points) to the plot
        /// </summary>
        public ScatterPlot AddLine(double slope, double offset, (double x1, double x2) xLimits, Color? color = null, float lineWidth = 1)
        {
            double y1 = xLimits.x1 * slope + offset;
            double y2 = xLimits.x2 * slope + offset;
            return AddScatter(new double[] { xLimits.x1, xLimits.x2 }, new double[] { y1, y2 }, color, lineWidth, 0);
        }

        /// <summary>
        /// Add a marker at a specific X/Y position.
        /// </summary>
        public MarkerPlot AddMarker(double x, double y, MarkerShape shape = MarkerShape.filledCircle, double size = 10, Color? color = null, string label = null)
        {
            var plottable = new MarkerPlot()
            {
                X = x,
                Y = y,
                MarkerShape = shape,
                MarkerSize = (float)size,
                Color = color ?? GetNextColor(),
                Label = label,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a draggable marker at a specific X/Y position.
        /// </summary>
        public DraggableMarkerPlot AddMarkerDraggable(double x, double y, MarkerShape shape = MarkerShape.filledCircle, double size = 10, Color? color = null, string label = null)
        {
            var plottable = new DraggableMarkerPlot()
            {
                X = x,
                Y = y,
                MarkerShape = shape,
                MarkerSize = (float)size,
                Color = color ?? GetNextColor(),
                Label = label,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add OHLC (open, high, low, close) data to the plot
        /// </summary>
        public FinancePlot AddOHLCs(IOHLC[] ohlcs)
        {
            FinancePlot plottable = new(ohlcs)
            {
                Candle = false,
                ColorUp = ColorTranslator.FromHtml("#26a69a"),
                ColorDown = ColorTranslator.FromHtml("#ef5350"),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a pie chart to the plot
        /// </summary>
        public PiePlot AddPie(double[] values, bool hideGridAndFrame = true)
        {
            Color[] colors = Enumerable.Range(0, values.Length)
                                       .Select(i => settings.PlottablePalette.GetColor(i))
                                       .ToArray();

            PiePlot pie = new PiePlot(values, null, colors);
            Add(pie);

            if (hideGridAndFrame)
            {
                Grid(false);
                Frameless();
            }

            return pie;
        }

        /// <summary>
        /// Add a point (a scatter plot with a single marker)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color">color of the marker</param>
        /// <param name="size">size of the marker</param>
        /// <param name="shape">maker shape</param>
        /// <param name="label">text to appear in the legend</param>
        /// <returns>
        /// The scatter plot that was created and added to the plot. 
        /// Interact with its public fields and methods to customize style and update data.
        /// </returns>
        public MarkerPlot AddPoint(double x, double y, Color? color = null, float size = 5, MarkerShape shape = MarkerShape.filledCircle, string label = null)
        {
            var plottable = new MarkerPlot()
            {
                X = x,
                Y = y,
                MarkerShape = shape,
                MarkerSize = size,
                Color = color ?? GetNextColor(),
                Label = label,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a polygon to the plot
        /// </summary>
        public Polygon AddPolygon(double[] xs, double[] ys, Color? fillColor = null, double lineWidth = 0, Color? lineColor = null)
        {
            var plottable = new Polygon(xs, ys)
            {
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black,
                FillColor = fillColor ?? settings.GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add many polygons using an optimized rendering method
        /// </summary>
        public Polygons AddPolygons(List<List<(double x, double y)>> polys, Color? fillColor = null, double lineWidth = 0, Color? lineColor = null)
        {
            var plottable = new Polygons(polys)
            {
                LineWidth = lineWidth,
                LineColor = lineColor ?? Color.Black,
                FillColor = fillColor ?? settings.GetNextColor(),
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a population to the plot
        /// </summary>
        public PopulationPlot AddPopulation(Population population, string label = null)
        {
            var plottable = new PopulationPlot(population, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add multiple populations to the plot as a single series
        /// </summary>
        public PopulationPlot AddPopulations(Population[] populations, string label = null)
        {
            var plottable = new PopulationPlot(populations, label, settings.GetNextColor());
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add multiple populations to the plot as a single series
        /// </summary>
        public PopulationPlot AddPopulations(PopulationMultiSeries multiSeries)
        {
            for (int i = 0; i < multiSeries.multiSeries.Length; i++)
                multiSeries.multiSeries[i].color = settings.PlottablePalette.GetColor(i);

            var plottable = new PopulationPlot(multiSeries);
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a radar plot (a two-dimensional chart of three or more quantitative variables represented on axes starting from the same point)
        /// </summary>
        /// <param name="values">2D array containing categories (columns) and groups (rows)</param>
        /// <param name="independentAxes">if true, axis (category) values are scaled independently</param>
        /// <param name="maxValues">if provided, each category (column) is normalized to these values</param>
        /// <param name="disableFrameAndGrid">also make the plot frameless and disable its grid</param>
        /// <param name="alphafill">if provided, this value overrides the intermediate transparency (alpha) applied by default to the area fill color</param>
        /// <returns>the radar plot that was just created and added to the plot</returns>
        public RadarPlot AddRadar(double[,] values, bool independentAxes = false, double[] maxValues = null, bool disableFrameAndGrid = true, int alphafill = 50)
        {

            Color[] colors = Enumerable.Range(0, values.Length)
                                       .Select(i => settings.PlottablePalette.GetColor(i))
                                       .ToArray();

            Color[] fills = colors.Select(x => Color.FromArgb(alphafill, x)).ToArray();

            RadarPlot plottable = new(values, colors, fills, independentAxes, maxValues);
            Add(plottable);

            if (disableFrameAndGrid)
            {
                Frameless();
                Grid(enable: false);
            }

            return plottable;
        }

        /// <summary>
        /// Add a radial gauge plot (a chart where data is represented by concentric circular gauges)
        /// </summary>
        /// <param name="values">Array of gauge values</param>
        /// <param name="disableFrameAndGrid">Also make the plot frameless and disable its grid</param>
        /// <returns>The radial gaugle plot that was just created and added to the plot</returns>
        public ScottPlot.Plottable.RadialGaugePlot AddRadialGauge(double[] values, bool disableFrameAndGrid = true)
        {
            Color[] colors = Enumerable.Range(0, values.Length).Select(x => Palette.GetColor(x)).ToArray();
            ScottPlot.Plottable.RadialGaugePlot plottable = new(values, colors);
            Add(plottable);

            if (disableFrameAndGrid)
            {
                Frameless();
                Grid(enable: false);
            }

            return plottable;
        }

        public RectanglePlot AddRectangle(double xMin, double xMax, double yMin, double yMax)
        {
            CoordinateRect rect = new(xMin, xMax, yMin, yMax);
            RectanglePlot rp = new(rect);
            Add(rp);
            return rp;
        }

        /// <summary>
        /// A Pie chart where the angle of slices is constant but the radii are not.
        /// </summary>
        /// <param name="values">The data to plot</param>
        /// <param name="hideGridAndFrame">Whether to make the plot frameless and disable the grid</param>
        public CoxcombPlot AddCoxcomb(double[] values, bool hideGridAndFrame = true)
        {
            Color[] colors = Enumerable.Range(0, values.Length)
                           .Select(i => settings.PlottablePalette.GetColor(i))
                           .ToArray();

            CoxcombPlot plottable = new(values, colors);
            Add(plottable);

            if (hideGridAndFrame)
            {
                Grid(false);
                Frameless();
            }

            return plottable;

        }

        /// <summary>
        /// Add error bars to the plot with custom dimensions in all 4 directions.
        /// </summary>
        /// <param name="xs">Horizontal center of the errorbar</param>
        /// <param name="ys">Vertical center of each errorbar</param>
        /// <param name="xErrorsPositive">Magnitude of positive vertical error</param>
        /// <param name="xErrorsNegative">Magnitude of positive horizontal error</param>
        /// <param name="yErrorsPositive">Magnitude of negative vertical error</param>
        /// <param name="yErrorsNegative">Magnitude of negative horizontal error</param>
        /// <param name="color">Color (null for next color in palette)</param>
        /// <param name="markerSize">Size (in pixels) to draw a marker at the center of each errorbar</param>
        public ErrorBar AddErrorBars(double[] xs, double[] ys, double[] xErrorsPositive, double[] xErrorsNegative, double[] yErrorsPositive, double[] yErrorsNegative, Color? color = null, float markerSize = 0)
        {
            ErrorBar errorBar = new(xs, ys, xErrorsPositive, xErrorsNegative, yErrorsPositive, yErrorsNegative)
            {
                Color = color ?? GetNextColor(),
                MarkerSize = markerSize,
            };
            Add(errorBar);

            return errorBar;
        }

        /// <summary>
        /// Add error bars to the plot which have symmetrical positive/negative errors
        /// </summary>
        /// <param name="xs">Horizontal center of the errorbar</param>
        /// <param name="ys">Vertical center of each errorbar</param>
        /// <param name="xErrors">Magnitude of vertical error</param>
        /// <param name="yErrors">Magnitude of horizontal error</param>
        /// <param name="color">Color (null for next color in palette)</param>
        /// <param name="markerSize">Size (in pixels) to draw a marker at the center of each errorbar</param>
        public ErrorBar AddErrorBars(double[] xs, double[] ys, double[] xErrors, double[] yErrors, Color? color = null, float markerSize = 0) =>
            AddErrorBars(xs, ys, xErrors, xErrors, yErrors, yErrors, color, markerSize);

        /// <summary>
        /// Add an L-shaped scalebar to the corner of the plot
        /// </summary>
        public ScaleBar AddScaleBar(double width, double height, string xLabel = null, string yLabel = null)
        {
            var scalebar = new ScaleBar()
            {
                Width = width,
                Height = height,
                HorizontalLabel = xLabel,
                VerticalLabel = yLabel,
            };
            Add(scalebar);
            return scalebar;
        }

        /// <summary>
        /// Add a scatter plot from X/Y pairs. 
        /// Lines and markers are shown by default.
        /// Scatter plots are slower than Signal plots.
        /// </summary>
        public ScatterPlot AddScatter(
            double[] xs,
            double[] ys,
            Color? color = null,
            float lineWidth = 1,
            float markerSize = 5,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid,
            string label = null)
        {
            var plottable = new ScatterPlot(xs, ys, null, null)
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = markerSize,
                Label = label,
                MarkerShape = markerShape,
                LineStyle = lineStyle
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a scatter plot from X/Y pairs connected by lines (no markers).
        /// Scatter plots are slower than Signal plots.
        /// </summary>
        public ScatterPlot AddScatterLines(
            double[] xs,
            double[] ys,
            Color? color = null,
            float lineWidth = 1,
            LineStyle lineStyle = LineStyle.Solid,
            string label = null)
        {
            var plottable = new ScatterPlot(xs, ys, null, null)
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = 0,
                Label = label,
                LineStyle = lineStyle
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a scatter plot from X/Y pairs using markers at points (no lines).
        /// Scatter plots are slower than Signal plots.
        /// </summary>
        public ScatterPlot AddScatterPoints(
            double[] xs,
            double[] ys,
            Color? color = null,
            float markerSize = 5,
            MarkerShape markerShape = MarkerShape.filledCircle,
            string label = null)
        {
            var plottable = new ScatterPlot(xs, ys, null, null)
            {
                Color = color ?? GetNextColor(),
                LineWidth = 0,
                MarkerSize = markerSize,
                Label = label,
                MarkerShape = markerShape
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a step plot is a type of line plot where points are connected with right angles instead of straight lines.
        /// </summary>
        public ScatterPlot AddScatterStep(
            double[] xs,
            double[] ys,
            Color? color = null,
            float lineWidth = 1,
            string label = null)
        {
            var plottable = new ScatterPlot(xs, ys, null, null)
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                Label = label,
                MarkerSize = 0,
                StepDisplay = true
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Scatter plot with Add() and Clear() methods for updating data
        /// </summary>
        public ScatterPlotList<double> AddScatterList(
            Color? color = null,
            float lineWidth = 1,
            float markerSize = 5,
            string label = null,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid)
        {
            var spl = new ScatterPlotList<double>()
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = markerSize,
                Label = label,
                MarkerShape = markerShape,
                LineStyle = lineStyle
            };

            Add(spl);
            return spl;
        }

        /// <summary>
        /// Generic ScatterPlotList using generic types (as long as they can be converted to double)
        /// </summary>
        public ScatterPlotList<T> AddScatterList<T>(
            Color? color = null,
            float lineWidth = 1,
            float markerSize = 5,
            string label = null,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid)
        {
            var spl = new ScatterPlotList<T>()
            {
                Color = color ?? GetNextColor(),
                LineWidth = lineWidth,
                MarkerSize = markerSize,
                Label = label,
                MarkerShape = markerShape,
                LineStyle = lineStyle
            };

            Add(spl);
            return spl;
        }

        /// <summary>
        /// Signal plots have evenly-spaced X points and render very fast.
        /// </summary>
        public SignalPlot AddSignal(double[] ys, double sampleRate = 1, Color? color = null, string label = null)
        {
            var signal = new SignalPlot()
            {
                Ys = ys,
                SampleRate = sampleRate,
                Color = color ?? settings.GetNextColor(),
                Label = label,

                // TODO: FIX THIS!!!
                MinRenderIndex = 0,
                MaxRenderIndex = Math.Max(0, ys.Length - 1),
            };
            Add(signal);
            return signal;
        }

        /// <summary>
        /// Signal plots have evenly-spaced X points and render very fast.
        /// </summary>
        public SignalPlotGeneric<T> AddSignal<T>(T[] ys, double sampleRate = 1, Color? color = null, string label = null) where T : struct, IComparable
        {
            var signal = new SignalPlotGeneric<T>()
            {
                Ys = ys,
                SampleRate = sampleRate,
                Color = color ?? settings.GetNextColor(),
                Label = label,

                MinRenderIndex = 0,
                MaxRenderIndex = ys.Length - 1,
            };
            Add(signal);
            return signal;
        }


        /// <summary>
        /// SignalConts plots have evenly-spaced X points and render faster than Signal plots
        /// but data in source arrays cannot be changed after it is loaded.
        /// Methods can be used to update all or portions of the data.
        /// </summary>
        public SignalPlotConst<T> AddSignalConst<T>(T[] ys, double sampleRate = 1, Color? color = null, string label = null) where T : struct, IComparable
        {
            SignalPlotConst<T> plottable = new SignalPlotConst<T>()
            {
                Ys = ys,
                SampleRate = sampleRate,
                Color = color ?? settings.GetNextColor(),
                Label = label,
                MinRenderIndex = 0,
                MaxRenderIndex = ys.Length - 1,
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Speed-optimized plot for Ys with unevenly-spaced ascending Xs
        /// </summary>
        public SignalPlotXY AddSignalXY(double[] xs, double[] ys, Color? color = null, string label = null)
        {
            SignalPlotXY plottable = new SignalPlotXY()
            {
                Xs = xs,
                Ys = ys,
                Color = color ?? settings.GetNextColor(),
                Label = label,
                MinRenderIndex = 0,
                MaxRenderIndex = ys.Length - 1,
            };
            Add(plottable);
            return plottable;
        }


        /// <summary>
        /// Speed-optimized plot for Ys with unevenly-spaced ascending Xs.
        /// Faster than SignalXY but values cannot be modified after loading.
        /// </summary>
        public SignalPlotXYConst<TX, TY> AddSignalXYConst<TX, TY>(TX[] xs, TY[] ys, Color? color = null, string label = null)
            where TX : struct, IComparable where TY : struct, IComparable
        {
            SignalPlotXYConst<TX, TY> signal = new SignalPlotXYConst<TX, TY>()
            {
                Xs = xs,
                Ys = ys,
                Color = color ?? settings.GetNextColor(),
                Label = label,
                MinRenderIndex = 0,
                MaxRenderIndex = ys.Length - 1,
            };
            Add(signal);
            return signal;
        }

        /// <summary>
        /// Display text at specific X/Y coordinates
        /// </summary>
        public Text AddText(string label, double x, double y, float size = 12, Color? color = null) =>
            AddText(label, x, y, new Drawing.Font() { Size = size, Color = color ?? GetNextColor() });

        /// <summary>
        /// Display text at specific X/Y coordinates
        /// </summary>
        public Text AddText(string label, double x, double y, Drawing.Font font)
        {
            var plottable = new Text()
            {
                Label = label,
                X = x,
                Y = y,
                Font = font
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Display a text bubble that points to an X/Y location on the plot
        /// </summary>
        public Tooltip AddTooltip(string label, double x, double y)
        {
            var plottable = new Tooltip() { Label = label, X = x, Y = y };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a 2D vector field to the plot
        /// </summary>
        public VectorField AddVectorField(
            Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Drawing.Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            // TODO: refactor constructor to eliminate styling arguments
            var vectorField = new VectorField(vectors, xs, ys,
                colormap, scaleFactor, color ?? settings.GetNextColor())
            { Label = label };

            Add(vectorField);
            return vectorField;
        }

        /// <summary>
        /// Add a 2D vector field to the plot
        /// </summary>
        public VectorFieldList AddVectorFieldList()
        {
            var vectorFieldList = new VectorFieldList();
            Add(vectorFieldList);
            return vectorFieldList;
        }

        /// <summary>
        /// Add a vertical axis line at a specific X position
        /// </summary>
        public VLine AddVerticalLine(double x, Color? color = null, float width = 1, LineStyle style = LineStyle.Solid, string label = null)
        {
            VLine plottable = new VLine()
            {
                X = x,
                Color = color ?? settings.GetNextColor(),
                LineWidth = width,
                LineStyle = style,
                Label = label
            };
            Add(plottable);
            return plottable;
        }

        /// <summary>
        /// Add a horizontal span (shades the region between two X positions)
        /// </summary>
        public VSpan AddVerticalSpan(double yMin, double yMax, Color? color = null, string label = null)
        {
            var plottable = new VSpan()
            {
                Y1 = yMin,
                Y2 = yMax,
                Color = color ?? GetNextColor(.5),
                Label = label,
            };
            Add(plottable);
            return plottable;
        }
    }
}
