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

            foreach (var axis in settings.Axes)
            {
                axis.Title.Font.Color = label ?? axis.Title.Font.Color;
                axis.Ticks.MajorLabelFont.Color = tick ?? axis.Ticks.MajorLabelFont.Color;
                axis.Ticks.MajorGridColor = grid ?? axis.Ticks.MajorGridColor;
                axis.Ticks.MinorGridColor = grid ?? axis.Ticks.MinorGridColor;
                axis.Ticks.Color = tick ?? axis.Ticks.Color;
                axis.Line.Color = tick ?? axis.Line.Color;
            }

            settings.XAxis2.Title.Font.Color = title ?? settings.XAxis2.Title.Font.Color;
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
                axis.Line.IsVisible = drawFrame ?? axis.Line.IsVisible;
                axis.Line.Color = frameColor ?? axis.Line.Color;
            }

            settings.YAxis.Line.IsVisible = left ?? settings.YAxis.Line.IsVisible;
            settings.YAxis2.Line.IsVisible = right ?? settings.YAxis2.Line.IsVisible;
            settings.XAxis.Line.IsVisible = bottom ?? settings.XAxis.Line.IsVisible;
            settings.XAxis2.Line.IsVisible = top ?? settings.XAxis2.Line.IsVisible;
        }

        public void Benchmark(bool show = true, bool toggle = false) =>
            settings.BenchmarkMessage.IsVisible = toggle ? !settings.BenchmarkMessage.IsVisible : show;

        /// <summary>
        /// Adjust layout to accommodate axis title and tick label sizes
        /// </summary>
        public void TightenLayout(int? padding = null, bool render = false)
        {
            if (padding.HasValue)
                foreach (var axis in settings.Axes)
                    axis.PixelSizeMinimum = padding.Value;

            if (render)
                GetBitmap();

            if (!settings.axes.hasBeenSet && settings.Plottables.Count > 0)
                settings.AxisAuto();

            settings.Resize(settings.Width, settings.Height);
        }

        public void LayoutFrameless()
        {
            foreach (var axis in settings.Axes)
            {
                axis.IsVisible = false;
                axis.PixelSizeMinimum = 0;
                axis.PixelSizePadding = 0;
            }
        }

        /// <summary>
        /// Define minimum size (in pixels) around the edge of the data area for each axis
        /// </summary>
        public void Layout(float? left = null, float? right = null, float? bottom = null, float? top = null, float? padding = 5)
        {
            settings.YAxis.PixelSizeMinimum = left ?? settings.YAxis.PixelSize;
            settings.YAxis2.PixelSizeMinimum = right ?? settings.YAxis2.PixelSize;
            settings.XAxis.PixelSizeMinimum = bottom ?? settings.XAxis.PixelSize;
            settings.XAxis2.PixelSizeMinimum = top ?? settings.XAxis2.PixelSize;

            foreach (var axis in settings.Axes)
                axis.PixelSizePadding = padding ?? axis.PixelSizePadding;
        }

        [Obsolete("use other overload", true)]
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
            DateTimeUnit? xSpacingDateTimeUnit = null,
            DateTimeUnit? ySpacingDateTimeUnit = null,
            float? lineWidth = null,
            LineStyle? lineStyle = null,
            bool? snapToNearestPixel = null
            )
        {
            settings.XAxis.Ticks.MajorGridEnable = enable ?? settings.XAxis.Ticks.MajorGridEnable;
            settings.YAxis.Ticks.MajorGridEnable = enable ?? settings.YAxis.Ticks.MajorGridEnable;
            settings.XAxis.Ticks.MajorGridEnable = enableHorizontal ?? settings.XAxis.Ticks.MajorGridEnable;
            settings.YAxis.Ticks.MajorGridEnable = enableVertical ?? settings.YAxis.Ticks.MajorGridEnable;

            settings.XAxis.Ticks.MajorGridColor = color ?? settings.XAxis.Ticks.MajorGridColor;
            settings.YAxis.Ticks.MajorGridColor = color ?? settings.YAxis.Ticks.MajorGridColor;

            settings.XAxis.Ticks.TickCollection.manualSpacingX = xSpacing ?? settings.XAxis.Ticks.TickCollection.manualSpacingX;
            settings.YAxis.Ticks.TickCollection.manualSpacingY = ySpacing ?? settings.YAxis.Ticks.TickCollection.manualSpacingY;
            settings.XAxis.Ticks.TickCollection.manualDateTimeSpacingUnitX = xSpacingDateTimeUnit ?? settings.XAxis.Ticks.TickCollection.manualDateTimeSpacingUnitX;
            settings.YAxis.Ticks.TickCollection.manualDateTimeSpacingUnitY = ySpacingDateTimeUnit ?? settings.YAxis.Ticks.TickCollection.manualDateTimeSpacingUnitY;

            settings.XAxis.Ticks.MajorGridWidth = lineWidth ?? settings.XAxis.Ticks.MajorGridWidth;
            settings.YAxis.Ticks.MajorGridWidth = lineWidth ?? settings.YAxis.Ticks.MajorGridWidth;

            settings.XAxis.Ticks.MajorGridStyle = lineStyle ?? settings.XAxis.Ticks.MajorGridStyle;
            settings.YAxis.Ticks.MajorGridStyle = lineStyle ?? settings.YAxis.Ticks.MajorGridStyle;

            settings.XAxis.Ticks.SnapPx = snapToNearestPixel ?? settings.XAxis.Ticks.SnapPx;
            settings.YAxis.Ticks.SnapPx = snapToNearestPixel ?? settings.YAxis.Ticks.SnapPx;
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
