using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Config
{
    public class LegendItem
    {
        public string label;
        public System.Drawing.Color color;
        public LineStyle lineStyle;

        public LegendItem(string label, System.Drawing.Color color, LineStyle lineStyle)
        {
            this.label = label;
            this.color = color;
            this.lineStyle = lineStyle;
        }
    }
}
