using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess.Events
{
    public class PlottableDragModernEvent: IUIEvent
    {
        private readonly double XFrom;
        private readonly double XTo;
        private readonly double YFrom;
        private readonly double YTo;
        private readonly IDraggableModern PlottableBeingDragged;
        private readonly bool ShiftDown;
        private readonly Configuration Configuration;
        public RenderType RenderType => Configuration.QualityConfiguration.MouseInteractiveDragged;

        public PlottableDragModernEvent(double xFrom, double xTo, double yFrom, double yTo,bool shiftDown, IDraggableModern plottable, Configuration config)
        {
            XFrom = xFrom;
            XTo = xTo;
            YFrom = yFrom;
            YTo = yTo;
            ShiftDown = shiftDown;
            PlottableBeingDragged = plottable;
            Configuration = config;
        }

        public void ProcessEvent()
        {
            PlottableBeingDragged.Drag(XFrom, XTo, YFrom, YTo, fixedSize: ShiftDown);
        }
    }
}
