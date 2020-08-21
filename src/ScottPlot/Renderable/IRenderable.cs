using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Renderable
{
    public interface IRenderable
    {
        bool IsVisible { get; set; }
        void Render(Settings settings);
    }
}
