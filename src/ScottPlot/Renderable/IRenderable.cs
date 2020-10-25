using ScottPlot.Drawing;
using System;
using System.Drawing;

namespace ScottPlot.Renderable
{
    /// <summary>
    /// A "renderable" is any object which can be drawn on the figure.
    /// </summary>
    public interface IRenderable
    {
        bool IsVisible { get; set; }
        void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);
    }
}
