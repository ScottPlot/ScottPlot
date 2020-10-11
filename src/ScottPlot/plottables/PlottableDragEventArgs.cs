using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public delegate void PlottableDragEventHandler(object sender, PlottableDragEventArgs args);

    public enum PlottableDragEventType
    {
        MouseDown,
        MouseUp,
        MouseDrag
    }

    public class PlottableDragEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="ScottPlot.Plot"/> that <see cref="PlottableDragEventArgs.Draggable"/> belongs to.
        /// /// </summary>
        public Plot Plot { get; }

        /// <summary>
        /// Draggable which trigger this event.
        /// </summary>
        public IDraggable Draggable { get; }

        /// <summary>
        /// The PlottableDragEventType
        /// </summary>
        public PlottableDragEventType EventType { get; }

        /// <summary>
        /// Source mouse event arguments.
        /// In WinForm, it is System.Windows.Forms.MouseEventArgs
        /// In WPF, it is System.Windows.Input.MouseEventArgs
        /// In Avalonia, it is null
        /// </summary>
        public object SourceEventArgs { get; }

        public PlottableDragEventArgs(Plot plot, IDraggable draggable, PlottableDragEventType type, object source)
        {
            Plot = plot;
            Draggable = draggable;
            EventType = type;
            SourceEventArgs = source;
        }
    }
}
