using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public interface IDraggableSpan
    {
        public event EventHandler<double> Edge1Dragged;
        public event EventHandler<double> Edge2Dragged;
        public event EventHandler<double> MinDragged;
        public event EventHandler<double> MaxDragged;
    }
}
