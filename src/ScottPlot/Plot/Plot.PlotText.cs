/* Code here extends Plot module with methods to construct plottables.
 *   - Plottables created here are added to the plottables list and returned.
 *   - Long lists of optional arguments (matplotlib style) are permitted.
 *   - Use one line per argument to simplify the tracking of changes.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public partial class Plot
    {
        public PlottableText PlotText(
            string text,
            double x,
            double y,
            Color? color = null,
            string fontName = null,
            double fontSize = 12,
            bool bold = false,
            string label = null,
            TextAlignment alignment = TextAlignment.middleLeft,
            double rotation = 0,
            bool frame = false,
            Color? frameColor = null
            )
        {
            if (!string.IsNullOrWhiteSpace(label))
                Debug.WriteLine("WARNING: the PlotText() label argument is ignored");

            PlottableText plottableText = new PlottableText()
            {
                text = text,
                x = x,
                y = y,
                FontColor = color ?? settings.GetNextColor(),
                FontName = fontName ?? Config.Fonts.GetDefaultFontName(),
                FontSize = (float)fontSize,
                FontBold = bold,
                alignment = alignment,
                rotation = rotation,
                frame = frame,
                frameColor = frameColor ?? Color.White
            };
            Add(plottableText);
            return plottableText;
        }

        public PlottableAnnotation PlotAnnotation(
            string label,
            double xPixel = 10,
            double yPixel = 10,
            double fontSize = 12,
            string fontName = "Segoe UI",
            Color? fontColor = null,
            double fontAlpha = 1,
            bool fill = true,
            Color? fillColor = null,
            double fillAlpha = .2,
            double lineWidth = 1,
            Color? lineColor = null,
            double lineAlpha = 1,
            bool shadow = false
            )
        {
            fontColor = fontColor ?? Color.Black;
            fillColor = fillColor ?? Color.Yellow;
            lineColor = lineColor ?? Color.Black;

            fontColor = Color.FromArgb((int)(255 * fontAlpha), fontColor.Value.R, fontColor.Value.G, fontColor.Value.B);
            fillColor = Color.FromArgb((int)(255 * fillAlpha), fillColor.Value.R, fillColor.Value.G, fillColor.Value.B);
            lineColor = Color.FromArgb((int)(255 * lineAlpha), lineColor.Value.R, lineColor.Value.G, lineColor.Value.B);

            var plottable = new PlottableAnnotation()
            {
                xPixel = xPixel,
                yPixel = yPixel,
                label = label,
                FontSize = (float)fontSize,
                FontName = fontName,
                FontColor = fontColor.Value,
                Background = fill,
                BackgroundColor = fillColor.Value,
                BorderWidth = (float)lineWidth,
                BorderColor = lineColor.Value,
                Shadow = shadow
            };

            Add(plottable);
            return plottable;
        }

    }
}
