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
            legendLocation location = legendLocation.lowerRight,
            shadowDirection shadowDirection = shadowDirection.lowerRight,
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

            if (location == legendLocation.upperLeft)
                settings.Legend.Location = Direction.NW;
            else if (location == legendLocation.upperCenter)
                settings.Legend.Location = Direction.N;
            else if (location == legendLocation.upperRight)
                settings.Legend.Location = Direction.NE;
            else if (location == legendLocation.middleRight)
                settings.Legend.Location = Direction.E;
            else if (location == legendLocation.lowerRight)
                settings.Legend.Location = Direction.SE;
            else if (location == legendLocation.lowerCenter)
                settings.Legend.Location = Direction.S;
            else if (location == legendLocation.lowerLeft)
                settings.Legend.Location = Direction.SW;
            else if (location == legendLocation.middleLeft)
                settings.Legend.Location = Direction.W;
        }

        public Bitmap GetLegendBitmap()
        {
            if (settings.bmpData is null)
                RenderBitmap();
            return settings.Legend.GetBitmap(settings);
        }
    }
}
