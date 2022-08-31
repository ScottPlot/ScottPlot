using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Colormaps
{
    public interface IColormap
    {
        string Name { get; }
        Color GetColor(double intensity, Range? domain);
    }
}
