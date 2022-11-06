using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style.Hatches
{
    public interface IHatch
    {
        SKShader GetShader(Color backgroundColor, Color hatchColor);
    }
}
