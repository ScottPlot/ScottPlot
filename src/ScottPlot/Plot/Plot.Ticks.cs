/* Code here customizes tick styling and behavior */
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
            if (displayTicksX != null)
                settings.ticks.displayXmajor = (bool)displayTicksX;
            if (displayTicksY != null)
                settings.ticks.displayYmajor = (bool)displayTicksY;
            if (color != null)
                settings.ticks.color = (Color)color;
            if (useMultiplierNotation != null)
                settings.ticks.useMultiplierNotation = (bool)useMultiplierNotation;
            if (useOffsetNotation != null)
                settings.ticks.useOffsetNotation = (bool)useOffsetNotation;
            if (useExponentialNotation != null)
                settings.ticks.useExponentialNotation = (bool)useExponentialNotation;
            if (displayTicksXminor != null)
                settings.ticks.displayXminor = (bool)displayTicksXminor;
            if (displayTicksYminor != null)
                settings.ticks.displayYminor = (bool)displayTicksYminor;
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
            if (fontSize != null)
                settings.ticks.fontSize = (float)fontSize;
            if (fontName != null)
                settings.ticks.fontName = fontName;
            if (displayTickLabelsX != null)
                settings.ticks.displayXlabels = (bool)displayTickLabelsX;
            if (displayTickLabelsY != null)
                settings.ticks.displayYlabels = (bool)displayTickLabelsY;
            if (xTickRotation != null)
                settings.ticks.rotationX = xTickRotation.Value;
            if (logScaleX != null)
                settings.ticks.x.logScale = logScaleX.Value;
            if (logScaleY != null)
                settings.ticks.y.logScale = logScaleY.Value;
            if (numericFormatStringX != null)
                settings.ticks.x.numericFormatString = numericFormatStringX;
            if (numericFormatStringY != null)
                settings.ticks.y.numericFormatString = numericFormatStringY;
            if (snapToNearestPixel != null)
                settings.ticks.snapToNearestPixel = snapToNearestPixel.Value;
            if (dateTimeFormatStringX != null)
                settings.ticks.x.dateTimeFormatString = dateTimeFormatStringX;
            if (dateTimeFormatStringY != null)
                settings.ticks.y.dateTimeFormatString = dateTimeFormatStringY;

            if (baseX != null)
            {
                settings.ticks.x.radix = baseX.Value;
                settings.ticks.x.prefix = prefixX;
            }
            if (baseY != null)
            {
                settings.ticks.y.radix = baseY.Value;
                settings.ticks.y.prefix = prefixY;
            }

            // dont use offset notation if the sign is inverted
            if (settings.ticks.x.invertSign || settings.ticks.y.invertSign)
                settings.ticks.useOffsetNotation = false;

            if (dateTimeX != null || dateTimeY != null)
            {
                // why these in this order? voodoo magic
                TightenLayout();
                RenderBitmap();
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
