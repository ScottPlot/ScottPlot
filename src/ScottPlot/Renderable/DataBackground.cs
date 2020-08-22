using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Renderable
{
    public class DataBackground : IRenderable
    {
        public Color Color { get; set; } = Color.White;

        public void Render(Settings settings)
        {
            if (settings.gfxData is null)
                return;

            settings.gfxData.Clear(Color);
        }
    }
}
