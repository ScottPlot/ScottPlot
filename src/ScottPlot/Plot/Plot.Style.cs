/* Code here relates to customization of the figure or plot layout or styling (not behavior) */

using System;
using System.Collections.Generic;
using System.Drawing;
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
            if (drawFrame != null)
                settings.layout.displayAxisFrames = (bool)drawFrame;
            if (frameColor != null)
                settings.ticks.color = (Color)frameColor;
            if (left != null)
                settings.layout.displayFrameByAxis[0] = (bool)left;
            if (right != null)
                settings.layout.displayFrameByAxis[1] = (bool)right;
            if (bottom != null)
                settings.layout.displayFrameByAxis[2] = (bool)bottom;
            if (top != null)
                settings.layout.displayFrameByAxis[3] = (bool)top;
            TightenLayout();
        }

        public void Benchmark(bool show = true, bool toggle = false) =>
            settings.BenchmarkMessage.IsVisible = toggle ? !settings.BenchmarkMessage.IsVisible : show;

        [Obsolete("dont use this old system", false)]
        public void TightenLayout(int? padding = null, bool render = false)
        {
            if (render)
                GetBitmap();
            if (!settings.axes.hasBeenSet && settings.plottables.Count > 0)
                settings.AxisAuto();

            settings.ticks?.x?.Recalculate(settings); // this probably never happens
            settings.ticks?.y?.Recalculate(settings); // this probably never happens

            settings.Resize(settings.Width, settings.Height);
            settings.layout.Update(settings.Width, settings.Height);
            settings.layout.tighteningOccurred = true;

            Resize();
        }

        [Obsolete("dont use this old layout system", false)]
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
            TightenLayout(render: true);

            if (yLabelWidth != null) settings.layout.yLabelWidth = (int)yLabelWidth;
            if (yScaleWidth != null) settings.layout.yScaleWidth = (int)yScaleWidth;
            if (y2LabelWidth != null) settings.layout.y2LabelWidth = (int)y2LabelWidth;
            if (y2ScaleWidth != null) settings.layout.y2ScaleWidth = (int)y2ScaleWidth;
            if (titleHeight != null) settings.layout.titleHeight = (int)titleHeight;
            if (xLabelHeight != null) settings.layout.xLabelHeight = (int)xLabelHeight;
            if (xScaleHeight != null) settings.layout.xScaleHeight = (int)xScaleHeight;

            Resize();
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
                settings.XAxis.MajorTicks.GridEnable = enable.Value;
                settings.YAxis.MajorTicks.GridEnable = enable.Value;
            }

            if (enableHorizontal != null)
                settings.XAxis.MajorTicks.GridEnable = enableHorizontal.Value;

            if (enableVertical != null)
                settings.YAxis.MajorTicks.GridEnable = enableVertical.Value;

            if (color != null)
            {
                settings.XAxis.MajorTicks.GridLineColor = color.Value;
                settings.YAxis.MajorTicks.GridLineColor = color.Value;
            }

            if (xSpacing != null)
                settings.ticks.manualSpacingX = xSpacing.Value;

            if (ySpacing != null)
                settings.ticks.manualSpacingY = ySpacing.Value;

            if (xSpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitX = xSpacingDateTimeUnit.Value;

            if (ySpacingDateTimeUnit != null)
                settings.ticks.manualDateTimeSpacingUnitY = ySpacingDateTimeUnit.Value;

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

            var sourceLayout = sourcePlot.GetSettings(false).layout;

            if (horizontal)
            {
                settings.layout.yLabelWidth = sourceLayout.yLabelWidth;
                settings.layout.y2LabelWidth = sourceLayout.y2LabelWidth;
                settings.layout.yScaleWidth = sourceLayout.yScaleWidth;
                settings.layout.y2ScaleWidth = sourceLayout.y2ScaleWidth;
            }

            if (vertical)
            {
                settings.layout.titleHeight = sourceLayout.titleHeight;
                settings.layout.xLabelHeight = sourceLayout.xLabelHeight;
                settings.layout.xScaleHeight = sourceLayout.xScaleHeight;
            }
        }
    }
}
