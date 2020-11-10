using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        public bool IsVisible { get; set; } = true;

        public Edge Edge;
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;

        public Color Color = Color.Black;
        public float Rotation = 0;

        public bool MajorLabelEnable = true;
        public Drawing.Font MajorLabelFont = new Drawing.Font() { Size = 11 };

        public bool MajorTickEnable = true;
        public float MajorTickLength = 5;

        public bool MinorTickEnable = true;
        public float MinorTickLength = 2;

        public bool MajorGridEnable = false;
        public LineStyle MajorGridStyle = LineStyle.Solid;
        public Color MajorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MajorGridWidth = 1;

        public bool MinorGridEnable = false;
        public LineStyle MinorGridStyle = LineStyle.Solid;
        public Color MinorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MinorGridWidth = 1;

        public bool RulerMode = false;
        public bool SnapPx = true;

        public readonly TickCollection TickCollection = new TickCollection();

        public void Render(PlotDimensions2D dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            {
                if (MajorTickEnable)
                {
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMajor, RulerMode ? MajorTickLength * 4 : MajorTickLength, Color);
                    RenderTickLabels(dims, gfx);
                }

                if (MinorTickEnable)
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMinor, MinorTickLength, Color);

                if (MajorGridEnable)
                    RenderGridLines(dims, gfx, TickCollection.tickPositionsMajor, MajorGridStyle, MajorGridColor, MajorGridWidth);

                if (MinorGridEnable)
                    RenderGridLines(dims, gfx, TickCollection.tickPositionsMinor, MinorGridStyle, MinorGridColor, MinorGridWidth);
            }
        }

        private void RenderTickMarks(PlotDimensions2D dims, Graphics gfx, double[] positions, float tickLength, Color tickColor)
        {
            if (positions is null || positions.Length == 0)
                return;

            if (IsVertical)
            {
                float x = (Edge == Edge.Left) ? dims.DataOffsetX : dims.DataOffsetX + dims.DataWidth;
                float x2 = (Edge == Edge.Left) ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX;
                float tickDelta = (Edge == Edge.Left) ? -tickLength : tickLength;

                var ys = positions.Select(i => dims.GetPixelY(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float y in ys)
                        gfx.DrawLine(pen, x, y, x + tickDelta, y);
            }

            if (IsHorizontal)
            {
                float y = (Edge == Edge.Top) ? dims.DataOffsetY : dims.DataOffsetY + dims.DataHeight;
                float y2 = (Edge == Edge.Top) ? dims.DataOffsetY + dims.DataHeight : dims.DataOffsetY;
                float tickDelta = (Edge == Edge.Top) ? -tickLength : tickLength;

                var xs = positions.Select(i => dims.GetPixelX(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float x in xs)
                        gfx.DrawLine(pen, x, y, x, y + tickDelta);
            }
        }

        private void RenderGridLines(PlotDimensions2D dims, Graphics gfx, double[] positions,
            LineStyle gridLineStyle, Color gridLineColor, float gridLineWidth)
        {
            if (positions is null || positions.Length == 0 || gridLineStyle == LineStyle.None)
                return;

            if (IsVertical)
            {
                float x = (Edge == Edge.Left) ? dims.DataOffsetX : dims.DataOffsetX + dims.DataWidth;
                float x2 = (Edge == Edge.Left) ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX;
                var ys = positions.Select(i => dims.GetPixelY(i));
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float y in ys)
                            gfx.DrawLine(pen, x, y, x2, y);
            }

            if (IsHorizontal)
            {
                float y = (Edge == Edge.Top) ? dims.DataOffsetY : dims.DataOffsetY + dims.DataHeight;
                float y2 = (Edge == Edge.Top) ? dims.DataOffsetY + dims.DataHeight : dims.DataOffsetY;
                var xs = positions.Select(i => dims.GetPixelX(i));
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float x in xs)
                            gfx.DrawLine(pen, x, y, x, y2);
            }
        }

        private void RenderTickLabels(PlotDimensions2D dims, Graphics gfx)
        {
            if (TickCollection.tickLabels is null || TickCollection.tickLabels.Length == 0 || MajorLabelEnable == false)
                return;

            using (var font = GDI.Font(MajorLabelFont))
            using (var brush = GDI.Brush(Color))
            using (var sf = GDI.StringFormat())
            {
                if (Edge == Edge.Bottom)
                {
                    if (Rotation == 0)
                    {
                        sf.Alignment = RulerMode ? StringAlignment.Near : StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                            gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                                x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                                y: dims.DataOffsetY + dims.DataHeight + MajorTickLength);

                        sf.Alignment = StringAlignment.Far;
                        gfx.DrawString(TickCollection.cornerLabel, font, brush, format: sf,
                            x: dims.DataOffsetX + dims.DataWidth,
                            y: dims.DataOffsetY + dims.DataHeight + MajorTickLength + TickCollection.maxLabelHeight);
                    }
                    else
                    {
                        for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        {
                            float x = dims.GetPixelX(TickCollection.tickPositionsMajor[i]);
                            float y = dims.DataOffsetY + dims.DataHeight + MajorTickLength + 3;

                            gfx.TranslateTransform(x, y);
                            gfx.RotateTransform(-Rotation);
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Center;
                            gfx.DrawString(TickCollection.tickLabels[i], font, brush, 0, 0, sf);
                            gfx.ResetTransform();
                        }
                    }
                }
                else if (Edge == Edge.Top)
                {
                    sf.Alignment = RulerMode ? StringAlignment.Near : StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                            y: dims.DataOffsetY - MajorTickLength);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = RulerMode ? StringAlignment.Far : StringAlignment.Center;
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX - MajorTickLength,
                            y: dims.GetPixelY(TickCollection.tickPositionsMajor[i]));

                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Near;
                    gfx.DrawString(TickCollection.cornerLabel, font, brush, dims.DataOffsetX, dims.DataOffsetY, sf);
                }
                else if (Edge == Edge.Right)
                {
                    sf.LineAlignment = RulerMode ? StringAlignment.Far : StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX + MajorTickLength + dims.DataWidth,
                            y: dims.GetPixelY(TickCollection.tickPositionsMajor[i]));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
