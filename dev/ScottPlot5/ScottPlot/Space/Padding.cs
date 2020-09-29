namespace ScottPlot.Space
{
    public struct Padding
    {
        public float Left;
        public float Right;
        public float Below;
        public float Above;

        public float TotalHorizontal { get { return Left + Right; } }
        public float TotalVertical { get { return Above + Below; } }
    }
}
