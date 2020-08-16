using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class DataBackground
    {
        public Color color = Color.White;

        public void Render(Settings settings)
        {
            using (var gfx = Graphics.FromImage(settings.Bitmap))
            using (var brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, settings.DataL, settings.DataT, settings.DataWidth, settings.DataHeight);
            }
        }
    }
}
