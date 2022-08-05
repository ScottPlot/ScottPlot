using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Style
{
    public struct Fill
    {
        public Color Color { get; set; } = Colors.CornflowerBlue;
        public Color HatchColor { get; set; } = Colors.Transparent;
        public HatchStyle HatchStyle { get; set; } = HatchStyle.None;

        public Fill() { }
    }
}
