using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public interface IBitmapDemo
    {
        // For plots which cannot be rendered onto an existing plot.
        System.Drawing.Bitmap Render(int width, int height);
    }
}
