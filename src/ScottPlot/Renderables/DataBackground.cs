using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class DataBackground
    {
        public Color color = Color.White;

        public void Render(Bitmap bmp, Experimental.FigureInfo fig)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, fig.DataL, fig.DataT, fig.DataWidth, fig.DataHeight);
            }
        }
    }
}
