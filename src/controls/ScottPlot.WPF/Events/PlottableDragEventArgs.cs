using ScottPlot.Plottable;
using System;
using System.Windows.Input;

namespace ScottPlot.WinForms.Events
{
    public delegate void PlottableDragEventHandler(object sender, PlottableDragEventArgs args);

    public class PlottableDragEventArgs : System.Windows.Input.MouseEventArgs
    {
        /// <summary>
        /// Draggable which trigger this event.
        /// </summary>
        public IDraggable Draggable { get; }


        internal PlottableDragEventArgs(IDraggable draggable, MouseEventArgs source)
            : base(source.MouseDevice, source.Timestamp, source.StylusDevice)
        {
            Draggable = draggable;
        }
    }
}
