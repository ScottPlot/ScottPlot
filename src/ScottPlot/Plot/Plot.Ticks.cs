/* Code here customizes tick styling and behavior */
using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        public void Ticks(
            bool? displayTicksX = null,
            bool? displayTicksY = null,
            bool? displayTicksXminor = null,
            bool? displayTicksYminor = null,
            bool? displayTickLabelsX = null,
            bool? displayTickLabelsY = null,
            Color? color = null,
            bool? useMultiplierNotation = null,
            bool? useOffsetNotation = null,
            bool? useExponentialNotation = null,
            bool? dateTimeX = null,
            bool? dateTimeY = null,
            bool? rulerModeX = null,
            bool? rulerModeY = null,
            bool? invertSignX = null,
            bool? invertSignY = null,
            string fontName = null,
            float? fontSize = null,
            double? xTickRotation = null,
            bool? logScaleX = null,
            bool? logScaleY = null,
            string numericFormatStringX = null,
            string numericFormatStringY = null,
            bool? snapToNearestPixel = null,
            int? baseX = null,
            int? baseY = null,
            string prefixX = null,
            string prefixY = null,
            string dateTimeFormatStringX = null,
            string dateTimeFormatStringY = null
            )
        {
            settings.XAxis.MajorTicks.IsVisible = displayTicksX ?? settings.XAxis.MajorTicks.IsVisible;
            settings.YAxis.MajorTicks.IsVisible = displayTicksY ?? settings.YAxis.MajorTicks.IsVisible;
            settings.XAxis.MinorTicks.IsVisible = displayTicksXminor ?? settings.XAxis.MinorTicks.IsVisible;
            settings.YAxis.MinorTicks.IsVisible = displayTicksYminor ?? settings.YAxis.MinorTicks.IsVisible;

            settings.YAxis.MajorTicks.MarkColor = color ?? settings.YAxis.MajorTicks.MarkColor;
            settings.YAxis.MinorTicks.MarkColor = color ?? settings.YAxis.MinorTicks.MarkColor;
            settings.YAxis.LineColor = color ?? settings.YAxis.LineColor;
            settings.YAxis.TickFont.Color = color ?? settings.YAxis.TickFont.Color;


            settings.XAxis.TitleFont.Size = fontSize ?? settings.XAxis.TitleFont.Size;
            settings.YAxis.TitleFont.Size = fontSize ?? settings.YAxis.TitleFont.Size;
            settings.XAxis.TitleFont.Name = fontName ?? settings.XAxis.TitleFont.Name;
            settings.YAxis.TitleFont.Name = fontName ?? settings.YAxis.TitleFont.Name;
            settings.XAxis.MajorTicks.IsLabelVisible = displayTickLabelsX ?? settings.XAxis.MajorTicks.IsLabelVisible;
            settings.YAxis.MajorTicks.IsLabelVisible = displayTickLabelsY ?? settings.YAxis.MajorTicks.IsLabelVisible;

            settings.XAxis.MajorTicks.LabelRotation = (float)(xTickRotation ?? settings.XAxis.MajorTicks.LabelRotation);

            settings.XAxis.MajorTicks.PixelSnap = snapToNearestPixel ?? settings.XAxis.MajorTicks.PixelSnap;
            settings.XAxis.MinorTicks.PixelSnap = snapToNearestPixel ?? settings.XAxis.MinorTicks.PixelSnap;
            settings.YAxis.MajorTicks.PixelSnap = snapToNearestPixel ?? settings.YAxis.MajorTicks.PixelSnap;
            settings.YAxis.MinorTicks.PixelSnap = snapToNearestPixel ?? settings.YAxis.MinorTicks.PixelSnap;

            if (useMultiplierNotation != null)
                settings.ticks.useMultiplierNotation = (bool)useMultiplierNotation;
            if (useOffsetNotation != null)
                settings.ticks.useOffsetNotation = (bool)useOffsetNotation;
            if (useExponentialNotation != null)
                settings.ticks.useExponentialNotation = (bool)useExponentialNotation;
            if (dateTimeX != null)
                settings.ticks.x.dateFormat = (bool)dateTimeX;
            if (dateTimeY != null)
                settings.ticks.y.dateFormat = (bool)dateTimeY;
            if (rulerModeX != null)
                settings.ticks.rulerModeX = (bool)rulerModeX;
            if (rulerModeY != null)
                settings.ticks.rulerModeY = (bool)rulerModeY;
            if (invertSignX != null)
                settings.ticks.x.invertSign = (bool)invertSignX;
            if (invertSignY != null)
                settings.ticks.y.invertSign = (bool)invertSignY;
            if (logScaleX != null)
                settings.ticks.x.logScale = logScaleX.Value;
            if (logScaleY != null)
                settings.ticks.y.logScale = logScaleY.Value;
            if (numericFormatStringX != null)
                settings.ticks.x.numericFormatString = numericFormatStringX;
            if (numericFormatStringY != null)
                settings.ticks.y.numericFormatString = numericFormatStringY;
            if (dateTimeFormatStringX != null)
                settings.ticks.x.dateTimeFormatString = dateTimeFormatStringX;
            if (dateTimeFormatStringY != null)
                settings.ticks.y.dateTimeFormatString = dateTimeFormatStringY;

            settings.ticks.x.radix = baseX ?? settings.ticks.x.radix;
            settings.ticks.x.prefix = prefixX ?? settings.ticks.x.prefix;
            settings.ticks.y.radix = baseY ?? settings.ticks.y.radix;
            settings.ticks.y.prefix = prefixY ?? settings.ticks.y.prefix;

            // dont use offset notation if the sign is inverted
            if (settings.ticks.x.invertSign || settings.ticks.y.invertSign)
                settings.ticks.useOffsetNotation = false;

            if (dateTimeX != null || dateTimeY != null)
            {
                TightenLayout();
                RenderBitmap(lowQuality: true);
            }

            TightenLayout();
        }

        public void XTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            XTicks(DataGen.Consecutive(labels.Length), labels);
        }

        public void XTicks(double[] positions = null, string[] labels = null)
        {
            TightenLayout();
            settings.ticks.x.manualTickPositions = positions;
            settings.ticks.x.manualTickLabels = labels;
        }

        public void YTicks(string[] labels)
        {
            if (labels is null)
                throw new ArgumentException("labels cannot be null");

            YTicks(DataGen.Consecutive(labels.Length), labels);
        }

        public void YTicks(double[] positions = null, string[] labels = null)
        {
            TightenLayout();
            settings.ticks.y.manualTickPositions = positions;
            settings.ticks.y.manualTickLabels = labels;
        }
    }
}
