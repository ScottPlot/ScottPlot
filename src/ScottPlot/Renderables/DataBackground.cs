using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class DataBackground
    {
        public Color color = Color.White;

        public void Render(Bitmap bmp, Settings settings)
        {
            using (var gfx = Graphics.FromImage(bmp))
            using (var brush = new SolidBrush(color))
            {
                float x = settings.dataOrigin.X;
                float y = settings.dataOrigin.Y;
                float w = settings.dataSize.Width;
                float h = settings.dataSize.Height;

                gfx.FillRectangle(brush, x, y, w, h);
            }
        }
    }
}
