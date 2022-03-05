using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess.Events
{
    class RenderDelayedHighQuality : IUIEvent
    {
        public RenderType RenderType => RenderType.LowQualityThenHighQualityDelayed;

        public void ProcessEvent()
        {
        }
    }
}
