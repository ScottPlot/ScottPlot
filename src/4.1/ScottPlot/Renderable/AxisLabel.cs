using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisLabelLeft : AxisLabel { public AxisLabelLeft() { Edge = Edge.Left; Size.Width = 20; } }
    public class AxisLabelRight : AxisLabel { public AxisLabelRight() { Edge = Edge.Right; Size.Width = 20; } }
    public class AxisLabelTop : AxisLabel { public AxisLabelTop() { Edge = Edge.Top; Size.Height = 28; FontSize = 14; } }
    public class AxisLabelBottom : AxisLabel { public AxisLabelBottom() { Edge = Edge.Bottom; Size.Height = 20; } }

    public class AxisLabel : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;
        public readonly Size Size = new Size();

        public Edge Edge;

        public string Text = "";
        public string FontName = "segoe ui";
        public float FontSize = 12;
        public Color FontColor = Colors.Black;

        public void Render(IRenderer renderer, PlotInfo info)
        {
            renderer.AntiAlias(AntiAlias);

            float centerX = info.DataOffsetX + info.DataWidth / 2;
            float centerY = info.DataOffsetY + info.DataHeight / 2;
            Font font = new Font(FontName, FontSize);

            if (Edge == Edge.Bottom)
            {
                Point pt = new Point(centerX, info.Height);
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Bottom;
                renderer.DrawText(pt, Text, FontColor, font);
            }
            else if (Edge == Edge.Top)
            {
                Point pt = new Point(centerX, 0);
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Top;
                renderer.DrawText(pt, Text, FontColor, font);
            }
            else if (Edge == Edge.Left)
            {
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Top;
                renderer.Rotate(-90, new Point(0, centerY));
                renderer.DrawText(new Point(0, 0), Text, FontColor, font);
                renderer.RotateReset();
            }
            else if (Edge == Edge.Right)
            {
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Top;
                renderer.Rotate(90, new Point(info.Width, centerY));
                renderer.DrawText(new Point(0, 0), Text, FontColor, font);
                renderer.RotateReset();
            }
        }
    }
}
