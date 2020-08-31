﻿using ScottPlot.Renderer;
using ScottPlot.Space;
using ScottPlot.Ticks;
using System;
using System.Diagnostics;

namespace ScottPlot.Renderable
{
    public class AxisTicksLeft : AxisTicks { public AxisTicksLeft() { Edge = Edge.Left; } }
    public class AxisTicksRight : AxisTicks { public AxisTicksRight() { Edge = Edge.Right; } }
    public class AxisTicksTop : AxisTicks { public AxisTicksTop() { Edge = Edge.Top; } }
    public class AxisTicksBottom : AxisTicks { public AxisTicksBottom() { Edge = Edge.Bottom; } }

    public class AxisTicks : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.AboveData;
        public int XAxisIndex { get; set; }
        public int YAxisIndex { get; set; }

        public Edge Edge;
        public float Offset = 0;
        public float LabelOffset = 40;

        public float EdgeWidth = 1;
        public Color EdgeColor = Colors.Black;

        public string Label = "Special Axis";
        public string LabelFontName = "segoe ui";
        public float LabelFontSize = 12;
        public Color LabelFontColor = Colors.Black;

        public string TickFontName = "segoe ui";
        public float TickFontSize = 9;
        public Color TickFontColor = Colors.Black;

        public Color MajorTickColor = Colors.Black;
        public float majorTickWidth = 1;
        public float majorTickLength = 5;

        public Color MinorTickColor = Colors.Black;
        public float MinorTickWidth = 1;
        public float MinorTickLength = 2;

        public bool MajorGrid = true;
        public Color MajorGridColor = new Color(35, 0, 0, 0);
        public float MajorGridWidth = 1;

        public bool MinorGrid = true;
        public Color MinorGridColor = new Color(10, 0, 0, 0);
        public float MinorGridWidth = 1;

        public ITickGenerator TickGenerator = new NumericTickGenerator();

        public void Recalculate(AxisLimits2D limits)
        {
            if (Edge == Edge.Bottom || Edge == Edge.Top)
                TickGenerator.Recalculate(limits.X1, limits.X2);
            if (Edge == Edge.Left || Edge == Edge.Right)
                TickGenerator.Recalculate(limits.Y1, limits.Y2);
        }

        public void Render(IRenderer renderer, Dimensions dims)
        {
            DrawGridLines(renderer, dims);
            DrawTickMarks(renderer, dims);
            DrawTickLabels(renderer, dims);
            DrawAxisLines(renderer, dims);
            DrawAxisLabels(renderer, dims);
        }

        private Font GetTickFont()
        {
            Font fnt = new Font(TickFontName, TickFontSize);
            if (Edge == Edge.Left)
            {
                fnt.HorizontalAlignment = HorizontalAlignment.Right;
                fnt.VerticalAlignment = VerticalAlignment.Center;
            }
            else if (Edge == Edge.Right)
            {
                fnt.HorizontalAlignment = HorizontalAlignment.Left;
                fnt.VerticalAlignment = VerticalAlignment.Center;
            }
            else if (Edge == Edge.Top)
            {
                fnt.HorizontalAlignment = HorizontalAlignment.Center;
                fnt.VerticalAlignment = VerticalAlignment.Bottom;
            }
            else if (Edge == Edge.Bottom)
            {
                fnt.HorizontalAlignment = HorizontalAlignment.Center;
                fnt.VerticalAlignment = VerticalAlignment.Top;
            }
            else
            {
                throw new NotImplementedException("unsupported edge");
            }
            return fnt;
        }

        public void DrawAxisLabels(IRenderer renderer, Dimensions info)
        {
            Font font = new Font(LabelFontName, LabelFontSize);

            if (Edge == Edge.Bottom)
            {
                Point pt = info.DataSC.Shift(0, Offset + LabelOffset);
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Top;
                renderer.DrawText(pt, Label, LabelFontColor, font);
            }
            else if (Edge == Edge.Top)
            {
                Point pt = info.DataNC.Shift(0, -(Offset + LabelOffset));
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Bottom;
                renderer.DrawText(pt, Label, LabelFontColor, font);
            }
            else if (Edge == Edge.Left)
            {
                Point pt = info.DataWC.Shift(-(Offset + LabelOffset), 0);
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Bottom;
                renderer.Rotate(-90, pt);
                renderer.DrawText(new Point(0, 0), Label, LabelFontColor, font);
                renderer.RotateReset();
            }
            else if (Edge == Edge.Right)
            {
                Point pt = info.DataEC.Shift(Offset + LabelOffset, 0);
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Bottom;
                renderer.Rotate(90, pt);
                renderer.DrawText(new Point(0, 0), Label, LabelFontColor, font);
                renderer.RotateReset();
            }
        }

        private void DrawGridLines(IRenderer renderer, Dimensions dims)
        {
            foreach (Tick tick in TickGenerator.Ticks)
            {
                Point pt1, pt2;

                if (Edge == Edge.Left)
                {
                    float tickY = dims.GetPixelY(tick.Position, YAxisIndex);
                    pt1 = new Point(dims.DataOffsetX, tickY);
                    pt2 = new Point(dims.DataOffsetX + dims.DataWidth, tickY);
                }
                else if (Edge == Edge.Right)
                {
                    float tickY = dims.GetPixelY(tick.Position, YAxisIndex);
                    pt1 = new Point(dims.DataOffsetX, tickY);
                    pt2 = new Point(dims.DataOffsetX + dims.DataWidth, tickY);
                }
                else if (Edge == Edge.Bottom)
                {
                    float tickX = dims.GetPixelX(tick.Position, XAxisIndex);
                    pt1 = new Point(tickX, dims.DataOffsetY);
                    pt2 = new Point(tickX, dims.DataOffsetY + dims.DataHeight);
                }
                else if (Edge == Edge.Top)
                {
                    float tickX = dims.GetPixelX(tick.Position, XAxisIndex);
                    pt1 = new Point(tickX, dims.DataOffsetY + dims.DataHeight);
                    pt2 = new Point(tickX, dims.DataOffsetY + dims.DataHeight);
                }
                else
                {
                    throw new NotImplementedException("unsupported edge");
                }

                if (tick.IsMajor && MajorGrid)
                    renderer.DrawLine(pt1, pt2, MajorGridColor, MajorGridWidth);
                else if (tick.IsMinor && MinorGrid)
                    renderer.DrawLine(pt1, pt2, MinorGridColor, MinorGridWidth);
            }
        }

        private void DrawTickMarks(IRenderer renderer, Dimensions dims)
        {
            float majorTickLength = 5;
            float minorTickLength = 2;

            // draw ticks, tick labels, and grid
            foreach (Tick tick in TickGenerator.Ticks)
            {
                float tickLength = tick.IsMajor ? majorTickLength : minorTickLength;

                Point tickPt1, tickPt2;

                if (Edge == Edge.Left)
                {
                    float tickY = dims.GetPixelY(tick.Position, YAxisIndex);
                    tickPt1 = new Point(dims.DataOffsetX - Offset, tickY);
                    tickPt2 = new Point(dims.DataOffsetX - Offset - tickLength, tickY);
                }
                else if (Edge == Edge.Right)
                {
                    float tickY = dims.GetPixelY(tick.Position, YAxisIndex);
                    tickPt1 = new Point(dims.DataOffsetX + Offset + dims.DataWidth, tickY);
                    tickPt2 = new Point(dims.DataOffsetX + Offset + dims.DataWidth + tickLength, tickY);
                }
                else if (Edge == Edge.Bottom)
                {
                    float tickX = dims.GetPixelX(tick.Position, XAxisIndex);
                    tickPt1 = new Point(tickX, dims.DataOffsetY + dims.DataHeight + Offset);
                    tickPt2 = new Point(tickX, dims.DataOffsetY + dims.DataHeight + Offset + tickLength);
                }
                else if (Edge == Edge.Top)
                {
                    float tickX = dims.GetPixelX(tick.Position, XAxisIndex);
                    tickPt1 = new Point(tickX, dims.DataOffsetY - Offset);
                    tickPt2 = new Point(tickX, dims.DataOffsetY - Offset - tickLength);
                }
                else
                {
                    throw new NotImplementedException("unsupported edge");
                }

                if (tick.IsMajor)
                    renderer.DrawLine(tickPt1, tickPt2, MajorTickColor, majorTickWidth);
                else
                    renderer.DrawLine(tickPt1, tickPt2, MinorTickColor, MinorTickWidth);
            }
        }

        private void DrawTickLabels(IRenderer renderer, Dimensions dims)
        {
            Font fnt = GetTickFont();

            foreach (Tick tick in TickGenerator.Ticks)
            {
                if (tick.IsMinor)
                    continue;

                Point pt;
                if (Edge == Edge.Left)
                    pt = new Point(dims.DataOffsetX - Offset - majorTickLength, dims.GetPixelY(tick.Position, YAxisIndex));
                else if (Edge == Edge.Right)
                    pt = new Point(dims.DataOffsetX + Offset + dims.DataWidth + majorTickLength, dims.GetPixelY(tick.Position, YAxisIndex));
                else if (Edge == Edge.Bottom)
                    pt = new Point(dims.GetPixelX(tick.Position, XAxisIndex), dims.DataOffsetY + dims.DataHeight + Offset + majorTickLength);
                else if (Edge == Edge.Top)
                    pt = new Point(dims.GetPixelX(tick.Position, XAxisIndex), dims.DataOffsetY - Offset - majorTickLength);
                else
                    throw new NotImplementedException();

                renderer.DrawText(pt, tick.Label, TickFontColor, fnt);
            }
        }

        private void DrawAxisLines(IRenderer renderer, Dimensions dims)
        {
            if (Edge == Edge.Left)
            {
                Point edgePt1 = new Point(dims.DataW - Offset, dims.DataN);
                Point edgePt2 = new Point(dims.DataW - Offset, dims.DataS);
                renderer.DrawLine(edgePt1, edgePt2, EdgeColor, EdgeWidth);
            }
            else if (Edge == Edge.Right)
            {
                Point edgePt1 = new Point(dims.DataE + Offset, dims.DataN);
                Point edgePt2 = new Point(dims.DataE + Offset, dims.DataS);
                renderer.DrawLine(edgePt1, edgePt2, EdgeColor, EdgeWidth);
            }
            else if (Edge == Edge.Top)
            {
                Point edgePt1 = new Point(dims.DataW, dims.DataN - Offset);
                Point edgePt2 = new Point(dims.DataE, dims.DataN - Offset);
                renderer.DrawLine(edgePt1, edgePt2, EdgeColor, EdgeWidth);
            }
            else if (Edge == Edge.Bottom)
            {
                Point edgePt1 = new Point(dims.DataW, dims.DataS + Offset);
                Point edgePt2 = new Point(dims.DataE, dims.DataS + Offset);
                renderer.DrawLine(edgePt1, edgePt2, EdgeColor, EdgeWidth);
            }
            else
            {
                throw new NotImplementedException("unsupported edge");
            }
        }
    }
}
