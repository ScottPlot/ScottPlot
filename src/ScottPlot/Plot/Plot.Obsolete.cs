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
using ScottPlot.Plottable;

namespace ScottPlot
{
    public partial class Plot
    {
        [Obsolete("Use AddAnnotation() and customize the object it returns")]
        public Annotation PlotAnnotation(
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

            var plottable = new Annotation()
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

        [Obsolete("Use AddArrow() and customize the object it returns")]
        public ScatterPlot PlotArrow(
            double tipX,
            double tipY,
            double baseX,
            double baseY,
            double lineWidth = 5,
            float arrowheadWidth = 3,
            float arrowheadLength = 3,
            Color? color = null,
            string label = null
            )
        {
            var scatter = PlotScatter(
                                        xs: new double[] { baseX, tipX },
                                        ys: new double[] { baseY, tipY },
                                        color: color,
                                        lineWidth: lineWidth,
                                        label: label,
                                        markerSize: 0
                                    );

            scatter.ArrowheadLength = arrowheadLength;
            scatter.ArrowheadWidth = arrowheadWidth;
            return scatter;
        }

        [Obsolete("Use AddText() and customize the object it returns")]
        public Text PlotText(
            string text,
            double x,
            double y,
            Color? color = null,
            string fontName = null,
            double fontSize = 12,
            bool bold = false,
            string label = null,
            Alignment alignment = Alignment.MiddleLeft,
            double rotation = 0,
            bool frame = false,
            Color? frameColor = null
            )
        {
            if (!string.IsNullOrWhiteSpace(label))
                Debug.WriteLine("WARNING: the PlotText() label argument is ignored");

            Text plottableText = new Text()
            {
                text = text,
                x = x,
                y = y,
                FontColor = color ?? settings.GetNextColor(),
                FontName = fontName,
                FontSize = (float)fontSize,
                FontBold = bold,
                alignment = alignment,
                rotation = (float)rotation,
                FillBackground = frame,
                BackgroundColor = frameColor ?? Color.White
            };
            Add(plottableText);
            return plottableText;
        }
    }
}
