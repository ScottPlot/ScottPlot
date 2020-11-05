using ScottPlot.Config;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    public class AxisTitleSettings
    {
        public bool Enable = true;
        public string Label = null;
        public Drawing.Font Font = new Drawing.Font() { Size = 16 };
        public bool IsVisible => Enable && !string.IsNullOrWhiteSpace(Label);
    }

    // styles tick marks (major/minor), grid lines (major/minor), and tick labels (major)
    public class AxisTickSettings
    {
        public Color Color = Color.Black;

        public bool MajorLabelEnable = true;
        public Drawing.Font MajorLabelFont = new Drawing.Font() { Size = 11 };

        public bool MajorTickEnable = true;
        public float MajorTickLength = 5;

        public bool MinorTickEnable = true;
        public float MinorTickLength = 2;
        public bool MinorTickLogDistribution = false;

        public LineStyle MajorGridStyle = LineStyle.None;
        public Color MajorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MajorGridWidth = 1;

        public LineStyle MinorGridStyle = LineStyle.None;
        public Color MinorGridColor = ColorTranslator.FromHtml("#efefef");
        public float MinorGridWidth = 1;

        public bool RulerMode = false;
        public bool SnapPx = true;
    }

    public class AxisLineSettings
    {
        public bool Enable = true;
        public Color Color = Color.Black;
        public float Width = 1;
    }

    /// <summary>
    /// This class holds axis rendering details (label, ticks, tick labels) but no logic
    /// </summary>
    public class Axis : IRenderable
    {
        public bool IsVisible { get; set; } = true;

        public Edge Edge = Edge.Bottom;
        public bool IsHorizontal => Edge == Edge.Top || Edge == Edge.Bottom;
        public bool IsVertical => Edge == Edge.Left || Edge == Edge.Right;

        public float PixelSize = 40;
        public float PixelSizeMinimum = 5;

        public readonly TickCollection TickCollection = new TickCollection();
        public readonly AxisTitleSettings Title = new AxisTitleSettings();
        public readonly AxisTickSettings Ticks = new AxisTickSettings();
        public readonly AxisLineSettings Line = new AxisLineSettings();
        public Color Color
        {
            set
            {
                Title.Font.Color = value;
                Ticks.MajorLabelFont.Color = value;
                Ticks.Color = value;
                Line.Color = value;
            }
        }

        // TODO: support ruler mode
        // TODO: support offset and multiplier notation
        // TODO: support inverted sign

        public void AutoSize()
        {
            // adjust PixelSize based on measured dimensions of the axis label and ticks

            using (var tickFont = GDI.Font(Ticks.MajorLabelFont))
            using (var titleFont = GDI.Font(Title.Font))
            {
                (float tickWidth, float tickHeight) = TickCollection.tickLabels?.Length > 0 ?
                                                      LargestStringSize(TickCollection.tickLabels, tickFont) :
                                                      (0, 0);

                float titleHeight = Title.IsVisible ?
                                    GDI.MeasureString(Title.Label, titleFont).Height :
                                    0;

                PixelSize = IsHorizontal ?
                    tickHeight + titleHeight :
                    tickWidth + titleHeight + 5;

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
            TickCollection.Recalculate(dims);
            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var testFill = GDI.Brush(Color.LightGray))
            {
                var rect = new RectangleF(
                    x: dims.DataOffsetX,
                    y: dims.DataOffsetY + dims.DataHeight,
                    width: dims.DataWidth,
                    height: dims.Height - (dims.DataHeight + dims.DataOffsetY));

                if (Ticks.MajorTickEnable)
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMajor, Ticks.MajorTickLength, Ticks.Color, Ticks.MajorGridStyle, Ticks.MajorGridColor, Ticks.MajorGridWidth);

                if (Ticks.MinorTickEnable)
                    RenderTickMarks(dims, gfx, TickCollection.tickPositionsMinor, Ticks.MinorTickLength, Ticks.Color, Ticks.MinorGridStyle, Ticks.MinorGridColor, Ticks.MinorGridWidth);

                if (Ticks.MajorLabelEnable)
                    RenderTickLabels(dims, gfx);

                if (Line.Enable)
                    RenderLine(dims, gfx);

                if (Title.Enable)
                    RenderTitle(dims, gfx);
            }
        }

        private void RenderTickMarks(PlotDimensions dims, Graphics gfx, double[] positions,
            float tickLength, Color tickColor, LineStyle gridLineStyle, Color gridLineColor, float gridLineWidth)
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

                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float y in ys)
                            gfx.DrawLine(pen, x, y, x2, y);
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

                if (gridLineStyle != LineStyle.None)
                    using (var pen = GDI.Pen(gridLineColor, gridLineWidth, gridLineStyle))
                        foreach (float x in xs)
                            gfx.DrawLine(pen, x, y, x, y2);
            }
        }

        private void RenderTickLabels(PlotDimensions dims, Graphics gfx)
        {
            if (TickCollection.tickLabels is null || TickCollection.tickLabels.Length == 0)
                return;

            using (var font = GDI.Font(Ticks.MajorLabelFont))
            using (var brush = GDI.Brush(Ticks.Color))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Middle))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                            y: dims.DataOffsetY + dims.DataHeight + Ticks.MajorTickLength);
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.GetPixelX(TickCollection.tickPositionsMajor[i]),
                            y: dims.DataOffsetY - Ticks.MajorTickLength);
                }
                else if (Edge == Edge.Left)
                {
                    sf.Alignment = StringAlignment.Far;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX - Ticks.MajorTickLength,
                            y: dims.GetPixelY(TickCollection.tickPositionsMajor[i]));
                }
                else if (Edge == Edge.Right)
                {
                    sf.Alignment = StringAlignment.Near;
                    for (int i = 0; i < TickCollection.tickPositionsMajor.Length; i++)
                        gfx.DrawString(TickCollection.tickLabels[i], font, brush, format: sf,
                            x: dims.DataOffsetX + Ticks.MajorTickLength + dims.DataWidth,
                            y: dims.GetPixelY(TickCollection.tickPositionsMajor[i]));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void RenderLine(PlotDimensions dims, Graphics gfx)
        {
            if (!Line.Enable)
                return;

            using (var pen = GDI.Pen(Line.Color, Line.Width))
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
            if (string.IsNullOrWhiteSpace(Title.Label))
                return;

            float dataCenterX = dims.DataOffsetX + dims.DataWidth / 2;
            float dataCenterY = dims.DataOffsetY + dims.DataHeight / 2;

            using (var font = GDI.Font(Title.Font))
            using (var brush = GDI.Brush(Title.Font.Color))
            using (var sf = GDI.StringFormat(HorizontalAlignment.Center, VerticalAlignment.Lower))
            {
                if (Edge == Edge.Bottom)
                {
                    sf.LineAlignment = StringAlignment.Far;
                    gfx.DrawString(Title.Label, font, brush, dataCenterX, dims.Height, sf);
                }
                else if (Edge == Edge.Top)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.DrawString(Title.Label, font, brush, dataCenterX, 0, sf);
                }
                else if (Edge == Edge.Left)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(0, dataCenterY);
                    gfx.RotateTransform(-90);
                    gfx.DrawString(Title.Label, font, brush, 0, 0, sf);
                    gfx.ResetTransform();
                }
                else if (Edge == Edge.Right)
                {
                    sf.LineAlignment = StringAlignment.Near;
                    gfx.TranslateTransform(dims.Width, dataCenterY);
                    gfx.RotateTransform(90);
                    gfx.DrawString(Title.Label, font, brush, 0, 0, sf);
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
