using ScottPlot.Experimental;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class AxisLabel : IRenderable
    {
        public Edge Edge;
        public int Padding = 5;
        public string Text;

        public void Render(Bitmap bmp, FigureInfo fig)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Font font = new Font(FontFamily.GenericSansSerif, 12))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                switch (Edge)
                {
                    // TODO: measure string and adjust properly
                    case Edge.Left:
                        gfx.DrawString(Text, font, brush, Padding, fig.Height / 2);
                        break;
                    case Edge.Right:
                        gfx.DrawString(Text, font, brush, fig.Width - Padding, fig.Height / 2);
                        break;
                    case Edge.Top:
                        gfx.DrawString(Text, font, brush, fig.Width / 2, Padding);
                        break;
                    case Edge.Bottom:
                        gfx.DrawString(Text, font, brush, fig.Width / 2, fig.Height - Padding - 20);
                        break;
                }
            }
        }
    }
}
