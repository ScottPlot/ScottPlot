using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        public Edge Edge { get; set; } = Edge.Bottom;
        public bool IsHorizontal { get => Edge == Edge.Top || Edge == Edge.Bottom; }
        public bool IsVertical { get => Edge == Edge.Left || Edge == Edge.Right; }
        public bool IsVisible { get; set; } = true;
        public float PixelSize = 40;

        public string Title = null;
        public Drawing.Font TitleFont = new Drawing.Font() { Size = 16 };
        public Drawing.Font TickFont = new Drawing.Font() { Size = 11 };

        public Ticks MajorTicks = new Ticks() { MarkLength = 5, GridLines = true };
        public Ticks MinorTicks = new Ticks() { MarkLength = 2, GridLines = false };

        public void SetTicks(double[] positions, string[] labels, double[] minorPositions)
        {
            MajorTicks.Positions = positions;
            MajorTicks.Labels = labels;
            MinorTicks.Positions = minorPositions;
        }

        public void AutoSize()
        {
            // adjust PixelSize based on measured dimensions of the axis label and ticks

            using (var tickFont = GDI.Font(MajorTicks.LabelFont))
            using (var titleFont = GDI.Font(TitleFont))
            {
                var tickSize = LargestStringSize(MajorTicks.Labels, tickFont);
                var titleSize = GDI.MeasureString(Title, titleFont);
                if (IsHorizontal)
                {
                    PixelSize = tickSize.height + titleSize.Height;
                }
                else
                {
                    PixelSize = tickSize.width + titleSize.Height + 5;
                }
            }
        }

        private (float width, float height) LargestStringSize(string[] strings, System.Drawing.Font font)
        {
            if (strings is null || strings.Length == 0)
                return (0, 0);

            string largestString = "";
            foreach (var s in strings)
                if (s.Length > largestString.Length)
                    largestString = s;

            SizeF sz = GDI.MeasureString(largestString, font);
            return (sz.Width, sz.Height);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var testFill = GDI.Brush(Color.LightGray))
            {
                var rect = new RectangleF(
                    x: dims.DataOffsetX,
                    y: dims.DataOffsetY + dims.DataHeight,
                    width: dims.DataWidth,
                    height: dims.Height - (dims.DataHeight + dims.DataOffsetY));

                RenderTickMarks(dims, gfx, MajorTicks);
                RenderTickMarks(dims, gfx, MinorTicks);
                RenderTickLabels(dims, gfx, MajorTicks);
                RenderLine(dims, gfx);
                RenderTitle(dims, gfx);
            }
        }

        private void RenderTickMarks(PlotDimensions dims, Graphics gfx, Ticks tick)
        {
            using (var pen = GDI.Pen(tick.MarkColor))
            {
                if (Edge == Edge.Bottom)
                {
                    float y = dims.DataOffsetY + dims.DataHeight;
                    foreach (float x in tick.Positions.Select(x => dims.GetPixelX(x)))
                        gfx.DrawLine(pen, x, y, x, y + tick.MarkLength);
                }
                else if (Edge == Edge.Left)
                {
                    float x = dims.DataOffsetX;
                    foreach (float y in tick.Positions.Select(y => dims.GetPixelY(y)))
                        gfx.DrawLine(pen, x, y, x - tick.MarkLength, y);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RenderTickLabels(PlotDimensions dims, Graphics gfx, Ticks tick)
        {
            using (var font = GDI.Font(TickFont.Name, TickFont.Size, TickFont.Bold))
            using (var brush = GDI.Brush(TickFont.Color))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Middle))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    for (int i = 0; i < tick.Positions.Length; i++)
                        gfx.DrawString(tick.Labels[i], font, brush, format: sf,
                            x: dims.GetPixelX(tick.Positions[i]),
                            y: dims.DataOffsetY + dims.DataHeight + tick.MarkLength);
                }
                else if (Edge == Edge.Left)
                {
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 0; i < tick.Positions.Length; i++)
                        gfx.DrawString(tick.Labels[i], font, brush, format: sf,
                            x: dims.DataOffsetX - tick.MarkLength,
                            y: dims.GetPixelY(tick.Positions[i]));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RenderLine(PlotDimensions dims, Graphics gfx)
        {
            using (var pen = GDI.Pen(MajorTicks.MarkColor))
            {
                PointF bottomLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY + dims.DataHeight);
                PointF topLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY);
                PointF topRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY);
                PointF bottomRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY + dims.DataHeight);

                if (Edge == Edge.Bottom)
                {
                    gfx.DrawLine(pen, bottomLeft, bottomRight);
                }
                else if (Edge == Edge.Left)
                {
                    gfx.DrawLine(pen, bottomLeft, topLeft);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RenderTitle(PlotDimensions dims, Graphics gfx)
        {
            PointF bottom = new PointF(dims.DataOffsetX + dims.DataWidth / 2, dims.DataOffsetY + dims.DataHeight);
            PointF left = new PointF(dims.DataOffsetX, dims.DataOffsetY + dims.DataHeight / 2);

            using (var font = GDI.Font(TitleFont.Name, TitleFont.Size, TitleFont.Bold))
            using (var brush = GDI.Brush(TitleFont.Color))
            using (var pen = GDI.Pen(MajorTicks.MarkColor))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    float padding = MajorTicks.MarkLength + MajorTicks.LabelFont.Size;
                    gfx.DrawString(Title, font, brush, bottom.X, bottom.Y + padding, sf);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(0, left.Y);
                    gfx.RotateTransform(-90);
                    gfx.DrawString(Title, font, brush, 0, 0, sf);
                    gfx.ResetTransform();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
