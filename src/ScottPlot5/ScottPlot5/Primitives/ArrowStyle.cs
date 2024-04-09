using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Primitives;

public enum ArrowAnchor
{
   Center,
   Tip,
   Tail,
}

public struct ArrowStyle
{

    public LineStyle LineStyle { get; set; } = new();
    public ArrowAnchor Anchor { get; set; } = ArrowAnchor.Center;

    public ArrowStyle(LineStyle lineStyle, ArrowAnchor anchor)
    {
        LineStyle = lineStyle;
        Anchor = anchor;
    }

    public ArrowStyle()
    {
    }
}
