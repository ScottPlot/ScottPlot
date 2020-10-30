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
            CornerLegend.IsVisible = enableLegend;
            CornerLegend.FontName = fontName ?? CornerLegend.FontName;
            CornerLegend.FontSize = fontSize ?? CornerLegend.FontSize;
            CornerLegend.FontColor = fontColor ?? CornerLegend.FontColor;
            CornerLegend.FillColor = backColor ?? CornerLegend.FillColor;
            CornerLegend.OutlineColor = frameColor ?? CornerLegend.OutlineColor;
            CornerLegend.ReverseOrder = reverseOrder ?? CornerLegend.ReverseOrder;
            CornerLegend.FontBold = bold ?? CornerLegend.FontBold;
            CornerLegend.FixedLineWidth = fixedLineWidth ?? CornerLegend.FixedLineWidth;
            CornerLegend.Location = location;
        }

        public Bitmap GetLegendBitmap()
        {
            RenderBitmap();
            return CornerLegend.GetBitmap();
        }
    }
}
