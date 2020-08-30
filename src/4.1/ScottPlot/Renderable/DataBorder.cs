using ScottPlot.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderable
{
    public class DataBorder : IRenderable
    {
        public PlotLayer Layer => PlotLayer.AboveData;

        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = false;

        public Color Color = Colors.Black;
        public float LineWidth = 1;

        public bool Left = true;
        public bool Right = true;
        public bool Top = true;
        public bool Bottom = true;

        public void Render(IRenderer renderer, PlotInfo info)
        {
            if (Visible == false)
                return;

            Point NW = new Point(info.DataOffsetX, info.DataOffsetY);
            Point NE = new Point(info.DataOffsetX - 1 + info.DataWidth, info.DataOffsetY);
            Point SW = new Point(info.DataOffsetX, info.DataOffsetY + info.DataHeight - 1);
            Point SE = new Point(info.DataOffsetX - 1 + info.DataWidth, info.DataOffsetY + info.DataHeight - 1);

            if (Left)
                renderer.DrawLine(SW, NW, Color, LineWidth);
            if (Right)
                renderer.DrawLine(SE, NE, Color, LineWidth);
            if (Top)
                renderer.DrawLine(NW, NE, Color, LineWidth);
            if (Bottom)
                renderer.DrawLine(SW, SE, Color, LineWidth);
        }
    }
}
