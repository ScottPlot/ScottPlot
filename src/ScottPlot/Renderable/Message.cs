using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace ScottPlot.Renderable
{
    public class BenchmarkMessage : Message
    {
        private readonly Stopwatch Sw = new Stopwatch();
        public double MSec { get { return Sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency; } }
        public double Hz { get { return (MSec > 0) ? 1000.0 / MSec : 0; } }
        public string Message { get => $"Rendered in {MSec:00.00} ms ({Hz:0000.00} Hz)"; }

        private int MaxRenderTimes = 100;
        private List<double> RenderTimes = new();

        public void Restart() => Sw.Restart();
        public void Stop()
        {
            Sw.Stop();

            RenderTimes.Add(Sw.Elapsed.TotalMilliseconds);
            while (RenderTimes.Count > MaxRenderTimes)
                RenderTimes.RemoveAt(0);

            Text = Message;
        }

        /// <summary>
        /// Returns an array of render times (in milliseconds) of the last several renders.
        /// The most recent renders are at the end of the array.
        /// </summary>
        public double[] GetRenderTimes() => RenderTimes.ToArray();

        public BenchmarkMessage()
        {
            HAlign = HorizontalAlignment.Left;
            VAlign = VerticalAlignment.Lower;
            FontColor = Color.Black;
            FillColor = Color.FromArgb(200, Color.Yellow);
            BorderColor = Color.Black;
        }
    }

    public class ErrorMessage : Message
    {
        public ErrorMessage()
        {
            HAlign = HorizontalAlignment.Left;
            VAlign = VerticalAlignment.Upper;
            FontColor = Color.Black;
            FillColor = Color.FromArgb(50, Color.Red);
            BorderColor = Color.Black;
        }
    }

    public class Message : IRenderable
    {
        public string Text;

        public HorizontalAlignment HAlign;
        public VerticalAlignment VAlign;

        public Color FontColor = Color.Black;
        public string FontName = InstalledFont.Monospace();
        public float FontSize = 12;
        public bool FontBold = false;

        public Color FillColor = Color.LightGray;
        public Color BorderColor = Color.Black;
        public float BorderWidth = 1;

        public float Padding = 3;

        public bool IsVisible { get; set; } = false;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible || string.IsNullOrWhiteSpace(Text))
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            using (var font = GDI.Font(FontName, FontSize, FontBold))
            using (var fontBrush = new SolidBrush(FontColor))
            using (var fillBrush = new SolidBrush(FillColor))
            using (var borderPen = new Pen(BorderColor, BorderWidth))
            {
                SizeF textSize = GDI.MeasureString(gfx, Text, font);
                float textHeight = textSize.Height;
                float textWidth = textSize.Width;

                float textY = 0;
                if (VAlign == VerticalAlignment.Upper)
                    textY = dims.DataOffsetY + Padding;
                else if (VAlign == VerticalAlignment.Middle)
                    textY = dims.DataOffsetY + dims.DataHeight / 2 - textHeight / 2;
                else if (VAlign == VerticalAlignment.Lower)
                    textY = dims.DataOffsetY + dims.DataHeight - textHeight - Padding;

                float textX = 0;
                if (HAlign == HorizontalAlignment.Left)
                    textX = dims.DataOffsetX + Padding;
                else if (HAlign == HorizontalAlignment.Center)
                    textX = dims.DataOffsetX + dims.DataWidth / 2 - textWidth / 2;
                else if (HAlign == HorizontalAlignment.Right)
                    textX = dims.DataOffsetX + dims.DataWidth - textWidth - Padding;

                RectangleF textRect = new RectangleF(textX, textY, textWidth, textHeight);
                gfx.FillRectangle(fillBrush, textRect);
                gfx.DrawRectangle(borderPen, Rectangle.Round(textRect));
                gfx.DrawString(Text, font, fontBrush, textX, textY);
            }
        }
    }
}
