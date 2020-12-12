/* This module is responsible for calculating, storing, and rendering:
 *   - tick marks (major and minor)
 *   - tick labels
 *   - grid lines (major and minor)
 * 
 */
using ScottPlot.Ticks;
using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    public class AxisTicks : IRenderable
    {
        // the tick collection determines where ticks should go and what tick labels should say
        public readonly TickCollection TickCollection = new TickCollection();

        // tick label styling
        public bool TickLabelVisible = true;
        public float TickLabelRotation = 0;
        public Drawing.Font TickLabelFont = new Drawing.Font() { Size = 11 };

        // major tick/grid styling
        public bool MajorTickVisible = true;
        public float MajorTickLength = 5;
        public Color MajorTickColor = Color.Black;
        public bool MajorGridVisible = false;
        public LineStyle MajorGridStyle = LineStyle.Solid;
        public Color MajorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MajorGridWidth = 1;

        // minor tick/grid styling
        public bool MinorTickVisible = true;
        public float MinorTickLength = 2;
        public Color MinorTickColor = Color.Black;
        public bool MinorGridVisible = false;
        public LineStyle MinorGridStyle = LineStyle.Solid;
        public Color MinorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MinorGridWidth = 1;

        // misc configuration
        public Edge Edge;
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;
        public bool RulerMode = false;
        public bool SnapPx = true;
        public float PixelOffset = 0;
        public bool IsVisible { get; set; } = true;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            {
                if (MajorTickVisible)
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMajor, RulerMode ? MajorTickLength * 4 : MajorTickLength, MajorTickColor);

                if (MajorTickVisible && TickLabelVisible)
                    RenderTickLabels(dims, gfx);

                if (MinorTickVisible)
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMinor, MinorTickLength, MinorTickColor);

                if (MajorGridVisible)
                    RenderGridLines(dims, gfx, TickCollection.tickPositionsMajor, MajorGridStyle, MajorGridColor, MajorGridWidth);

                if (MinorGridVisible)
                    RenderGridLines(dims, gfx, TickCollection.tickPositionsMinor, MinorGridStyle, MinorGridColor, MinorGridWidth);
            }
        }

        private void RenderTickMarks(PlotDimensions dims, Graphics gfx, double[] positions, float tickLength, Color tickColor)
        {
            if (positions is null || positions.Length == 0)
                return;

            if (IsVertical)
            {
                float x = (Edge == Edge.Left) ? dims.DataOffsetX - PixelOffset : dims.DataOffsetX + dims.DataWidth + PixelOffset;
                float tickDelta = (Edge == Edge.Left) ? -tickLength : tickLength;

                var ys = positions.Select(i => dims.GetPixelY(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float y in ys)
                        gfx.DrawLine(pen, x, y, x + tickDelta, y);
            }

            if (IsHorizontal)
            {
                float y = (Edge == Edge.Top) ? dims.DataOffsetY - PixelOffset : dims.DataOffsetY + dims.DataHeight + PixelOffset;
                float tickDelta = (Edge == Edge.Top) ? -tickLength : tickLength;

                var xs = positions.Select(i => dims.GetPixelX(i));
                using (var pen = GDI.Pen(tickColor))
                    foreach (float x in xs)
                        gfx.DrawLine(pen, x, y, x, y + tickDelta);
            }
        }

        private void RenderGridLines(PlotDimensions dims, Graphics gfx, double[] positions,
            LineStyle gridLineStyle, Color gridLineColor, float gridLineWidth)
        {
            if (positions is null || positions.Length == 0 || gridLineStyle == LineStyle.None)
                return;

            // don't draw grid lines on the last pixel to prevent drawing over the data frame
            float xEdgeLeft = dims.DataOffsetX + 1;
            float xEdgeRight = dims.DataOffsetX + dims.DataWidth - 1;
            float yEdgeTop = dims.DataOffsetY + 1;
            float yEdgeBottom = dims.DataOffsetY + dims.DataHeight - 1;

            if (IsVertical)
            {
                float x = (Edge == Edge.Left) ? dims.DataOffsetX : dims.DataOffsetX + dims.DataWidth;
                float x2 = (Edge == Edge.Left) ? dims.DataOffsetX + dims.DataWidth : dims.DataOffsetX;
                var ys = positions.Select(i => dims.GetPixelY(i)).Where(y => yEdgeTop < y && y < yEdgeBottom);
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float y in ys)
                            gfx.DrawLine(pen, x, y, x2, y);
            }

            if (IsHorizontal)
            {
                float y = (Edge == Edge.Top) ? dims.DataOffsetY : dims.DataOffsetY + dims.DataHeight;
                float y2 = (Edge == Edge.Top) ? dims.DataOffsetY + dims.DataHeight : dims.DataOffsetY;
                var xs = positions.Select(i => dims.GetPixelX(i)).Where(x => xEdgeLeft < x && x < xEdgeRight);
                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float x in xs)
                            gfx.DrawLine(pen, x, y, x, y2);
            }
        }

        private void RenderTickLabels(PlotDimensions dims, Graphics gfx)
        {
            if (TickCollection.tickLabels is null || TickCollection.tickLabels.Length == 0 || TickLabelVisible == false)
                return;

            using (var font = GDI.Font(TickLabelFont))
            using (var brush = GDI.Brush(TickLabelFont.Color))
            using (var sf = GDI.StringFormat())
            {
                if (Edge == Edge.Bottom)
                {
                    if (TickLabelRotation == 0)
                    {
                        sf.Alignment = RulerMode ? StringAlignment.Near : StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                            gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                                x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                                y: dims.DataOffsetY + dims.DataHeight + PixelOffset + MajorTickLength);

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
                            gfx.RotateTransform(-TickLabelRotation);
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Center;
                            gfx.DrawString(TickCollection.tickLabels[i], font, brush, 0, 0, sf);
                            gfx.ResetTransform();
                        }
                    }
                }
                else if (Edge == Edge.Top)
                {
                    sf.Alignment = RulerMode ? StringAlignment.Near : StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                            y: dims.DataOffsetY - PixelOffset - MajorTickLength);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = RulerMode ? StringAlignment.Far : StringAlignment.Center;
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX - PixelOffset - MajorTickLength,
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
                            x: dims.DataOffsetX + PixelOffset + MajorTickLength + dims.DataWidth,
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
