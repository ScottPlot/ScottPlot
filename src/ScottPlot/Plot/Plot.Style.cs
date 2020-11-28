/* Code here relates to customization of the figure or plot layout or styling (not behavior) */

using ScottPlot.Ticks;
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
            var primaryAxes = new Renderable.Axis[] { XAxis, XAxis2, YAxis, YAxis2 };
            foreach (var axis in primaryAxes)
            {
                axis.Line.IsVisible = drawFrame ?? axis.Line.IsVisible;
                axis.Line.Color = frameColor ?? axis.Line.Color;
            }

            settings.YAxis.Line.IsVisible = left ?? settings.YAxis.Line.IsVisible;
            settings.YAxis2.Line.IsVisible = right ?? settings.YAxis2.Line.IsVisible;
            settings.XAxis.Line.IsVisible = bottom ?? settings.XAxis.Line.IsVisible;
            settings.XAxis2.Line.IsVisible = top ?? settings.XAxis2.Line.IsVisible;
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
            if (!sourcePlot.GetSettings(showWarning: false).AllAxesHaveBeenSet)
                sourcePlot.AxisAuto();

            if (!settings.AllAxesHaveBeenSet)
                AxisAuto();

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
