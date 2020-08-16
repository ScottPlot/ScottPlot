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
        public int Padding = 0;
        public string Text;
        public FontStyle FontStyle = FontStyle.Regular;
        public bool CenterToDataArea = true;

        public void Render(Bitmap bmp, FigureInfo fig)
        {
            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Font font = new Font(Config.Fonts.GetDefaultFontName(), 12, FontStyle))
            using (Brush brush = new SolidBrush(Color.Black))
            using (StringFormat sf = new StringFormat())
            {
                float centerX = CenterToDataArea ? fig.DataWidth / 2 + fig.DataL : fig.Width / 2;
                float centerY = CenterToDataArea ? fig.DataHeight / 2 + fig.DataT : fig.Height / 2;

                switch (Edge)
                {
                    case Edge.Left:
                        gfx.TranslateTransform(Padding, centerY);
                        gfx.RotateTransform(-90);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(Text, font, brush, 0, 0, sf);
                        return;
                    case Edge.Right:
                        gfx.TranslateTransform(fig.Width - Padding, centerY);
                        gfx.RotateTransform(90);
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(Text, font, brush, 0, 0, sf);
                        return;
                    case Edge.Top:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Near;
                        gfx.DrawString(Text, font, brush, centerX, Padding, sf);
                        return;
                    case Edge.Bottom:
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Far;
                        gfx.DrawString(Text, font, brush, centerX, fig.Height - Padding, sf);
                        return;
                }
            }
        }
    }
}
