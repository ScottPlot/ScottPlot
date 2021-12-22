namespace ScottPlot.Plottable
{
    /// <summary>
    /// This interface is for plottable objects that could be styled using the plot's style. 
    /// Typically this is for things like frames, tick marks, and text labels.
    /// </summary>
    public interface IStylable
    {
        public void SetStyle(System.Drawing.Color? tickMarkColor, System.Drawing.Color? tickFontColor);
    }
}
