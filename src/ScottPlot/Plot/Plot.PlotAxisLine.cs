﻿/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Plottable;

namespace ScottPlot
{
    public partial class Plot
    {
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
            };
            axLine.SetLimits(dragLimitLower, dragLimitUpper, null, null);

            Add(axLine);
            return axLine;
        }

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
                color = color ?? settings.GetNextColor(),
                alpha = alpha,
                label = label,
                DragEnabled = draggable,
                DragFixedSize = dragFixedSize,
            };
            axisSpan.SetLimits(null, null, dragLimitLower, dragLimitUpper);
            Add(axisSpan);
            return axisSpan;
        }

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
            };

            hline.SetLimits(null, null, dragLimitLower, dragLimitUpper);

            Add(hline);
            return hline;
        }

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
                color = color ?? settings.GetNextColor(),
                alpha = alpha,
                label = label,
                DragEnabled = draggable,
                DragFixedSize = dragFixedSize
            };
            axisSpan.SetLimits(dragLimitLower, dragLimitUpper, null, null);

            Add(axisSpan);
            return axisSpan;
        }
    }
}
