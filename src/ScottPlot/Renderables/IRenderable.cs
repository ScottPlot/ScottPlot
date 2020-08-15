using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderables
{
    public interface IRenderable
    {
        void Render(System.Drawing.Bitmap bmp, Settings settings);
    }
}