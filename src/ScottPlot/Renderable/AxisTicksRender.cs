using ScottPlot.Drawing;
using ScottPlot.Ticks;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    static class AxisTicksRender
    {
        private static bool EdgeIsVertical(Edge edge) => (edge == Edge.Left || edge == Edge.Right);

        private static bool EdgeIsHorizontal(Edge edge) => (edge == Edge.Top || edge == Edge.Bottom);

        public static void RenderGridLines(PlotDimensions dims, Graphics gfx, double[] positions,
            LineStyle gridLineStyle, Color gridLineColor, float gridLineWidth, Edge edge)
        {
            if (positions is null || positions.Length == 0 || gridLineStyle == LineStyle.None)
                return;

            // don't draw grid lines on the last pixel to prevent drawing over the data frame
            float xEdgeLeft = dims.DataOffsetX + 1;
            float xEdgeRight = dims.DataOffsetX + dims.DataWidth - 1;
            float yEdgeTop = dims.DataOffsetY + 1;
            float yEdgeBottom = dims.DataOffsetY + dims.DataHeight - 1;

            if (EdgeIsVertical(edge))
            {
                float x = (edge == Edge.Left) ? dims.DataOffsetX : dims.DataOffsetX + dims.DataWidth;
                float x2 = (edge == Edge.Left) ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX;
                var ys = positions.Select(i => dims.GetPixelY(i)).Where(y => yEdgeTop < y && y < yEdgeBottom);
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float y in ys)
                            gfx.DrawLine(pen, x, y, x2, y);
            }

            if (EdgeIsHorizontal(edge))
            {
                float y = (edge == Edge.Top) ? dims.DataOffsetY : dims.DataOffsetY + dims.DataHeight;
                float y2 = (edge == Edge.Top) ? dims.DataOffsetY + dims.DataHeight : dims.DataOffsetY;
                var xs = positions.Select(i => dims.GetPixelX(i)).Where(x => xEdgeLeft < x && x < xEdgeRight);
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float x in xs)
                            gfx.DrawLine(pen, x, y, x, y2);
            }
        }

        public static void RenderTickMarks(PlotDimensions dims, Graphics gfx, double[] positions, float tickLength, Color tickColor, Edge edge, float pixelOffset)
        {
            if (positions is null || positions.Length == 0)
                return;

            if (EdgeIsVertical(edge))
            {
                float x = (edge == Edge.Left) ? dims.DataOffsetX - pixelOffset : dims.DataOffsetX + dims.DataWidth + pixelOffset;
                float tickDelta = (edge == Edge.Left) ? -tickLength : tickLength;

                var ys = positions.Select(i => dims.GetPixelY(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float y in ys)
                        gfx.DrawLine(pen, x, y, x + tickDelta, y);
            }

            if (EdgeIsHorizontal(edge))
            {
                float y = (edge == Edge.Top) ? dims.DataOffsetY - pixelOffset : dims.DataOffsetY + dims.DataHeight + pixelOffset;
                float tickDelta = (edge == Edge.Top) ? -tickLength : tickLength;

                var xs = positions.Select(i => dims.GetPixelX(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float x in xs)
                        gfx.DrawLine(pen, x, y, x, y + tickDelta);
            }
        }

        public static void RenderTickLabels(PlotDimensions dims, Graphics gfx, TickCollection tc, Drawing.Font tickFont, Edge edge, float rotation, bool rulerMode, float PixelOffset, float MajorTickLength, float MinorTickLength)
        {
            if (tc.tickLabels is null || tc.tickLabels.Length == 0)
                return;

            using (var font = GDI.Font(tickFont))
            using (var brush = GDI.Brush(tickFont.Color))
            using (var sf = GDI.StringFormat())
            {
                if (edge == Edge.Bottom)
                {
                    if (rotation == 0)
                    {
                        sf.Alignment = rulerMode ? StringAlignment.Near : StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        for (int i = 0; i < tc.tickPositionsMajor.Length; i++)
                            gfx.DrawString(tc.tickLabels[i], font, brush, format: sf,
                                x: dims.GetPixelX(tc.tickPositionsMajor[i]),
                                y: dims.DataOffsetY + dims.DataHeight + PixelOffset + MajorTickLength);

                        sf.Alignment = StringAlignment.Far;
                        gfx.DrawString(tc.cornerLabel, font, brush, format: sf,
                            x: dims.DataOffsetX + dims.DataWidth,
                            y: dims.DataOffsetY + dims.DataHeight + MajorTickLength + tc.maxLabelHeight);
                    }
                    else
                    {
                        for (int i = 0; i < tc.tickPositionsMajor.Length; i++)
                        {
                            float x = dims.GetPixelX(tc.tickPositionsMajor[i]);
                            float y = dims.DataOffsetY + dims.DataHeight + MajorTickLength + 3;

                            gfx.TranslateTransform(x, y);
                            gfx.RotateTransform(-rotation);
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Center;
                            gfx.DrawString(tc.tickLabels[i], font, brush, 0, 0, sf);
                            gfx.ResetTransform();
                        }
                    }
                }
                else if (edge == Edge.Top)
                {
                    sf.Alignment = rulerMode ? StringAlignment.Near : StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    for (int i = 0; i < tc.tickPositionsMajor.Length; i++)
                        gfx.DrawString(tc.tickLabels[i], font, brush, format: sf,
                            x: dims.GetPixelX(tc.tickPositionsMajor[i]),
                            y: dims.DataOffsetY - PixelOffset - MajorTickLength);
                }
                else if (edge == Edge.Left)
                {
                    sf.LineAlignment = rulerMode ? StringAlignment.Far : StringAlignment.Center;
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 0; i < tc.tickPositionsMajor.Length; i++)
                        gfx.DrawString(tc.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX - PixelOffset - MajorTickLength,
                            y: dims.GetPixelY(tc.tickPositionsMajor[i]));

                    sf.LineAlignment = StringAlignment.Far;
                    sf.Alignment = StringAlignment.Near;
                    gfx.DrawString(tc.cornerLabel, font, brush, dims.DataOffsetX, dims.DataOffsetY, sf);
                }
                else if (edge == Edge.Right)
                {
                    sf.LineAlignment = rulerMode ? StringAlignment.Far : StringAlignment.Center;
                    sf.Alignment = StringAlignment.Near;
                    for (int i = 0; i < tc.tickPositionsMajor.Length; i++)
                        gfx.DrawString(tc.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX + PixelOffset + MajorTickLength + dims.DataWidth,
                            y: dims.GetPixelY(tc.tickPositionsMajor[i]));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
