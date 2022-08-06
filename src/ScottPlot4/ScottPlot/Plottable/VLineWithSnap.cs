namespace ScottPlot.Plottable
{
    public class VLineWithSnap : AxisLineWithSnap
    {
        /// <summary>
        /// X position to render the line
        /// </summary>
        public double X { get => Position; set => Position = value; }

        public double[] Xs { get => Positions; set => Positions = value; }

        public override string ToString() => $"Vertical line with snap at X={X}";

        public VLineWithSnap() : base(false) { }
    }
}
