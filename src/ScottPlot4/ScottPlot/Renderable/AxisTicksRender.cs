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

            using var font = GDI.Font(tickFont);
            using var brush = GDI.Brush(tickFont.Color);
            using var sf = GDI.StringFormat();

            Tick[] visibleMajorTicks = tc.GetVisibleMajorTicks(dims);

            switch (edge)
            {
                case Edge.Bottom:
                    for (int i = 0; i < visibleMajorTicks.Length; i++)
                    {
                        float x = dims.GetPixelX(visibleMajorTicks[i].Position);
                        float y = dims.DataOffsetY + dims.DataHeight + MajorTickLength + PixelOffset;

                        gfx.TranslateTransform(x, y);
                        gfx.RotateTransform(-rotation);
                        sf.Alignment = rotation == 0 ? StringAlignment.Center : StringAlignment.Far;
                        if (rulerMode) sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = rotation == 0 ? StringAlignment.Near : StringAlignment.Center;
                        gfx.DrawString(visibleMajorTicks[i].Label, font, brush, 0, 0, sf);
                        GDI.ResetTransformPreservingScale(gfx, dims);
                    }
                    break;

                case Edge.Top:
                    for (int i = 0; i < visibleMajorTicks.Length; i++)
                    {
                        float x = dims.GetPixelX(visibleMajorTicks[i].Position);
                        float y = dims.DataOffsetY - MajorTickLength - PixelOffset;

                        gfx.TranslateTransform(x, y);
                        gfx.RotateTransform(-rotation);
                        sf.Alignment = rotation == 0 ? StringAlignment.Center : StringAlignment.Near;
                        if (rulerMode) sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = rotation == 0 ? StringAlignment.Far : StringAlignment.Center;
                        gfx.DrawString(visibleMajorTicks[i].Label, font, brush, 0, 0, sf);
                        GDI.ResetTransformPreservingScale(gfx, dims);
                    }
                    break;

                case Edge.Left:
                    for (int i = 0; i < visibleMajorTicks.Length; i++)
                    {
                        float x = dims.DataOffsetX - PixelOffset - MajorTickLength;
                        float y = dims.GetPixelY(visibleMajorTicks[i].Position);

                        gfx.TranslateTransform(x, y);
                        gfx.RotateTransform(-rotation);
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = rulerMode ? StringAlignment.Far : StringAlignment.Center;
                        if (rotation == 90)
                        {
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Far;
                        }
                        gfx.DrawString(visibleMajorTicks[i].Label, font, brush, 0, 0, sf);
                        GDI.ResetTransformPreservingScale(gfx, dims);
                    }
                    break;

                case Edge.Right:
                    for (int i = 0; i < visibleMajorTicks.Length; i++)
                    {
                        float x = dims.DataOffsetX + PixelOffset + MajorTickLength + dims.DataWidth;
                        float y = dims.GetPixelY(visibleMajorTicks[i].Position);

                        gfx.TranslateTransform(x, y);
                        gfx.RotateTransform(-rotation);
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = rulerMode ? StringAlignment.Far : StringAlignment.Center;
                        if (rotation == 90)
                        {
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Near;
                        }
                        gfx.DrawString(visibleMajorTicks[i].Label, font, brush, 0, 0, sf);
                        GDI.ResetTransformPreservingScale(gfx, dims);
                    }
                    break;

                default:
                    throw new NotImplementedException($"unsupported edge type {edge}");
            }

            if (!string.IsNullOrWhiteSpace(tc.CornerLabel))
            {
                switch (edge)
                {
                    case Edge.Left:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(s: "\n" + tc.CornerLabel,
                            x: dims.DataOffsetX - MajorTickLength - PixelOffset,
                            y: dims.DataOffsetY,
                            font: font, brush: brush, format: sf);
                        break;

                    case Edge.Bottom:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(s: "\n" + tc.CornerLabel,
                            x: dims.DataOffsetX + dims.DataWidth,
                            y: dims.DataOffsetY + dims.DataHeight + MajorTickLength + PixelOffset,
                            font: font, brush: brush, format: sf);
                        break;

                    case Edge.Right:
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(s: "\n" + tc.CornerLabel,
                            x: dims.DataOffsetX + dims.DataWidth + MajorTickLength + PixelOffset,
                            y: dims.DataOffsetY,
                            font: font, brush: brush, format: sf);
                        break;

                    case Edge.Top:
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(s: tc.CornerLabel + "\n\n",
                            x: dims.DataOffsetX + dims.DataWidth,
                            y: dims.DataOffsetY - MajorTickLength - PixelOffset,
                            font: font, brush: brush, format: sf);
                        break;

                    default:
                        throw new NotImplementedException($"unsupported edge type {edge}");
                }
            }
        }
    }
}
