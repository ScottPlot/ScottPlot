using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot.Plottable
{
    public class Colorbar : IPlottable
    {
        public Renderable.Edge Edge = Renderable.Edge.Right;

        private Colormap Colormap;
        private Bitmap BmpScale;
        private readonly List<string> TickLabels = new List<string>();
        private readonly List<double> TickFractions = new List<double>();

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get => 0; set { } }
        public int YAxisIndex { get => 0; set { } }
        public int Width = 20;

        public Colorbar(Colormap colormap = null)
        {
            UpdateColormap(colormap ?? Colormap.Viridis);
        }

        public void ClearTicks()
        {
            TickFractions.Clear();
            TickLabels.Clear();
        }

        public void AddTick(double fraction, string label)
        {
            TickFractions.Add(fraction);
            TickLabels.Add(label);
        }

        public void AddTicks(double[] fractions, string[] labels)
        {
            if (fractions.Length != labels.Length)
                throw new ArgumentException("fractions and labels must have the same length");

            for (int i = 0; i < fractions.Length; i++)
            {
                TickFractions.Add(fractions[i]);
                TickLabels.Add(labels[i]);
            }
        }

        public void SetTicks(double[] fractions, string[] labels)
        {
            ClearTicks();
            AddTicks(fractions, labels);
        }

        public LegendItem[] GetLegendItems() => null;

        public AxisLimits GetAxisLimits() => new AxisLimits();

        public void ValidateData(bool deep = false)
        {
            if (TickLabels.Count != TickFractions.Count)
                throw new InvalidOperationException("Tick labels and positions must have the same length");
        }

        public void UpdateColormap(Colormap newColormap)
        {
            Colormap = newColormap;
            UpdateBitmap();
        }

        private void UpdateBitmap()
        {
            BmpScale?.Dispose();
            BmpScale = GetBitmap();
        }

        public Bitmap GetBitmap() =>
            Colormap.Colorbar(Colormap, Width, 256, true);

        public Bitmap GetBitmap(int width, int height, bool vertical = true) =>
            Colormap.Colorbar(Colormap, width, height, vertical);

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (BmpScale is null)
                UpdateBitmap();

            RectangleF colorbarRect = RenderColorbar(dims, bmp);
            RenderTicks(dims, bmp, lowQuality, colorbarRect);
        }

        private RectangleF RenderColorbar(PlotDimensions dims, Bitmap bmp)
        {
            float scaleLeftPad = 10;

            PointF location = new PointF(dims.DataOffsetX + dims.DataWidth + scaleLeftPad, dims.DataOffsetY);
            SizeF size = new SizeF(Width, dims.DataHeight);
            RectangleF rect = new RectangleF(location, size);

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality: true, clipToDataArea: false))
            using (var pen = GDI.Pen(Color.Black))
            {
                gfx.DrawImage(BmpScale, location.X, location.Y, size.Width, size.Height + 1);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }

            return rect;
        }

        private void RenderTicks(PlotDimensions dims, Bitmap bmp, bool lowQuality, RectangleF colorbarRect)
        {
            float tickLengh = 4;
            float tickLabelPadding = 2;

            float tickLeftPx = colorbarRect.Right;
            float tickRightPx = tickLeftPx + tickLengh;
            float tickLabelPx = tickRightPx + tickLabelPadding;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, false))
            using (var pen = GDI.Pen(Color.Black))
            using (var brush = GDI.Brush(Color.Black))
            using (var font = GDI.Font(null, 12))
            using (var sf = new StringFormat() { LineAlignment = StringAlignment.Center })
            {
                for (int i = 0; i < TickLabels.Count; i++)
                {
                    float y = colorbarRect.Top + (float)((1 - TickFractions[i]) * colorbarRect.Height);
                    gfx.DrawLine(pen, tickLeftPx, y, tickRightPx, y);
                    gfx.DrawString(TickLabels[i], font, brush, tickLabelPx, y, sf);
                }
            }
        }
    }
}
