/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {

        public PlottableScatter PlotPoint(
            double x,
            double y,
            Color? color = null,
            double markerSize = 5,
            string label = null,
            double? errorX = null,
            double? errorY = null,
            double errorLineWidth = 1,
            double errorCapSize = 3,
            MarkerShape markerShape = MarkerShape.filledCircle,
            LineStyle lineStyle = LineStyle.Solid
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            var scatterPlot = new PlottableScatter(
                    xs: new double[] { x },
                    ys: new double[] { y },
                    errorX: (errorX is null) ? null : new double[] { (double)errorX },
                    errorY: (errorY is null) ? null : new double[] { (double)errorY }
                )
            {
                color = (Color)color,
                lineWidth = 0,
                markerSize = (float)markerSize,
                label = label,
                errorLineWidth = (float)errorLineWidth,
                errorCapSize = (float)errorCapSize,
                stepDisplay = false,
                markerShape = markerShape,
                lineStyle = lineStyle
            };

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableScatter PlotScatter(
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
            var scatterPlot = new PlottableScatter(xs, ys, errorX, errorY)
            {
                color = color ?? settings.GetNextColor(),
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                errorLineWidth = (float)errorLineWidth,
                errorCapSize = (float)errorCapSize,
                stepDisplay = false,
                markerShape = markerShape,
                lineStyle = lineStyle
            };

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableScatterHighlight PlotScatterHighlight(
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

            var scatterPlot = new PlottableScatterHighlight(xs, ys, errorX, errorY)
            {
                color = (Color)color,
                lineWidth = lineWidth,
                markerSize = (float)markerSize,
                label = label,
                errorLineWidth = (float)errorLineWidth,
                errorCapSize = (float)errorCapSize,
                stepDisplay = false,
                markerShape = markerShape,
                lineStyle = lineStyle,
                highlightedShape = highlightedShape,
                highlightedColor = highlightedColor.Value,
                highlightedMarkerSize = (float)highlightedMarkerSize.Value
            };

            Add(scatterPlot);
            return scatterPlot;
        }

        public PlottableErrorBars PlotErrorBars(
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
            )
        {
            var errorBars = new PlottableErrorBars(xs, ys, xPositiveError, xNegativeError, yPositiveError, yNegativeError)
            {
                Color = color ?? settings.GetNextColor(),
                LineWidth = (float)lineWidth,
                CapSize = (float)capWidth,
                label = label
            };

            Add(errorBars);
            return errorBars;
        }

        public PlottableScatter PlotLine(
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

        public PlottableScatter PlotLine(
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

        public PlottableScatter PlotStep(
            double[] xs,
            double[] ys,
            Color? color = null,
            double lineWidth = 1,
            string label = null
            )
        {
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter stepPlot = new PlottableScatter(xs, ys)
            {
                color = (Color)color,
                lineWidth = lineWidth,
                markerSize = 0,
                label = label,
                errorX = null,
                errorY = null,
                errorLineWidth = 0,
                errorCapSize = 0,
                stepDisplay = true,
                markerShape = MarkerShape.none,
                lineStyle = LineStyle.Solid
            };

            Add(stepPlot);
            return stepPlot;
        }
    }
}
