using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
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
        public float PixelSizeMinimum = 5;

        public string Title = null;
        public bool Bold { get => TitleFont.Bold; set => TitleFont.Bold = value; }
        public Drawing.Font TitleFont = new Drawing.Font() { Size = 16 };
        public Drawing.Font TickFont = new Drawing.Font() { Size = 11 };

        public Ticks MajorTicks = new Ticks() { MarkLength = 5 };
        public Ticks MinorTicks = new Ticks() { MarkLength = 2, IsGridVisible = false };
        public bool MajorGrid { get => MajorTicks.IsGridVisible; set => MajorTicks.IsGridVisible = value; }
        public bool MinorGrid { get => MinorTicks.IsGridVisible; set => MinorTicks.IsGridVisible = value; }

        public bool Line = true;
        public Color LineColor = Color.Black;
        public float LineWidth = 1;

        // TODO: support ruler mode
        // TODO: support offset and multiplier notation
        // TODO: support inverted sign

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
                var (width, height) = (MajorTicks?.Labels?.Length > 0) ? LargestStringSize(MajorTicks.Labels, tickFont) : (0, 0);
                var titleSize = (!string.IsNullOrWhiteSpace(Title)) ? GDI.MeasureString(Title, titleFont) : new SizeF(0, 0);

                PixelSize = IsHorizontal ?
                    height + titleSize.Height :
                    width + titleSize.Height + 5;

                PixelSize = Math.Max(PixelSize, PixelSizeMinimum);
            }
        }

        private (float width, float height) LargestStringSize(string[] strings, System.Drawing.Font font)
        {
            if (strings is null || strings.Length == 0)
                return (0, 0);

            string largestString = "";
            foreach (var s in strings.Where(x => string.IsNullOrWhiteSpace(x) == false))
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

        private void RenderTickMarks(PlotDimensions dims, Graphics gfx, Ticks tick, bool gridLines = false)
        {
            if (tick is null || tick.Positions is null)
                return;

            if (IsVertical)
            {
                float x = (Edge == Edge.Left) ? dims.DataOffsetX : dims.DataOffsetX + dims.DataWidth;
                float x2 = (Edge == Edge.Left) ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX;

                var ys = tick.Positions.Select(i => dims.GetPixelY(i));

                using (var pen = GDI.Pen(tick.MarkColor))
                    foreach (float y in ys)
                        gfx.DrawLine(pen, x, y, x - tick.MarkLength, y);

                if (tick.IsGridVisible)
                    using (var pen = GDI.Pen(tick.GridLineColor, tick.GridLineWidth, tick.GridLineStyle))
                        foreach (float y in ys)
                            gfx.DrawLine(pen, x, y, x2, y);
            }

            if (IsHorizontal)
            {
                float y = (Edge == Edge.Top) ? dims.DataOffsetY : dims.DataOffsetY + dims.DataHeight;
                float y2 = (Edge == Edge.Top) ? dims.DataOffsetY + dims.DataHeight : dims.DataOffsetY;

                var xs = tick.Positions.Select(i => dims.GetPixelX(i));

                using (var pen = GDI.Pen(tick.MarkColor))
                    foreach (float x in xs)
                        gfx.DrawLine(pen, x, y, x, y + tick.MarkLength);

                if (tick.IsGridVisible)
                    using (var pen = GDI.Pen(tick.GridLineColor, tick.GridLineWidth, tick.GridLineStyle))
                        foreach (float x in xs)
                            gfx.DrawLine(pen, x, y, x, y2);
            }
        }

        private void RenderTickLabels(PlotDimensions dims, Graphics gfx, Ticks tick)
        {
            if (tick is null || tick.Labels is null || tick.Labels.Length == 0)
                return;

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
            if (Line == false)
                return;

            using (var pen = GDI.Pen(LineColor, LineWidth))
            {
                PointF bottomLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY + dims.DataHeight);
                PointF topLeft = new PointF(dims.DataOffsetX, dims.DataOffsetY);
                PointF topRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY);
                PointF bottomRight = new PointF(dims.DataOffsetX + dims.DataWidth, dims.DataOffsetY + dims.DataHeight);

                if (Edge == Edge.Bottom)
                    gfx.DrawLine(pen, bottomLeft, bottomRight);
                else if (Edge == Edge.Left)
                    gfx.DrawLine(pen, bottomLeft, topLeft);
                else if (Edge == Edge.Right)
                    gfx.DrawLine(pen, bottomRight, topRight);
                else if (Edge == Edge.Top)
                    gfx.DrawLine(pen, topLeft, topRight);
                else
                    throw new NotImplementedException();
            }
        }

        private void RenderTitle(PlotDimensions dims, Graphics gfx)
        {
            if (string.IsNullOrWhiteSpace(Title))
                return;

            float dataCenterX = dims.DataOffsetX + dims.DataWidth / 2;
            float dataCenterY = dims.DataOffsetY + dims.DataHeight / 2;

            using (var font = GDI.Font(TitleFont.Name, TitleFont.Size, TitleFont.Bold))
            using (var brush = GDI.Brush(TitleFont.Color))
            using (var pen = GDI.Pen(MajorTicks.MarkColor))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Far;
                    gfx.DrawString(Title, font, brush, dataCenterX, dims.Height, sf);
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.DrawString(Title, font, brush, dataCenterX, 0, sf);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(0, dataCenterY);
                    gfx.RotateTransform(-90);
                    gfx.DrawString(Title, font, brush, 0, 0, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Right)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
