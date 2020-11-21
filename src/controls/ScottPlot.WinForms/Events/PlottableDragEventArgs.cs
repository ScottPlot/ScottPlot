using System;
using System.Windows.Forms;
using ScottPlot.Plottable;

namespace ScottPlot.WinForms.Events
{
    public delegate void PlottableDragEventHandler(object sender, PlottableDragEventArgs args);

    public class PlottableDragEventArgs : System.Windows.Forms.MouseEventArgs
    {
        /// <summary>
        /// Draggable which trigger this event.
        /// </summary>
        public IDraggable Draggable { get; }


        internal PlottableDragEventArgs(IDraggable draggable, MouseEventArgs source)
            : base(source.Button, source.Clicks, source.X, source.Y, source.Delta)
        {
            Draggable = draggable;
        }
    }
}
