/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {

        public PlottableVectorField PlotVectorField(
            Statistics.Vector2[,] vectors,
            double[] xs,
            double[] ys,
            string label = null,
            Color? color = null,
            Drawing.Colormap colormap = null,
            double scaleFactor = 1
            )
        {
            var vectorField = new PlottableVectorField(vectors, xs, ys,
                colormap, scaleFactor, color ?? settings.GetNextColor())
            { label = label };

            Add(vectorField);
            return vectorField;
        }

        public PlottableScatter PlotArrow(
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

        public PlottableRadar PlotRadar(
            double[,] values,
            string[] categoryNames = null,
            string[] groupNames = null,
            Color[] fillColors = null,
            double fillAlpha = .4,
            Color? webColor = null
            )
        {
            Color[] colors = fillColors ?? Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();
            Color[] colorsAlpha = colors.Select(x => Color.FromArgb((byte)(255 * fillAlpha), x)).ToArray();

            var plottable = new PlottableRadar(values, colors, fillColors ?? colorsAlpha)
            {
                categoryNames = categoryNames,
                groupNames = groupNames,
                webColor = webColor ?? Color.Gray
            };
            Add(plottable);
            MatchAxis(this);

            return plottable;
        }

        public PlottableFunction PlotFunction(
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

            PlottableFunction functionPlot = new PlottableFunction(function)
            {
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                lineStyle = lineStyle,
                label = label
            };

            Add(functionPlot);
            return functionPlot;
        }

        public PlottableScaleBar PlotScaleBar(
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
            var scalebar = new PlottableScaleBar()
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

        public PlottablePie PlotPie(
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
            colors = colors ?? Enumerable.Range(0, values.Length).Select(i => settings.colorset.GetColor(i)).ToArray();

            PlottablePie pie = new PlottablePie(values, sliceLabels, colors)
            {
                explodedChart = explodedChart,
                showValues = showValues,
                showPercentages = showPercentages,
                showLabels = showLabels,
                label = label
            };

            Add(pie);
            return pie;
        }

    }
}
