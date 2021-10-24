namespace ScottPlot.Plottable
{
    /// <summary>
    /// Horizontal line at a Y position
    /// </summary>
    public class HLine : AxisLine
    {
        /// <summary>
        /// Y position to render the line
        /// </summary>
        public double Y { get => Position; set => Position = value; }
        public override string ToString() => $"Horizontal line at Y={Y}";
        public HLine() : base(true) { }
    }
}
