using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderables
{
    public class FigureBackground : IRenderable
    {
        public Color color = Color.White;

        public void Render(Settings settings)
        {
            using (var gfx = Graphics.FromImage(settings.Bitmap))
            {
                gfx.Clear(color);
            }
        }
    }
}
