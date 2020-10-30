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
            settings.CornerLegend.IsVisible = enableLegend;
            settings.CornerLegend.FontName = fontName ?? settings.CornerLegend.FontName;
            settings.CornerLegend.FontSize = fontSize ?? settings.CornerLegend.FontSize;
            settings.CornerLegend.FontColor = fontColor ?? settings.CornerLegend.FontColor;
            settings.CornerLegend.FillColor = backColor ?? settings.CornerLegend.FillColor;
            settings.CornerLegend.OutlineColor = frameColor ?? settings.CornerLegend.OutlineColor;
            settings.CornerLegend.ReverseOrder = reverseOrder ?? settings.CornerLegend.ReverseOrder;
            settings.CornerLegend.FontBold = bold ?? settings.CornerLegend.FontBold;
            settings.CornerLegend.FixedLineWidth = fixedLineWidth ?? settings.CornerLegend.FixedLineWidth;
            settings.CornerLegend.Location = location;
        }

        public Bitmap GetLegendBitmap()
        {
            RenderBitmap();
            return settings.CornerLegend.GetBitmap();
        }
    }
}
