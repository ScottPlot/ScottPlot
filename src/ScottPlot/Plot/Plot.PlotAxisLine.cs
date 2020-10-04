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
        public PlottableVLine PlotVLine(
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
            if (color == null)
                color = settings.GetNextColor();

            PlottableVLine axLine = new PlottableVLine(
                position: x,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            Add(axLine);
            return axLine;
        }

        public PlottableVSpan PlotVSpan(
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
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableVSpan(
                position1: y1,
                position2: y2,
                color: (Color)color,
                alpha: alpha,
                label: label,
                draggable: draggable,
                dragFixedSize: dragFixedSize,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper
                );

            Add(axisSpan);
            return axisSpan;
        }

        public PlottableHLine PlotHLine(
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
            if (color == null)
                color = settings.GetNextColor();

            PlottableHLine axLine = new PlottableHLine(
                position: y,
                color: (Color)color,
                lineWidth: lineWidth,
                label: label,
                draggable: draggable,
                dragLimitLower: dragLimitLower,
                dragLimitUpper: dragLimitUpper,
                lineStyle: lineStyle
                );

            Add(axLine);
            return axLine;
        }

        public PlottableHSpan PlotHSpan(
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
            if (color == null)
                color = settings.GetNextColor();

            var axisSpan = new PlottableHSpan(
                    position1: x1,
                    position2: x2,
                    color: (Color)color,
                    alpha: alpha,
                    label: label,
                    draggable: draggable,
                    dragFixedSize: dragFixedSize,
                    dragLimitLower: dragLimitLower,
                    dragLimitUpper: dragLimitUpper
                    );

            Add(axisSpan);
            return axisSpan;
        }
    }
}
