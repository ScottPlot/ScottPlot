using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.LayoutSystem
{
    public interface IPanel
    {
        float Measure();
        Edge Edge { get; }
        void Render(SkiaSharp.SKSurface surface, PixelRect rect);
    }
    
    // Default interface implentationss aren't a thing yet on all of our targeted platforms :/
    public static class IPanelExtensions
    {
        public static bool IsHorizontal(this IPanel panel) => panel.Edge == Edge.Bottom || panel.Edge == Edge.Top;
        public static bool IsVertical(this IPanel panel) => panel.Edge == Edge.Bottom || panel.Edge == Edge.Top;
    }
}
