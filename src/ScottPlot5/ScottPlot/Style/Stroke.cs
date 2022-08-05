using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public struct Stroke
    {
        public Color Color { get; set; } = NamedColors.WebColors.Black;
        public double Width { get; set; } = 1;

        public Stroke() { }
    }
}
