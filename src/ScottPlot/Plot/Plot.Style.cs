/* Code here relates to customization of the figure or plot layout or styling (not behavior) */

using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {
        /// <summary>
        /// Change the default color palette for new plottables
        /// </summary>
        public Drawing.Palette Colorset(Drawing.Palette colorset = null)
        {
            if (colorset != null)
                settings.PlottablePalette = colorset;

            return settings.PlottablePalette;
        }

        public void Style(
            Color? figBg = null,
            Color? dataBg = null,
            Color? grid = null,
            Color? tick = null,
            Color? label = null,
            Color? title = null
            )
        {
            settings.DataBackground.Color = dataBg ?? settings.DataBackground.Color;
            settings.FigureBackground.Color = figBg ?? settings.FigureBackground.Color;

            foreach (var axis in settings.AllAxes)
            {
                axis.TitleFont.Color = label ?? axis.TitleFont.Color;
                axis.TickFont.Color = tick ?? axis.TickFont.Color;
                axis.MajorTicks.GridLineColor = grid ?? axis.MajorTicks.GridLineColor;
                axis.MinorTicks.GridLineColor = grid ?? axis.MajorTicks.GridLineColor;
                axis.MajorTicks.MarkColor = tick ?? axis.MajorTicks.MarkColor;
                axis.MinorTicks.MarkColor = tick ?? axis.MinorTicks.MarkColor;
            }

            settings.XAxis2.TitleFont.Color = title ?? settings.XAxis2.TitleFont.Color;
        }

        public void Style(Style style)
        {
            StyleTools.SetStyle(this, style);
        }

        public void Frame(
            bool? drawFrame = true,
            Color? frameColor = null,
            bool? left = true,
            bool? right = true,
            bool? bottom = true,
            bool? top = true
        )
        {
            foreach (var axis in settings.PrimaryAxes)
            {
                axis.Line = drawFrame ?? axis.Line;
                axis.LineColor = frameColor ?? axis.LineColor;
            }

            settings.YAxis.Line = left ?? settings.YAxis.Line;
            settings.YAxis2.Line = right ?? settings.YAxis2.Line;
            settings.XAxis.Line = bottom ?? settings.XAxis.Line;
            settings.XAxis2.Line = top ?? settings.XAxis2.Line;

            TightenLayout();
        }

        public void Benchmark(bool show = true, bool toggle = false) =>
            settings.BenchmarkMessage.IsVisible = toggle ? !settings.BenchmarkMessage.IsVisible : show;

        [Obsolete("dont use this old system", false)]
        public void TightenLayout(int? padding = null, bool render = false)
        {
            if (render)
                GetBitmap();

            if (!settings.axes.hasBeenSet && settings.Plottables.Count > 0)
                settings.AxisAuto();

            settings.ticks?.x?.Recalculate(settings); // this probably never happens
            settings.ticks?.y?.Recalculate(settings); // this probably never happens

            settings.Resize(settings.Width, settings.Height);

            Resize();
        }

        public void Layout(double? leftAxisWidth, double? rightAxisWidth, double? bottomAxisHeight, double? topAxisHeight)
        {
            settings.YAxis.PixelSize = (float)(leftAxisWidth ?? settings.YAxis.PixelSize);
            settings.YAxis2.PixelSize = (float)(rightAxisWidth ?? settings.YAxis2.PixelSize);
            settings.XAxis.PixelSize = (float)(bottomAxisHeight ?? settings.XAxis.PixelSize);
            settings.XAxis2.PixelSize = (float)(topAxisHeight ?? settings.XAxis2.PixelSize);
        }

        [Obsolete("use other overload", false)]
        public void Layout(
                double? yLabelWidth = null,
                double? yScaleWidth = null,
                double? y2LabelWidth = null,
                double? y2ScaleWidth = null,
                double? titleHeight = null,
                double? xLabelHeight = null,
                double? xScaleHeight = null
            )
        {
        }

        public void Grid(
            bool? enable = null,
            Color? color = null,
            double? xSpacing = null,
            double? ySpacing = null,
            bool? enableHorizontal = null,
            bool? enableVertical = null,
            Config.DateTimeUnit? xSpacingDateTimeUnit = null,
            Config.DateTimeUnit? ySpacingDateTimeUnit = null,
            double? lineWidth = null,
            LineStyle? lineStyle = null,
            bool? snapToNearestPixel = null
            )
        {
            if (enable != null)
            {
                settings.XAxis.MajorTicks.IsGridVisible = enable.Value;
                settings.YAxis.MajorTicks.IsGridVisible = enable.Value;
            }

            if (enableHorizontal != null)
                settings.XAxis.MajorTicks.IsGridVisible = enableHorizontal.Value;

            if (enableVertical != null)
                settings.YAxis.MajorTicks.IsGridVisible = enableVertical.Value;

            if (color != null)
            {
                settings.XAxis.MajorTicks.GridLineColor = color.Value;
                settings.YAxis.MajorTicks.GridLineColor = color.Value;
            }

            // TODO: support this with new axis system
            /*
            if (xSpacing != null)
                settings.ticks.manualSpacingX = xSpacing.Value;
            if (ySpacing != null)
                settings.ticks.manualSpacingY = ySpacing.Value;
            if (xSpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitX = xSpacingDateTimeUnit.Value;
            if (ySpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitY = ySpacingDateTimeUnit.Value;
            */

            if (lineWidth != null)
            {
                settings.XAxis.MajorTicks.GridLineWidth = (float)lineWidth.Value;
                settings.YAxis.MajorTicks.GridLineWidth = (float)lineWidth.Value;
            }

            if (lineStyle != null)
            {
                settings.XAxis.MajorTicks.GridLineStyle = lineStyle.Value;
                settings.YAxis.MajorTicks.GridLineStyle = lineStyle.Value;
            }

            if (snapToNearestPixel != null)
            {
                settings.XAxis.MajorTicks.PixelSnap = snapToNearestPixel.Value;
                settings.YAxis.MajorTicks.PixelSnap = snapToNearestPixel.Value;
            }
        }

        public void MatchLayout(Plot sourcePlot, bool horizontal = true, bool vertical = true)
        {
            if (!sourcePlot.GetSettings(showWarning: false).axes.hasBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.axes.hasBeenSet)
                AxisAuto();

            Resize();

            var sourceSettings = sourcePlot.GetSettings(false);

            if (horizontal)
            {
                settings.YAxis.PixelSize = sourceSettings.YAxis.PixelSize;
                settings.YAxis2.PixelSize = sourceSettings.YAxis2.PixelSize;
            }

            if (vertical)
            {
                settings.XAxis.PixelSize = sourceSettings.XAxis.PixelSize;
                settings.XAxis2.PixelSize = sourceSettings.XAxis2.PixelSize;
            }
        }
    }
}
