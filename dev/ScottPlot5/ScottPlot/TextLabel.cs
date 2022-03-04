using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public class TextLabel
    {
        public string Text = string.Empty;

        public float FontSize = 12;
        public string FontName = "Consolas";
        public int FontWeight = 400;
        public FontStyleType FontStyle = FontStyleType.Normal;
        public Color FontColor = Colors.Black;

        public Color BackgroundColor = Colors.Transparent;

        public float OutlineWidth = 0;
        public Color OutlineColor = Colors.Black;

        public bool DebugTestPattern = false;

        public TextLabel()
        {

        }

        public TextLabel(string text)
        {
            Text = text;
        }

        public PixelSize Measure(ICanvas canvas)
        {
            Microsoft.Maui.Graphics.Font font = new(FontName, FontWeight, FontStyle);
            SizeF size = canvas.GetStringSize(Text, font, FontSize);
            return PixelSize.FromSizeF(size);
        }

        public void Draw(ICanvas canvas, float x, float y, float rotate, bool invert = false)
        {
            Microsoft.Maui.Graphics.Font font = new(FontName, FontWeight, FontStyle);
            SizeF stringSize = canvas.GetStringSize(Text, font, FontSize);
            float textHeight = invert ? -stringSize.Height : stringSize.Height;
            canvas.SaveState();
            canvas.Translate(x + textHeight, y);
            canvas.Rotate(rotate);
            Draw(canvas, 0, 0, HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.ResetState();
        }

        public void Draw(ICanvas canvas, float x, float y, HorizontalAlignment ha, VerticalAlignment va)
        {
            Microsoft.Maui.Graphics.Font font = new(FontName, FontWeight, FontStyle);
            SizeF stringSize = canvas.GetStringSize(Text, font, FontSize);

            x = ha switch
            {
                HorizontalAlignment.Left => x,
                HorizontalAlignment.Center => x - stringSize.Width / 2,
                HorizontalAlignment.Right => x - stringSize.Width,
                HorizontalAlignment.Justified => x - stringSize.Width / 2,
                _ => throw new NotImplementedException(),
            };

            y = va switch
            {
                VerticalAlignment.Top => y,
                VerticalAlignment.Center => y - stringSize.Height / 2,
                VerticalAlignment.Bottom => y - stringSize.Height,
                _ => throw new NotImplementedException(),
            };

            RectangleF rect = new(x, y, stringSize.Width, stringSize.Height);
            if (DebugTestPattern)
                Dev.Draw.RectangleTestPattern(canvas, rect);

            canvas.FontColor = FontColor;
            canvas.FontSize = FontSize - 1; // reduce size to prevent clipping

            canvas.DrawString(Text, rect, HorizontalAlignment.Center, VerticalAlignment.Center, TextFlow.OverflowBounds, lineSpacingAdjustment: 0);
        }
    }
}
