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
            // TODO: support all of these
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
            settings.XAxis.TickCollection.manualTickPositions = positions;
            settings.XAxis.TickCollection.manualTickLabels = labels;
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
            settings.YAxis.TickCollection.manualTickPositions = positions;
            settings.YAxis.TickCollection.manualTickLabels = labels;
        }
    }
}
