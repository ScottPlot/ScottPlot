namespace ScottPlot.Plottable
{
    public class HLineWithSnap : AxisLineWithSnap
    {
        /// <summary>
        /// Y position to render the line
        /// </summary>
        public double Y { get => Position; set => Position = value; }

        public double[] Ys { get => Positions; set => Positions = value; }

        public override string ToString() => $"Horizontal line with snap at Y={Y}";

        public HLineWithSnap() : base(true) { }
    }
}
