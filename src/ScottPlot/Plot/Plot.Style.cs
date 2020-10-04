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
        public Drawing.Colorset Colorset(Drawing.Colorset colorset = null)
        {
            if (colorset != null)
                settings.colorset = colorset;

            return settings.colorset;
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
            if (figBg != null)
                settings.FigureBackground.Color = figBg.Value;
            if (dataBg != null)
                settings.DataBackground.Color = dataBg.Value;
            if (grid != null)
            {
                settings.HorizontalGridLines.Color = grid.Value;
                settings.VerticalGridLines.Color = grid.Value;
            }
            if (tick != null)
                settings.ticks.color = (Color)tick;
            if (label != null)
                settings.xLabel.color = (Color)label;
            if (label != null)
                settings.yLabel.color = (Color)label;
            if (title != null)
                settings.title.color = (Color)title;
            if (dataBg != null)
                settings.Legend.FillColor = (Color)dataBg;
            if (tick != null)
                settings.Legend.OutlineColor = (Color)tick;
            if (label != null)
                settings.Legend.FontColor = (Color)label;
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

        public void Benchmark(bool show = true, bool toggle = false)
        {
            if (toggle)
                settings.Benchmark.Visible = !settings.Benchmark.Visible;
            else
                settings.Benchmark.Visible = show;
        }

        public void TightenLayout(int? padding = null, bool render = false)
        {
            if (settings.gfxData is null)
                return;

            if (render)
                GetBitmap();
            if (!settings.axes.hasBeenSet && settings.plottables.Count > 0)
                settings.AxisAuto();

            settings.ticks?.x?.Recalculate(settings); // this probably never happens
            settings.ticks?.y?.Recalculate(settings); // this probably never happens

            int pad = (padding is null) ? 15 : (int)padding;
            settings.TightenLayout(pad, pad, pad, pad);

            Resize();
        }

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
                settings.HorizontalGridLines.Visible = enable.Value;
                settings.VerticalGridLines.Visible = enable.Value;
            }

            if (enableHorizontal != null)
                settings.HorizontalGridLines.Visible = enableHorizontal.Value;

            if (enableVertical != null)
                settings.VerticalGridLines.Visible = enableVertical.Value;

            if (color != null)
            {
                settings.HorizontalGridLines.Color = color.Value;
                settings.VerticalGridLines.Color = color.Value;
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
                settings.HorizontalGridLines.LineWidth = (float)lineWidth.Value;
                settings.VerticalGridLines.LineWidth = (float)lineWidth.Value;
            }

            if (lineStyle != null)
            {
                settings.HorizontalGridLines.LineStyle = lineStyle.Value;
                settings.VerticalGridLines.LineStyle = lineStyle.Value;
            }

            if (snapToNearestPixel != null)
            {
                settings.HorizontalGridLines.SnapToNearestPixel = snapToNearestPixel.Value;
                settings.VerticalGridLines.SnapToNearestPixel = snapToNearestPixel.Value;
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
