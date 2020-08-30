using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderable
{
    public class AxisLabel : IRenderable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.BelowData;

        public Edge Edge;

        public string Label = "Axis Label";
        public string FontName = "consolas";
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
                renderer.DrawText(pt, Label, FontColor, font);
            }
            else if (Edge == Edge.Left)
            {
                font.HorizontalAlignment = HorizontalAlignment.Center;
                font.VerticalAlignment = VerticalAlignment.Top;
                renderer.Rotate(-90, new Point(0, centerY));
                renderer.DrawText(new Point(0, 0), Label, FontColor, font);
                renderer.RotateReset();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
