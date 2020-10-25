using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        public void Legend(
            bool enableLegend = true,
            string fontName = null,
            float? fontSize = null,
            bool? bold = null,
            Color? fontColor = null,
            Color? backColor = null,
            Color? frameColor = null,
            Alignment location = Alignment.LowerRight,
            Alignment shadowDirection = Alignment.LowerRight,
            bool? fixedLineWidth = null,
            bool? reverseOrder = null
            )
        {
            settings.Legend.Visible = enableLegend;
            if (fontName != null)
                settings.Legend.FontName = fontName;
            if (fontSize != null)
                settings.Legend.FontSize = fontSize.Value;
            if (fontColor != null)
                settings.Legend.FontColor = fontColor.Value;
            if (backColor != null)
                settings.Legend.FillColor = backColor.Value;
            if (frameColor != null)
                settings.Legend.OutlineColor = frameColor.Value;
            if (reverseOrder != null)
                settings.Legend.ReverseOrder = reverseOrder.Value;
            if (bold != null)
                settings.Legend.FontBold = bold.Value;
            if (fixedLineWidth != null)
                settings.Legend.FixedLineWidth = fixedLineWidth.Value;

            settings.Legend.Location = location;
        }

        public Bitmap GetLegendBitmap()
        {
            if (settings.bmpData is null)
                RenderBitmap();
            return settings.Legend.GetBitmap(settings);
        }
    }
}
