using ScottPlot.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class LegendItem
    {
        public string? Label { get; set; } // TODO: I waffled on whether to make these init-only setters. I think there's enough value to in letting them be mutable
        public Stroke? Line { get; set; }
        public Marker? Marker { get; set; }
        public Fill? Fill { get; set; }
        public IList<LegendItem> Children { get; set; } = Array.Empty<LegendItem>();
    }
}
