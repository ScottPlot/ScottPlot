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
            using (StringFormat sf = new StringFormat())
            {
                switch (Edge)
                {
                    case Edge.Left:
                        gfx.TranslateTransform(Padding, fig.Height / 2);
                        gfx.RotateTransform(-90);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(Text, font, brush, 0, 0, sf);
                        return;
                    case Edge.Right:
                        gfx.TranslateTransform(fig.Width - Padding, fig.Height / 2);
                        gfx.RotateTransform(90);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(Text, font, brush, 0, 0, sf);
                        return;
                    case Edge.Top:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(Text, font, brush, fig.Width / 2, Padding, sf);
                        return;
                    case Edge.Bottom:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(Text, font, brush, fig.Width / 2, fig.Height - Padding, sf);
                        return;
                }
            }
        }
    }
}
