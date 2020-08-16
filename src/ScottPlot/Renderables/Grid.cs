using ScottPlot.Experimental;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class Grid : IRenderable
    {
        public bool Horizontal;
        public Color color = Color.LightGray;

        public void Render(Bitmap bmp, FigureInfo fig)
        {
            if (Horizontal)
                RenderHorizontal(bmp, fig);
            else
                RenderVertical(bmp, fig);
        }

        private void RenderHorizontal(Bitmap bmp, FigureInfo fig)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(color, 1))
            {
                for (int i = 0; i < fig.xMajorTicks.Length; i++)
                {
                    float pxX = (float)fig.PixelX(fig.xMajorTicks[i]);
                    gfx.DrawLine(pen, pxX, fig.DataT, pxX, fig.DataB);
                }
            }
        }

        private void RenderVertical(Bitmap bmp, FigureInfo fig)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var pen = new Pen(color, 1))
            {
                for (int i = 0; i < fig.yMajorTicks.Length; i++)
                {
                    float pxY = (float)fig.PixelY(fig.yMajorTicks[i]);
                    gfx.DrawLine(pen, fig.DataL, pxY, fig.DataR, pxY);
                }
            }
        }
    }
}
