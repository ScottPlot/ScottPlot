using ScottPlot.Experimental;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class DataFrame : IRenderable
    {
        public Color color = Color.Black;
        public bool Left = true;
        public bool Right = true;
        public bool Top = true;
        public bool Bottom = true;

        public void Render(Bitmap bmp, FigureInfo fig)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(color, 1))
            {
                if (Left)
                    gfx.DrawLine(pen, fig.DataL, fig.DataT, fig.DataL, fig.DataB);
                if (Right)
                    gfx.DrawLine(pen, fig.DataR, fig.DataT, fig.DataR, fig.DataB);
                if (Top)
                    gfx.DrawLine(pen, fig.DataL, fig.DataT, fig.DataR, fig.DataT);
                if (Bottom)
                    gfx.DrawLine(pen, fig.DataL, fig.DataB, fig.DataR, fig.DataB);
            }
        }
    }
}
