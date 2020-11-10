namespace ScottPlot
{
    /// <summary>
    /// Immutable information about an axis and methods to provide pixel/unit conversions
    /// </summary>
    public class PlotDimensions1D
    {
        public readonly float FigureSize;
        public readonly float DataSize;
        public readonly float DataOffset;
        public readonly double Min;
        public readonly double Max;
        public readonly double Center;
        public readonly double Span;
        public readonly double PxPerUnit;
        public readonly double UnitsPerPx;
        public readonly bool IsInverted;

        public PlotDimensions1D(float figureSize, float dataSize, float dataOffset, double min, double max, bool isInverted)
        {
            FigureSize = figureSize;
            DataSize = dataSize;
            DataOffset = dataOffset;
            Min = min;
            Max = max;
            IsInverted = isInverted;
            Center = (min + max) / 2;
            Span = max - min;
            PxPerUnit = DataSize / Span;
            UnitsPerPx = Span / DataSize;
        }

        public float GetPixel(double unit) =>
            (float)(DataOffset + ((IsInverted ? Max - unit : unit - Min) * PxPerUnit));

        public double GetUnit(float pixel) =>
            IsInverted ? DataSize - ((pixel - Min) * PxPerUnit) : (pixel - DataOffset) / PxPerUnit + Min;
    }
}
