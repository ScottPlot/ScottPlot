namespace ScottPlot.Plottable
{
    /// <summary>
    /// Vertical line at an X position
    /// </summary>
    public class VLine : AxisLine
    {
        /// <summary>
        /// X position to render the line
        /// </summary>
        public double X { get => Position; set => Position = value; }
        public override string ToString() => $"Vertical line at X={X}";
        public VLine() : base(false) { }
    }
}
