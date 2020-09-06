using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class FigureBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;

        public void Render(Settings settings)
        {
            if (settings.gfxFigure is null)
                return;

            settings.gfxFigure.Clear(Color);
        }
    }
}
