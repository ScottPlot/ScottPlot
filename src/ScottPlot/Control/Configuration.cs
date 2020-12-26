using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Control
{
    public class Configuration
    {
        public QualityMode Quality = QualityMode.LowWhileDragging;

        public bool LeftClickDragPan = true;
        public bool RightClickDragZoom = true;
        public bool MiddleClickDragZoom = true;
        public bool MiddleClickAutoAxis = true;
        public bool ScrollWheelZoom = true;
        public bool DoubleClickBenchmark = true;

        public bool LockVerticalAxis = false;
        public bool LockHorizontalAxis = false;

        public bool RenderIfPlottableCountChanges = true;
    }
}
