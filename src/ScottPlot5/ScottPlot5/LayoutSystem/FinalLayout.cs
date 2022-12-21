using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.LayoutSystem
{
    public struct PanelWithOffset
    {
        public IPanel Panel { get; private set; }
        public PixelSize Offset { get; private set; }

        public PanelWithOffset(IPanel panel, PixelSize offset)
        {
            Panel = panel;
            Offset = offset;
        }
    }

    public struct FinalLayout
    {
        public PixelRect Area { get; private set; }
        public IEnumerable<PanelWithOffset> Panels { get; private set; }

        public FinalLayout(PixelRect area, IEnumerable<PanelWithOffset> panels)
        {
            Area = area;
            Panels = panels;
        }
    }
}
