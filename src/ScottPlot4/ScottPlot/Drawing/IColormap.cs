using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Drawing
{
    public interface IColormap
    {
        (byte r, byte g, byte b) GetRGB(byte value);
        string Name { get; }
    }
}
