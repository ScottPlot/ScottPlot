using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess.Events
{
    class RenderHighQuality : IUIEvent
    {
        public RenderType RenderType => RenderType.HighQuality;

        public void ProcessEvent()
        {
        }
    }
}
