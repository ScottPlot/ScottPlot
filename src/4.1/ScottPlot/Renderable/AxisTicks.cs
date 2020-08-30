using ScottPlot.Renderer;
using ScottPlot.Space;
using ScottPlot.Ticks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        public PlotLayer Layer => PlotLayer.BelowData;
        public readonly Size Size = new Size();

        public Edge Edge;

        public string FontName = "segoe ui";
        public float FontSize = 9;
        public Color FontColor = Colors.Black;

        public Color MajorColor = Colors.Black;
        public float MajorWidth = 1;
        public float MajorLength = 5;

        public Color MinorColor = Colors.Black;
        public float MinorWidth = 1;
        public float MinorLength = 2;

        public bool MajorGrid = true;
        public Color MajorGridColor = new Color(35, 0, 0, 0);
        public float MajorGridWidth = 1;

        public bool MinorGrid = true;
        public Color MinorGridColor = new Color(10, 0, 0, 0);
        public float MinorGridWidth = 1;

        public ITickGenerator TickGenerator = new NumericTickGenerator();

        public void Recalculate(AxisLimits limits)
        {
            if (Edge == Edge.Bottom || Edge == Edge.Top)
                TickGenerator.Recalculate(limits.X1, limits.X2);
            if (Edge == Edge.Left || Edge == Edge.Right)
                TickGenerator.Recalculate(limits.Y1, limits.Y2);
        }

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            float majorTickLength = 5;
            float minorTickLength = 2;

            foreach (Tick tick in TickGenerator.Ticks)
            {
                float tickLength = tick.IsMajor ? majorTickLength : minorTickLength;

                Font fnt = new Font(FontName, FontSize);
                Point ptTickDataEdge, ptTickExtended, ptOppositeData;

                if (Edge == Edge.Left)
                {
                    fnt.HorizontalAlignment = HorizontalAlignment.Right;
                    fnt.VerticalAlignment = VerticalAlignment.Center;
                    float tickY = info.GetPixelY(tick.Position);
                    ptTickDataEdge = new Point(info.DataOffsetX, tickY);
                    ptTickExtended = new Point(info.DataOffsetX - tickLength, tickY);
                    ptOppositeData = new Point(info.DataOffsetX + info.DataWidth, tickY);
                }
                else if (Edge == Edge.Right)
                {
                    fnt.HorizontalAlignment = HorizontalAlignment.Left;
                    fnt.VerticalAlignment = VerticalAlignment.Center;
                    float tickY = info.GetPixelY(tick.Position);
                    ptTickDataEdge = new Point(info.DataOffsetX + info.DataWidth, tickY);
                    ptTickExtended = new Point(info.DataOffsetX + info.DataWidth + tickLength, tickY);
                    ptOppositeData = new Point(info.DataOffsetX, tickY);
                }
                else if (Edge == Edge.Bottom)
                {
                    fnt.HorizontalAlignment = HorizontalAlignment.Center;
                    fnt.VerticalAlignment = VerticalAlignment.Top;
                    float tickX = info.GetPixelX(tick.Position);
                    ptTickDataEdge = new Point(tickX, info.DataOffsetY + info.DataHeight);
                    ptTickExtended = new Point(tickX, info.DataOffsetY + info.DataHeight + tickLength);
                    ptOppositeData = new Point(tickX, info.DataOffsetY);
                }
                else if (Edge == Edge.Top)
                {
                    fnt.HorizontalAlignment = HorizontalAlignment.Center;
                    fnt.VerticalAlignment = VerticalAlignment.Bottom;
                    float tickX = info.GetPixelX(tick.Position);
                    ptTickDataEdge = new Point(tickX, info.DataOffsetY);
                    ptTickExtended = new Point(tickX, info.DataOffsetY - tickLength);
                    ptOppositeData = new Point(tickX, info.DataOffsetY + info.DataHeight);
                }
                else
                {
                    throw new NotImplementedException("unsupported edge");
                }

                if (tick.IsMajor)
                {
                    renderer.DrawText(ptTickExtended, tick.Label, FontColor, fnt);
                    renderer.DrawLine(ptTickDataEdge, ptTickExtended, MajorColor, MajorWidth);
                    if (MajorGrid)
                        renderer.DrawLine(ptTickDataEdge, ptOppositeData, MajorGridColor, MajorGridWidth);
                }
                else
                {
                    renderer.DrawLine(ptTickDataEdge, ptTickExtended, MinorColor, MinorWidth);
                    if (MinorGrid)
                        renderer.DrawLine(ptTickDataEdge, ptOppositeData, MinorGridColor, MinorGridWidth);
                }
            }
        }
    }
}
