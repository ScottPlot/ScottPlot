namespace ScottPlot.Ticks
{
    public struct Tick
    {
        public readonly double Position;
        public readonly string Label;
        public readonly bool IsMajor;
        public bool IsMinor { get { return !IsMajor; } }

        public Tick(double position)
        {
            Position = position;
            Label = null;
            IsMajor = false;
        }

        public Tick(double position, string label, bool isMajor = true)
        {
            Position = position;
            Label = label;
            IsMajor = isMajor;
        }
    }
}
