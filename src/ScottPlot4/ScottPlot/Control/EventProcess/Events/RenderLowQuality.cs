using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess.Events
{
    class RenderLowQuality : IUIEvent
    {
        public RenderType RenderType => RenderType.LowQuality;

        public void ProcessEvent()
        {
        }
    }
}
