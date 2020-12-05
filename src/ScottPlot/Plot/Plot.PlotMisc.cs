/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using ScottPlot.Plottable;
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
        public VectorField PlotVectorField(
            Statistics.Vector2[,] vectors,
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
            { label = label };

            Add(vectorField);
            return vectorField;
        }

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
                categoryNames = categoryNames,
                groupNames = groupNames,
                webColor = webColor ?? Color.Gray
            };
            Add(plottable);

            return plottable;
        }

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
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                lineStyle = lineStyle,
                label = label
            };

            Add(functionPlot);
            return functionPlot;
        }

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
            colors = colors ?? Enumerable.Range(0, values.Length).Select(i => settings.PlottablePalette.GetColor(i)).ToArray();

            PiePlot pie = new PiePlot(values, sliceLabels, colors)
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
