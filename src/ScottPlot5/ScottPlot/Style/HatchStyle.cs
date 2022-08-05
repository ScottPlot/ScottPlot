using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public enum HatchStyle
    {
        None, // TODO: Hatched fills are non-trivial in Skia, you need to create a SKPathEffect: https://docs.microsoft.com/en-us/dotnet/api/skiasharp.skpatheffect?view=skiasharp-2.88
    }
}
