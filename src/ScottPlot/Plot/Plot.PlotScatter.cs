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

            double[] errorXarray = (errorX != null) ? new double[] { (double)errorX } : null;
            double[] errorYarray = (errorY != null) ? new double[] { (double)errorY } : null;

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: new double[] { x },
                ys: new double[] { y },
                color: (Color)color,
                lineWidth: 0,
                markerSize: markerSize,
                label: label,
                errorX: errorXarray,
                errorY: errorYarray,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

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
            if (color == null)
                color = settings.GetNextColor();

            PlottableScatter scatterPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                errorX: errorX,
                errorY: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle
                );

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

            PlottableScatterHighlight scatterPlot = new PlottableScatterHighlight(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: markerSize,
                label: label,
                errorX: errorX,
                errorY: errorY,
                errorLineWidth: errorLineWidth,
                errorCapSize: errorCapSize,
                stepDisplay: false,
                markerShape: markerShape,
                lineStyle: lineStyle,
                highlightedShape: highlightedShape,
                highlightedColor: highlightedColor.Value,
                highlightedMarkerSize: highlightedMarkerSize.Value
                );

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
            if (color is null)
                color = settings.GetNextColor();

            PlottableErrorBars errorBars = new PlottableErrorBars(
                xs,
                ys,
                xPositiveError,
                xNegativeError,
                yPositiveError,
                yNegativeError,
                color.Value,
                lineWidth,
                capWidth,
                label
                );

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

            PlottableScatter stepPlot = new PlottableScatter(
                xs: xs,
                ys: ys,
                color: (Color)color,
                lineWidth: lineWidth,
                markerSize: 0,
                label: label,
                errorX: null,
                errorY: null,
                errorLineWidth: 0,
                errorCapSize: 0,
                stepDisplay: true,
                markerShape: MarkerShape.none,
                lineStyle: LineStyle.Solid
                );

            Add(stepPlot);
            return stepPlot;
        }
    }
}
